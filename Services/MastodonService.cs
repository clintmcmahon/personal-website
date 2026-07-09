using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using Website.Models;

namespace Website.Services;

public class MastodonService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MastodonService> _logger;
    private readonly string _instanceUrl;
    private readonly string _accessToken;
    private readonly string _blogBaseUrl;
    private readonly string _photoBaseUrl;

    public MastodonService(
        IHttpClientFactory httpClientFactory,
        IWebHostEnvironment env,
        IConfiguration config,
        ILogger<MastodonService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _env = env;
        _logger = logger;
        _instanceUrl = (config["Mastodon:InstanceUrl"] ?? "").TrimEnd('/');
        _accessToken = config["Mastodon:AccessToken"] ?? "";
        _blogBaseUrl = (config["Mastodon:BlogBaseUrl"] ?? "https://clintmcmahon.com").TrimEnd('/');
        _photoBaseUrl = (config["Mastodon:PhotoBaseUrl"] ?? "https://photos.clintmcmahon.com").TrimEnd('/');
    }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(_instanceUrl) && !string.IsNullOrWhiteSpace(_accessToken);

    public async Task PostBlogPostAsync(Post post)
    {
        if (!IsConfigured) return;

        var url = $"{_blogBaseUrl}/Blog/{post.Slug}";
        var tags = FormatTags(post.Tags ?? new List<string>());
        var status = BuildStatus(post.Title, url, tags);

        await PublishStatusAsync(status);
    }

    public async Task PostPhotoAsync(PhotoEntry photo)
    {
        if (!IsConfigured) return;

        var url = $"{_photoBaseUrl}/{photo.Date:yyyy-MM-dd}";

        // Always include #photography for discoverability
        var tagList = (photo.Tags ?? new List<string>()).ToList();
        if (!tagList.Any(t => t.Equals("photography", StringComparison.OrdinalIgnoreCase)))
            tagList.Add("photography");

        var tags = FormatTags(tagList);
        var title = string.IsNullOrWhiteSpace(photo.Title)
            ? photo.Date.ToString("MMMM d, yyyy")
            : photo.Title;

        var status = BuildStatus(title, url, tags);

        // Upload the first image
        string? mediaId = null;
        if (!string.IsNullOrWhiteSpace(photo.ImageUrl))
        {
            var localPath = Path.Combine(
                _env.WebRootPath,
                photo.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(localPath))
            {
                var altText = photo.Rows?.FirstOrDefault()?.FirstOrDefault()?.Alt;
                if (string.IsNullOrWhiteSpace(altText))
                    altText = photo.Title;

                mediaId = await UploadMediaAsync(localPath, altText ?? title);
            }
            else
            {
                _logger.LogWarning("Mastodon photo upload: image not found at {Path}", localPath);
            }
        }

        var mediaIds = mediaId != null ? new List<string> { mediaId } : null;
        await PublishStatusAsync(status, mediaIds);
    }

    private async Task<string?> UploadMediaAsync(string filePath, string altText)
    {
        try
        {
            var client = CreateClient();

            await using var fileStream = File.OpenRead(filePath);
            using var form = new MultipartFormDataContent();
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            form.Add(fileContent, "file", Path.GetFileName(filePath));

            if (!string.IsNullOrWhiteSpace(altText))
                form.Add(new StringContent(altText[..Math.Min(altText.Length, 1500)]), "description");

            var response = await client.PostAsync($"{_instanceUrl}/api/v1/media", form);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Mastodon media upload failed ({Status}): {Body}",
                    response.StatusCode, await response.Content.ReadAsStringAsync());
                return null;
            }

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return doc.RootElement.GetProperty("id").GetString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mastodon media upload threw");
            return null;
        }
    }

    private async Task PublishStatusAsync(string content, List<string>? mediaIds = null)
    {
        try
        {
            var client = CreateClient();

            var form = new List<KeyValuePair<string, string>>
            {
                new("status", content),
                new("visibility", "public")
            };

            if (mediaIds != null)
                foreach (var id in mediaIds)
                    form.Add(new("media_ids[]", id));

            var response = await client.PostAsync(
                $"{_instanceUrl}/api/v1/statuses",
                new FormUrlEncodedContent(form));

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning("Mastodon status post failed ({Status}): {Body}",
                    response.StatusCode, await response.Content.ReadAsStringAsync());
            else
                _logger.LogInformation("Mastodon: posted — {Content}", content[..Math.Min(content.Length, 80)]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mastodon status post threw");
        }
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("Mastodon");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _accessToken);
        return client;
    }

    private static string BuildStatus(string title, string url, string tags)
    {
        var parts = new List<string> { title, url };
        if (!string.IsNullOrWhiteSpace(tags))
            parts.Add(tags);
        return string.Join("\n\n", parts);
    }

    private static string FormatTags(List<string> tags) =>
        string.Join(" ", tags
            .Select(t => "#" + Regex.Replace(t.ToLowerInvariant(), @"[^a-z0-9]", ""))
            .Where(t => t.Length > 1));
}
