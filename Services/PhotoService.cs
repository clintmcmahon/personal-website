using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.Caching.Memory;
using Website.Models;

namespace Website.Services;

// Latest post from the photoblog at photos.clintmcmahon.com. The RSS feed supplies the title,
// image and caption; the location only exists on the permalink page (JSON-LD contentLocation),
// so that gets a second request. Cached so the page never waits on the photoblog per visit.
public class PhotoService
{
    private const string CacheKey = "photo:latest";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    private static readonly Regex ImgTag = new(@"<img\b[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex SrcAttr = new(@"\bsrc\s*=\s*""([^""]+)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private static readonly Regex JsonLd = new(@"<script[^>]*application/ld(?:\+|&#x2B;)json[^>]*>(.*?)</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly Regex AnyTag = new(@"<(/?)([a-zA-Z0-9]+)([^>]*)>", RegexOptions.Compiled);
    private static readonly Regex HrefAttr = new(@"\bhref\s*=\s*""([^""]*)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PhotoService> _logger;

    public PhotoService(IHttpClientFactory httpClientFactory, IMemoryCache cache, ILogger<PhotoService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
    }

    public async Task<LatestPhoto?> GetLatestAsync()
    {
        if (_cache.TryGetValue(CacheKey, out LatestPhoto? cached))
            return cached;

        var result = await FetchAsync();

        // Cache misses briefly so a down photoblog isn't hit on every request.
        _cache.Set(CacheKey, result, result != null ? CacheDuration : TimeSpan.FromMinutes(1));
        return result;
    }

    private async Task<LatestPhoto?> FetchAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Photos");
            var feed = await client.GetStringAsync($"{CanonicalUrlHelper.PhotoBaseUrl}/rss");
            var item = XDocument.Parse(feed).Descendants("item").FirstOrDefault();
            if (item == null) return null;

            var title = (string?)item.Element("title") ?? "Untitled";
            var link = (string?)item.Element("link");
            var description = (string?)item.Element("description") ?? "";
            if (!IsPhotoblogUrl(link)) return null;

            var images = ImgTag.Matches(description);
            var src = images.Count > 0 ? SrcAttr.Match(images[0].Value).Groups[1].Value : null;
            if (!IsPhotoblogUrl(src)) return null;

            // pubDate is midnight GMT; keep it UTC so local conversion doesn't roll the date back a day.
            DateTime.TryParse((string?)item.Element("pubDate"), System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AdjustToUniversal | System.Globalization.DateTimeStyles.AssumeUniversal,
                out var published);

            var caption = Sanitize(ImgTag.Replace(description, ""));
            var location = await FetchLocationAsync(client, link!);

            return new LatestPhoto(title, link!, src!, caption, location, published, Math.Max(images.Count, 1));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Latest photo fetch failed");
            return null;
        }
    }

    private async Task<string?> FetchLocationAsync(HttpClient client, string permalink)
    {
        try
        {
            var html = await client.GetStringAsync(permalink);
            foreach (Match m in JsonLd.Matches(html))
            {
                using var doc = JsonDocument.Parse(m.Groups[1].Value);
                if (doc.RootElement.TryGetProperty("contentLocation", out var place) &&
                    place.TryGetProperty("name", out var name))
                {
                    var value = name.GetString();
                    if (!string.IsNullOrWhiteSpace(value)) return value;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Photo location fetch failed");
        }
        return null;
    }

    private static bool IsPhotoblogUrl(string? url) =>
        url != null && url.StartsWith(CanonicalUrlHelper.PhotoBaseUrl + "/", StringComparison.Ordinal);

    // The caption is remote HTML rendered with Html.Raw, so rebuild it from an allowlist:
    // paragraph and inline formatting tags with no attributes, plus http(s) links.
    // Everything else is dropped and text is re-encoded.
    private static string? Sanitize(string html)
    {
        var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "p", "br", "em", "strong", "i", "b", "a" };
        var sb = new System.Text.StringBuilder();
        var pos = 0;

        foreach (Match m in AnyTag.Matches(html))
        {
            sb.Append(WebUtility.HtmlEncode(WebUtility.HtmlDecode(html[pos..m.Index])));
            pos = m.Index + m.Length;

            var closing = m.Groups[1].Value == "/";
            var tag = m.Groups[2].Value.ToLowerInvariant();
            if (!allowed.Contains(tag)) continue;

            if (closing) { sb.Append($"</{tag}>"); continue; }
            if (tag == "a")
            {
                var href = HrefAttr.Match(m.Groups[3].Value).Groups[1].Value;
                href = WebUtility.HtmlDecode(href);
                if (Uri.TryCreate(href, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https")
                    sb.Append($"<a href=\"{WebUtility.HtmlEncode(uri.AbsoluteUri)}\" rel=\"noopener\">");
                else
                    sb.Append("<a>");
            }
            else sb.Append($"<{tag}>");
        }
        sb.Append(WebUtility.HtmlEncode(WebUtility.HtmlDecode(html[pos..])));

        // The feed double-wraps captions in <p><p>; collapse that and drop empty paragraphs.
        var result = Regex.Replace(sb.ToString(), @"<p>\s*<p>", "<p>");
        result = Regex.Replace(result, @"</p>\s*</p>", "</p>");
        result = Regex.Replace(result, @"<p>\s*</p>", "");
        return string.IsNullOrWhiteSpace(result) ? null : result.Trim();
    }
}
