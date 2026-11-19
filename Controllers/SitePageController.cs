using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

// Serves database-backed pages at /{slug}. Mapped as the fallback route in
// Program.cs, so it only runs when no other controller claims the URL.
public class SitePageController : Controller
{
    private readonly BlogDbContext _db;

    public SitePageController(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Show(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return NotFound();

        var page = await _db.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Slug == slug);

        // Unpublished pages are visible to the logged-in admin only, as a preview.
        if (page == null || (!page.Published && User.Identity?.IsAuthenticated != true))
            return NotFound();

        ViewData["ContentHtml"] = DatabasePostRepository.RenderMarkdown(page.Content);

        // The blogroll list under the /feeds intro comes from NetNewsWire (/admin/blogroll),
        // not from the page's markdown.
        if (page.Slug == "feeds")
        {
            var feeds = await _db.BlogrollFeeds.AsNoTracking().ToListAsync();
            ViewData["Blogroll"] = BlogrollOpml.Categories
                .Select(c => (Category: c, Feeds: feeds.Where(f => f.Category == c)
                    .OrderBy(f => f.Title, StringComparer.OrdinalIgnoreCase).ToList()))
                .Where(g => g.Feeds.Count > 0)
                .ToList();
        }

        return View(page);
    }
}
