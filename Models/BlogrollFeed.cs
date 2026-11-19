namespace Website.Models;

// One feed on the blogroll (/feeds and /blogroll.opml). The whole table is replaced
// each time an OPML export from NetNewsWire is uploaded at /admin/blogroll.
public class BlogrollFeed
{
    public int Id { get; set; }

    // The NetNewsWire folder it came from: "Blogs" or "Photoblogs".
    public string Category { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    // The site's homepage. Null when the OPML had none; the list links the feed instead.
    public string? SiteUrl { get; set; }

    public string FeedUrl { get; set; } = string.Empty;

    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
}
