namespace Website.Models;

// A standalone page (/now, or anything created at /admin/pages) whose content lives
// in blog.db instead of a .cshtml file, so it can be edited on the live site.
// Named SitePage because "Page" collides with Razor's own types.
public class SitePage
{
    public int Id { get; set; }

    // Single URL segment: the page is served at /{Slug}.
    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    // <title> override. Falls back to "{Title} | Clint McMahon".
    public string? MetaTitle { get; set; }

    public string? Description { get; set; }

    // Short line shown under the heading. The "now" page's tagline is also the
    // status line on the homepage.
    public string? Tagline { get; set; }

    // Portrait-style image above the heading (the About page photo).
    public string? HeaderImageUrl { get; set; }

    // Markdown. Raw HTML passes through.
    public string? Content { get; set; }

    public bool Published { get; set; }

    // Prints "Last updated: Month Year" from UpdatedAt above the content.
    public bool ShowLastUpdated { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
