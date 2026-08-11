using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Website.Models;

namespace Website.Services;

public class MastodonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MastodonService> _logger;
    private readonly string _instanceUrl;
    private readonly string _accessToken;

    public MastodonService(
        IHttpClientFactory httpClientFactory,
        IConfiguration config,
        ILogger<MastodonService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _instanceUrl = (config["Mastodon:InstanceUrl"] ?? "").TrimEnd('/');
        _accessToken = config["Mastodon:AccessToken"] ?? "";
    }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(_instanceUrl) && !string.IsNullOrWhiteSpace(_accessToken);

    public async Task<string?> PostBlogPostAsync(Post post)
    {
        if (!IsConfigured) return null;

        var url = CanonicalUrlHelper.BlogPost(post.Slug);
        var tags = FormatTags(post.Tags ?? new List<string>());
        var status = BuildStatus(post.Title, null, url, tags);

        return await PublishStatusAsync(status);
    }

    private async Task<string?> PublishStatusAsync(string content)
    {
        try
        {
            var client = CreateClient();

            var form = new List<KeyValuePair<string, string>>
            {
                new("status", content),
                new("visibility", "public")
            };

            var response = await client.PostAsync(
                $"{_instanceUrl}/api/v1/statuses",
                new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Mastodon status post failed ({Status}): {Body}",
                    response.StatusCode, await response.Content.ReadAsStringAsync());
                return null;
            }

            _logger.LogInformation("Mastodon: posted — {Content}", content[..Math.Min(content.Length, 80)]);

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement.TryGetProperty("url", out var urlProp) ? urlProp.GetString() : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mastodon status post threw");
            return null;
        }
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("Mastodon");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _accessToken);
        return client;
    }

    // Mastodon's default per-instance status limit; used to keep long captions from
    // pushing a post past the point where the server rejects it outright.
    private const int StatusCharLimit = 500;

    private static string BuildStatus(string title, string? body, string url, string tags)
    {
        var trimmedBody = string.IsNullOrWhiteSpace(body) ? null : body.Trim();

        var fixedParts = new List<string> { title, url };
        if (!string.IsNullOrWhiteSpace(tags))
            fixedParts.Add(tags);

        if (trimmedBody != null)
        {
            // Budget = limit minus everything else minus the two blank-line separators
            // the body's own paragraph would add.
            var budget = StatusCharLimit - string.Join("\n\n", fixedParts).Length - 4;
            if (budget < 1)
                trimmedBody = null;
            else if (trimmedBody.Length > budget)
                trimmedBody = trimmedBody[..(budget - 1)].TrimEnd() + "…";
        }

        var parts = new List<string> { title };
        if (trimmedBody != null)
            parts.Add(trimmedBody);
        parts.Add(url);
        if (!string.IsNullOrWhiteSpace(tags))
            parts.Add(tags);

        return string.Join("\n\n", parts);
    }

    // Inverse of AdminController.FormatCaptionHtml: <br> -> newline, paragraph breaks ->
    // blank line, strip remaining tags, decode entities.
    private static string FormatTags(List<string> tags) =>
        string.Join(" ", tags
            .Select(t => "#" + Regex.Replace(t.ToLowerInvariant(), @"[^a-z0-9]", ""))
            .Where(t => t.Length > 1));
}
