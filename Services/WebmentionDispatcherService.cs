using Microsoft.EntityFrameworkCore;
using Website.Data;

namespace Website.Services;

// Polls for webmentions that are due (see WebmentionService.ScheduleAsync / SendDelay)
// and dispatches them, re-reading the entity's live content at send time so any edits
// made during the delay window are reflected.
public class WebmentionDispatcherService : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(15);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WebmentionDispatcherService> _logger;

    public WebmentionDispatcherService(IServiceScopeFactory scopeFactory, ILogger<WebmentionDispatcherService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
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

    private async Task DispatchDueAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var webmentionDb = scope.ServiceProvider.GetRequiredService<WebmentionDbContext>();
            var blogDb = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            var photoDb = scope.ServiceProvider.GetRequiredService<PhotoDbContext>();
            var webmentionService = scope.ServiceProvider.GetRequiredService<WebmentionService>();

            var due = await webmentionDb.PendingWebmentions
                .Where(p => !p.Sent && p.ScheduledFor <= DateTime.UtcNow)
                .ToListAsync(stoppingToken);

            foreach (var item in due)
            {
                try
                {
                    string? content = null;

                    if (item.EntityType == "Blog")
                    {
                        var post = await blogDb.Posts.AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Slug == item.EntityKey && !p.Draft, stoppingToken);
                        content = post?.Content;
                    }
                    else if (item.EntityType == "Photo" && DateTime.TryParse(item.EntityKey, out var targetDate))
                    {
                        var photo = await photoDb.Photos.AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Date.Date == targetDate.Date && !p.Draft, stoppingToken);
                        content = photo?.Content;
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
