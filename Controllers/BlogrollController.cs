using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Services;

namespace Website.Controllers;

// The blogroll as OPML, so readers can subscribe to all of it at once. Advertised
// with <link rel="blogroll"> in _Layout.cshtml.
public class BlogrollController : Controller
{
    private readonly BlogDbContext _db;

    public BlogrollController(BlogDbContext db)
    {
        _db = db;
    }

    [HttpGet("/blogroll.opml")]
    public async Task<IActionResult> Opml()
    {
        var feeds = await _db.BlogrollFeeds.AsNoTracking().ToListAsync();
        var ordered = BlogrollOpml.Categories
            .SelectMany(c => feeds.Where(f => f.Category == c).OrderBy(f => f.Title, StringComparer.OrdinalIgnoreCase));
        var updatedAt = feeds.Count > 0 ? feeds.Max(f => f.ImportedAt) : (DateTime?)null;

        var xml = BlogrollOpml.Build(ordered, updatedAt);
        return Content(xml.Declaration + "\n" + xml.Root, "text/x-opml; charset=utf-8");
    }
}
