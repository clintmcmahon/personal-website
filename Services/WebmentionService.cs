using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Website.Data;
using Website.Models;

namespace Website.Services;

// Discovers and sends outbound webmentions for links found in a post/photo's
// own rendered HTML. Receiving is handled separately by webmention.io.
public class WebmentionService
{
    private static readonly HashSet<string> OwnHosts = new(StringComparer.OrdinalIgnoreCase)
    {
        "clintmcmahon.com", "photos.clintmcmahon.com"
    };

    private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(2);

    private static readonly Regex HrefRegex = new(
        "<a\\s[^>]*href=[\"']([^\"']+)[\"']", RegexOptions.IgnoreCase | RegexOptions.Compiled, RegexTimeout);

    private static readonly Regex LinkHeaderValueRegex = new(
        "<([^>]+)>\\s*;\\s*rel=[\"']?([^\"';]+)[\"']?", RegexOptions.IgnoreCase | RegexOptions.Compiled, RegexTimeout);

    private static readonly Regex HtmlRelThenHrefRegex = new(
        "<(?:link|a)\\b[^>]*\\brel=[\"']?[^\"'>]*\\bwebmention\\b[^\"'>]*[\"']?[^>]*\\bhref=[\"']([^\"']+)[\"']",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, RegexTimeout);

    private static readonly Regex HtmlHrefThenRelRegex = new(
        "<(?:link|a)\\b[^>]*\\bhref=[\"']([^\"']+)[\"'][^>]*\\brel=[\"']?[^\"'>]*\\bwebmention\\b[^\"'>]*[\"']?",
        RegexOptions.IgnoreCase | RegexOptions.Compiled, RegexTimeout);

    private const int MaxLinksPerPublish = 30;
    private const int MaxRedirects = 5;
    private const int MaxBodyBytes = 100_000;

    // Gives typos/broken links a chance to get fixed before anyone downstream is notified —
    // see https://en.andros.dev/blog/0b8e451e/i-joined-the-indieweb-heres-what-i-learned/
    public static readonly TimeSpan SendDelay = TimeSpan.FromHours(24);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WebmentionDbContext _db;
    private readonly ILogger<WebmentionService> _logger;

    public WebmentionService(IHttpClientFactory httpClientFactory, WebmentionDbContext db, ILogger<WebmentionService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _db = db;
        _logger = logger;
    }

    // Queues a webmention send for `SendDelay` from now instead of sending immediately.
    // WebmentionDispatcherService picks these up and re-reads the entity's *current*
    // content at send time, so edits/corrections made during the delay are reflected.
    public async Task ScheduleAsync(string entityType, string entityKey, string sourceUrl)
    {
        _db.PendingWebmentions.Add(new PendingWebmention
        {
            EntityType = entityType,
            EntityKey = entityKey,
            SourceUrl = sourceUrl,
            ScheduledFor = DateTime.UtcNow + SendDelay
        });
        await _db.SaveChangesAsync();
    }

    public async Task SendWebmentionsAsync(string sourceUrl, string htmlContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(htmlContent)) return;

            var client = _httpClientFactory.CreateClient("Webmention");
            var targets = ExtractOutboundLinks(htmlContent);

