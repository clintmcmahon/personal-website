using Microsoft.EntityFrameworkCore;
using Website.Data;

namespace Website.Services;

// Polls for webmentions that are due (see WebmentionService.ScheduleAsync / SendDelay)
// and dispatches them, re-reading the entity's live content at send time so any edits
// made during the delay window are reflected.
public class WebmentionDispatcherService : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(15);

    // Blogroll rows: EntityKey is the blog's homepage, and a row existing (sent or not)
    // is the record that the blog has been notified, so each one is notified only once
    // even if it's removed and re-added by a later OPML upload.
    private const string BlogrollEntityType = "Blogroll";
    private const string BlogrollPageSlug = "feeds";
    private const string BlogrollSourceUrl = "https://clintmcmahon.com/feeds";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WebmentionDispatcherService> _logger;

    private readonly IHostEnvironment _env;

    public WebmentionDispatcherService(IServiceScopeFactory scopeFactory, ILogger<WebmentionDispatcherService> logger, IHostEnvironment env)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _env = env;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Catch up on anything that came due while the app was stopped/recycled.
        await DispatchDueAsync(stoppingToken);

        try
        {
            using var timer = new PeriodicTimer(PollInterval);
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DispatchDueAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Expected on normal shutdown — PeriodicTimer.WaitForNextTickAsync throws
            // rather than returning false when the token is cancelled. Left unhandled,
            // this would be treated as a fatal BackgroundService failure and crash the
            // whole host (BackgroundServiceExceptionBehavior defaults to StopHost).
        }
    }

    // Queues a webmention for every blogroll site that hasn't had one, with the same
    // SendDelay as posts. Runs each tick, so the initial seed and later OPML uploads
    // are both picked up without the upload having to touch webmentions.db.
    private static async Task ScheduleBlogrollAsync(WebmentionDbContext webmentionDb, BlogDbContext blogDb, CancellationToken ct)
    {
        var sites = await blogDb.BlogrollFeeds.AsNoTracking()
            .Where(f => f.SiteUrl != null)
            .Select(f => f.SiteUrl!)
            .Distinct()
            .ToListAsync(ct);

        var known = (await webmentionDb.PendingWebmentions.AsNoTracking()
                .Where(p => p.EntityType == BlogrollEntityType)
                .Select(p => p.EntityKey)
                .ToListAsync(ct))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var scheduledFor = DateTime.UtcNow + WebmentionService.SendDelay;
        var added = 0;
        foreach (var site in sites.Where(s => !known.Contains(s)))
        {
            webmentionDb.PendingWebmentions.Add(new Models.PendingWebmention
            {
                EntityType = BlogrollEntityType,
                EntityKey = site,
                SourceUrl = BlogrollSourceUrl,
                ScheduledFor = scheduledFor,
            });
            added++;
        }

        if (added > 0)
            await webmentionDb.SaveChangesAsync(ct);
    }

    private async Task DispatchDueAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var webmentionDb = scope.ServiceProvider.GetRequiredService<WebmentionDbContext>();
            var blogDb = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            var webmentionService = scope.ServiceProvider.GetRequiredService<WebmentionService>();

            // Production only: a local copy of blog.db would otherwise queue the same
            // notifications on a dev machine and send them a second time.
            if (_env.IsProduction())
                await ScheduleBlogrollAsync(webmentionDb, blogDb, stoppingToken);

            var due = await webmentionDb.PendingWebmentions
                .Where(p => !p.Sent && p.ScheduledFor <= DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            foreach (var item in due)
            {
                try
                {
                    string? content = null;

                    // Photos moved to their own app with its own queue and dispatcher, so
                    // only "Blog" items are scheduled here now. Any "Photo" rows left over
                    // from before the split fall through with null content and get marked
                    // sent below rather than retrying forever.
                    if (item.EntityType == "Blog")
                    {
                        var post = await blogDb.Posts.AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Slug == item.EntityKey && !p.Draft, stoppingToken);
                        content = post?.Content;
                    }
                    else if (item.EntityType == BlogrollEntityType)
                    {
                        // Wait while /feeds is unpublished, since the receiver checks that the
                        // source page links to the target. Leaving the row unsent retries next tick.
                        var feedsLive = await blogDb.Pages.AnyAsync(p => p.Slug == BlogrollPageSlug && p.Published, stoppingToken);
                        if (!feedsLive) continue;

                        // Dropped from the blogroll during the delay: mark sent without notifying.
                        var stillListed = await blogDb.BlogrollFeeds.AnyAsync(f => f.SiteUrl == item.EntityKey, stoppingToken);
                        if (stillListed)
                            await webmentionService.SendToTargetAsync(item.SourceUrl, item.EntityKey);
                    }

                    // If the post was deleted or unpublished again during the delay window, skip sending.
                    if (!string.IsNullOrWhiteSpace(content))
                        await webmentionService.SendWebmentionsAsync(item.SourceUrl, content);

                    item.Sent = true;
                    item.SentAt = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Webmention dispatch failed for pending item {Id}", item.Id);
                    // Mark sent anyway so a permanently-broken item doesn't retry forever;
                    // SendWebmentionsAsync already swallows per-link failures internally.
                    item.Sent = true;
                    item.SentAt = DateTime.UtcNow;
                }
            }

            if (due.Count > 0)
                await webmentionDb.SaveChangesAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "WebmentionDispatcherService tick failed");
        }
    }
}
