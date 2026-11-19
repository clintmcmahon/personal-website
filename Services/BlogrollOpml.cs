using System.Xml;
using System.Xml.Linq;
using Website.Models;

namespace Website.Services;

// Reads a NetNewsWire OPML export into blogroll rows, and writes the blogroll back
// out as OPML for /blogroll.opml.
public static class BlogrollOpml
{
    // NetNewsWire folders that make up the blogroll, in display order. Everything
    // else in the export (news, sports, music...) is ignored.
    public static readonly string[] Categories = ["Blogs", "Photoblogs"];

    public static List<BlogrollFeed> Parse(Stream opml)
    {
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };
        using var reader = XmlReader.Create(opml, settings);
        var body = XDocument.Load(reader).Root?.Element("body")
            ?? throw new FormatException("Not an OPML file: no <body> element.");

        var now = DateTime.UtcNow;
        var feeds = new List<BlogrollFeed>();

        foreach (var category in Categories)
        {
            var folder = body.Elements("outline").FirstOrDefault(o =>
                o.Attribute("xmlUrl") == null &&
                string.Equals(Attr(o, "text") ?? Attr(o, "title"), category, StringComparison.OrdinalIgnoreCase));
            if (folder == null) continue;

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var o in folder.Descendants("outline"))
            {
                var feedUrl = Attr(o, "xmlUrl");
                if (feedUrl == null || !IsHttpUrl(feedUrl) || !seen.Add(feedUrl)) continue;

                var siteUrl = Attr(o, "htmlUrl");
                feeds.Add(new BlogrollFeed
                {
                    Category = category,
                    Title = Attr(o, "title") ?? Attr(o, "text") ?? feedUrl,
                    SiteUrl = siteUrl != null && IsHttpUrl(siteUrl) ? siteUrl : null,
                    FeedUrl = feedUrl,
                    ImportedAt = now,
                });
            }
        }

        return feeds;
    }

    public static XDocument Build(IEnumerable<BlogrollFeed> feeds, DateTime? updatedAt)
    {
        var body = new XElement("body");
        foreach (var group in feeds.GroupBy(f => f.Category))
        {
            body.Add(new XElement("outline",
                new XAttribute("text", group.Key),
                new XAttribute("title", group.Key),
                group.Select(f => new XElement("outline",
                    new XAttribute("type", "rss"),
                    new XAttribute("text", f.Title),
                    new XAttribute("title", f.Title),
                    new XAttribute("xmlUrl", f.FeedUrl),
                    f.SiteUrl != null ? new XAttribute("htmlUrl", f.SiteUrl) : null))));
        }

        var head = new XElement("head",
            new XElement("title", "Clint McMahon's blogroll"),
            new XElement("ownerName", "Clint McMahon"),
            new XElement("ownerId", "https://clintmcmahon.com/"));
        if (updatedAt != null)
            head.Add(new XElement("dateModified", updatedAt.Value.ToUniversalTime().ToString("R")));

        return new XDocument(new XDeclaration("1.0", "utf-8", null),
            new XElement("opml", new XAttribute("version", "2.0"), head, body));
    }

    private static string? Attr(XElement e, string name)
    {
        var value = e.Attribute(name)?.Value.Trim();
        return string.IsNullOrEmpty(value) ? null : value;
    }

    private static bool IsHttpUrl(string value) =>
        Uri.TryCreate(value, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