            var count = 0;
            foreach (var target in targets)
            {
                if (count >= MaxLinksPerPublish) break;
                count++;

                try
                {
                    await SendOneAsync(client, sourceUrl, target);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Webmention: failed processing target {Target}", target);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Webmention: SendWebmentionsAsync threw");
        }
    }

    private static List<string> ExtractOutboundLinks(string htmlContent)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var result = new List<string>();

        MatchCollection matches;
        try
        {
            matches = HrefRegex.Matches(htmlContent);
        }
        catch (RegexMatchTimeoutException)
        {
            return result;
        }

        foreach (Match m in matches)
        {
            var href = WebUtility.HtmlDecode(m.Groups[1].Value);
            if (!Uri.TryCreate(href, UriKind.Absolute, out var uri)) continue;
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) continue;
            if (OwnHosts.Contains(uri.Host)) continue;

            var normalized = uri.GetLeftPart(UriPartial.Query);
            if (seen.Add(normalized))
                result.Add(normalized);
        }

        return result;
    }

    private async Task SendOneAsync(HttpClient client, string sourceUrl, string targetUrl)
    {
        var endpoint = await DiscoverEndpointAsync(client, targetUrl);
        if (endpoint == null) return;
        if (!await IsPublicHostAsync(endpoint.Host)) return;

        var form = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("source", sourceUrl),
            new KeyValuePair<string, string>("target", targetUrl)
        });

        using var response = await client.PostAsync(endpoint, form);
        if (response.IsSuccessStatusCode)
            _logger.LogInformation("Webmention: sent {Source} -> {Target} via {Endpoint}", sourceUrl, targetUrl, endpoint);
        else
            _logger.LogInformation("Webmention: endpoint {Endpoint} responded {Status} for {Target}", endpoint, response.StatusCode, targetUrl);
    }

    private async Task<Uri?> DiscoverEndpointAsync(HttpClient client, string targetUrl)
    {
        var currentUrl = targetUrl;

        for (var hop = 0; hop <= MaxRedirects; hop++)
        {
            if (!Uri.TryCreate(currentUrl, UriKind.Absolute, out var currentUri)) return null;
            if (currentUri.Scheme != Uri.UriSchemeHttp && currentUri.Scheme != Uri.UriSchemeHttps) return null;
            if (!await IsPublicHostAsync(currentUri.Host)) return null;

            using var request = new HttpRequestMessage(HttpMethod.Get, currentUri);
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if ((int)response.StatusCode is >= 300 and < 400 && response.Headers.Location != null)
            {
                currentUrl = new Uri(currentUri, response.Headers.Location).ToString();
                continue;
            }

            if (!response.IsSuccessStatusCode) return null;

            if (response.Headers.TryGetValues("Link", out var linkValues))
            {
                foreach (var headerValue in linkValues)
                {
                    var fromHeader = ParseLinkHeaderForWebmention(headerValue, currentUri);
                    if (fromHeader != null) return fromHeader;
                }
            }

            var mediaType = response.Content.Headers.ContentType?.MediaType ?? "";
            if (!mediaType.Contains("html", StringComparison.OrdinalIgnoreCase)) return null;

            var body = await ReadCappedAsync(response.Content, MaxBodyBytes);
            return ParseHtmlForWebmention(body, currentUri);
        }

        return null;
    }

    private static Uri? ParseLinkHeaderForWebmention(string headerValue, Uri baseUri)
    {
        try
        {
            foreach (Match m in LinkHeaderValueRegex.Matches(headerValue))
            {
                var rels = m.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (rels.Any(r => r.Equals("webmention", StringComparison.OrdinalIgnoreCase))
                    && Uri.TryCreate(baseUri, m.Groups[1].Value, out var resolved))
                {
                    return resolved;
                }
            }
        }
        catch (RegexMatchTimeoutException) { }

        return null;
    }

    private static Uri? ParseHtmlForWebmention(string html, Uri baseUri)
    {
        try
        {
            var m = HtmlRelThenHrefRegex.Match(html);
            if (!m.Success) m = HtmlHrefThenRelRegex.Match(html);
            if (!m.Success) return null;

            var href = WebUtility.HtmlDecode(m.Groups[1].Value);
            return Uri.TryCreate(baseUri, href, out var resolved) ? resolved : null;
        }
        catch (RegexMatchTimeoutException)
        {
            return null;
        }
    }

    private static async Task<string> ReadCappedAsync(HttpContent content, int maxBytes)
    {
        await using var stream = await content.ReadAsStreamAsync();
        var buffer = new byte[maxBytes];
        var totalRead = 0;
        int read;
        while (totalRead < maxBytes &&
               (read = await stream.ReadAsync(buffer.AsMemory(totalRead, maxBytes - totalRead))) > 0)
        {
            totalRead += read;
        }

        return Encoding.UTF8.GetString(buffer, 0, totalRead);
    }

    private static async Task<bool> IsPublicHostAsync(string host)
    {
        if (IPAddress.TryParse(host, out var literalIp))
            return !IsPrivateOrLoopback(literalIp);

        IPAddress[] addresses;
        try
        {
            addresses = await Dns.GetHostAddressesAsync(host);
        }
        catch
        {
            return false;
        }

        return addresses.Length > 0 && addresses.All(a => !IsPrivateOrLoopback(a));
    }

    private static bool IsPrivateOrLoopback(IPAddress ip)
    {
        if (IPAddress.IsLoopback(ip)) return true;

        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            var b = ip.GetAddressBytes();
            if (b[0] == 10) return true;                          // 10.0.0.0/8
            if (b[0] == 172 && b[1] is >= 16 and <= 31) return true; // 172.16.0.0/12
            if (b[0] == 192 && b[1] == 168) return true;           // 192.168.0.0/16
            if (b[0] == 169 && b[1] == 254) return true;           // 169.254.0.0/16 (incl. cloud metadata)
            return false;
        }

        if (ip.AddressFamily == AddressFamily.InterNetworkV6)
        {
            if (ip.IsIPv6LinkLocal || ip.IsIPv6SiteLocal) return true;
            var b = ip.GetAddressBytes();
            if ((b[0] & 0xFE) == 0xFC) return true; // fc00::/7 unique local
            return false;
        }

        return true; // unknown address family — reject rather than risk it
    }
}
