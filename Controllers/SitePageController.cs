using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Repositories;

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
        return View(page);
    }
}
