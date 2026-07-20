using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Website.Models;

namespace Website.Services;

// Pulls likes/boosts/replies for a post's syndicated Mastodon copy (POSSE backfeed),
// server-side and cached, for display on the original post/photo page.
public class MastodonEngagementService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<MastodonEngagementService> _logger;

    public MastodonEngagementService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        ILogger<MastodonEngagementService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
    }

    public async Task<MastodonEngagement?> GetEngagementAsync(string? syndicationUrl)
    {
        if (string.IsNullOrWhiteSpace(syndicationUrl)) return null;

        var cacheKey = $"mastodon-engagement:{syndicationUrl}";
        if (_cache.TryGetValue(cacheKey, out MastodonEngagement? cached))
            return cached;

        var result = await FetchAsync(syndicationUrl);

        // Cache both hits and misses (null) so a bad/unreachable status doesn't
        // get re-fetched on every page view.
        _cache.Set(cacheKey, result, CacheDuration);
        return result;
    }

    private async Task<MastodonEngagement?> FetchAsync(string syndicationUrl)
    {
        try
        {
            if (!Uri.TryCreate(syndicationUrl, UriKind.Absolute, out var uri)) return null;

            var statusId = uri.Segments.LastOrDefault()?.Trim('/');
            if (string.IsNullOrWhiteSpace(statusId)) return null;

            var instanceBase = $"{uri.Scheme}://{uri.Host}";
            var client = _httpClientFactory.CreateClient("MastodonPublic");

            using var statusResponse = await client.GetAsync($"{instanceBase}/api/v1/statuses/{statusId}");
            if (!statusResponse.IsSuccessStatusCode) return null;

            using var statusDoc = JsonDocument.Parse(await statusResponse.Content.ReadAsStringAsync());
            var root = statusDoc.RootElement;

            var engagement = new MastodonEngagement
            {
                Likes = root.TryGetProperty("favourites_count", out var f) ? f.GetInt32() : 0,
                Boosts = root.TryGetProperty("reblogs_count", out var b) ? b.GetInt32() : 0,
                RepliesCount = root.TryGetProperty("replies_count", out var r) ? r.GetInt32() : 0,
            };

            if (engagement.Likes > 0)
                engagement.Likers = await FetchAccountsAsync(client, $"{instanceBase}/api/v1/statuses/{statusId}/favourited_by");

            if (engagement.Boosts > 0)
                engagement.Boosters = await FetchAccountsAsync(client, $"{instanceBase}/api/v1/statuses/{statusId}/reblogged_by");

            if (engagement.RepliesCount > 0)
            {
                using var contextResponse = await client.GetAsync($"{instanceBase}/api/v1/statuses/{statusId}/context");
                if (contextResponse.IsSuccessStatusCode)
                {
                    using var contextDoc = JsonDocument.Parse(await contextResponse.Content.ReadAsStringAsync());
                    if (contextDoc.RootElement.TryGetProperty("descendants", out var descendants))
                    {
                        foreach (var reply in descendants.EnumerateArray())
                        {
                            // Only direct replies to this status, not the whole subthread.
                            var inReplyTo = reply.TryGetProperty("in_reply_to_id", out var irt) && irt.ValueKind == JsonValueKind.String
                                ? irt.GetString()
                                : null;
                            if (inReplyTo != statusId) continue;

                            var account = reply.TryGetProperty("account", out var acc) ? acc : default;
                            var authorName = account.ValueKind == JsonValueKind.Object && account.TryGetProperty("display_name", out var dn) && !string.IsNullOrWhiteSpace(dn.GetString())
                                ? dn.GetString()!
                                : (account.ValueKind == JsonValueKind.Object && account.TryGetProperty("username", out var un) ? un.GetString() ?? "Anonymous" : "Anonymous");
                            var authorAvatar = account.ValueKind == JsonValueKind.Object && account.TryGetProperty("avatar", out var av) ? av.GetString() : null;
                            var authorUrl = account.ValueKind == JsonValueKind.Object && account.TryGetProperty("url", out var au) ? au.GetString() ?? "#" : "#";
                            var rawContent = reply.TryGetProperty("content", out var c) ? c.GetString() ?? "" : "";
                            // Preserve paragraph/line breaks as spaces, but strip inline tags (e.g. the
                            // <span> wrappers Mastodon puts around "@" and "handle" in mentions) with no
                            // separator so tokens like "@blogamf" don't get split apart.
                            var withBreaks = Regex.Replace(rawContent, "</p>|<br\\s*/?>", " ");
                            var content = System.Net.WebUtility.HtmlDecode(Regex.Replace(withBreaks, "<[^>]+>", "")).Trim();
                            content = Regex.Replace(content, @"\s+", " ");
                            var publishedAt = reply.TryGetProperty("created_at", out var ca) && DateTime.TryParse(ca.GetString(), out var pd) ? pd : DateTime.MinValue;
                            var replyUrl = reply.TryGetProperty("url", out var ru) ? ru.GetString() ?? "#" : "#";

                            engagement.Replies.Add(new MastodonReplyView(authorName, authorAvatar, authorUrl, content, publishedAt, replyUrl));
                        }
                    }
                }
            }

            return engagement;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Mastodon engagement fetch failed for {Url}", syndicationUrl);
            return null;
        }
    }

    private static async Task<List<MastodonAccountView>> FetchAccountsAsync(HttpClient client, string url)
    {
        var accounts = new List<MastodonAccountView>();

        try
        {
            using var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return accounts;

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            foreach (var account in doc.RootElement.EnumerateArray())
            {
                var name = account.TryGetProperty("display_name", out var dn) && !string.IsNullOrWhiteSpace(dn.GetString())
                    ? dn.GetString()!
                    : (account.TryGetProperty("username", out var un) ? un.GetString() ?? "Anonymous" : "Anonymous");
                var avatar = account.TryGetProperty("avatar", out var av) ? av.GetString() : null;
                var accountUrl = account.TryGetProperty("url", out var au) ? au.GetString() ?? "#" : "#";

                accounts.Add(new MastodonAccountView(name, avatar, accountUrl));
            }
        }
        catch
        {
            // Best-effort — a failed facepile fetch shouldn't affect the rest of the page.
        }

        return accounts;
    }
}
