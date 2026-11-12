using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers;

// Editor for the database-backed pages SitePageController serves at /{slug}.
[Authorize]
public class AdminPagesController : Controller
{
    private readonly BlogDbContext _db;
    private readonly IActionDescriptorCollectionProvider _actions;

    public AdminPagesController(BlogDbContext db, IActionDescriptorCollectionProvider actions)
    {
        _db = db;
        _actions = actions;
    }

    [HttpGet("/admin/pages")]
    public async Task<IActionResult> Index()
    {
        var pages = await _db.Pages.AsNoTracking().OrderBy(p => p.Title).ToListAsync();
        return View(pages);
    }

    [HttpGet("/admin/pages/new")]
    public IActionResult New()
    {
        return View("Edit", new SitePage());
    }

    [HttpPost("/admin/pages/new")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(SitePage input)
    {
        var page = new SitePage { CreatedAt = DateTime.UtcNow };
        if (!await ApplyAsync(page, input))
            return View("Edit", input);

        _db.Pages.Add(page);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{page.Title}\" saved.";
        return RedirectToAction(nameof(Edit), new { id = page.Id });
    }

    [HttpGet("/admin/pages/{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var page = await _db.Pages.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (page == null) return NotFound();

        return View(page);
    }

    [HttpPost("/admin/pages/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SitePage input)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page == null) return NotFound();

        if (!await ApplyAsync(page, input))
        {
            input.Id = id;
            return View(input);
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{page.Title}\" updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost("/admin/pages/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page == null) return NotFound();

        _db.Pages.Remove(page);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{page.Title}\" deleted.";
        return RedirectToAction(nameof(Index));
    }

    // Validates the form and copies it onto the tracked entity. Returns false, with
    // ModelState populated, when the page should not be saved.
    private async Task<bool> ApplyAsync(SitePage page, SitePage input)
    {
        // Validation is all done here. The binder's implicit [Required] on non-nullable
        // strings would otherwise reject a blank slug before it can be generated.
        ModelState.Clear();

        input.Title = input.Title?.Trim() ?? string.Empty;
        input.Slug = Slugify(string.IsNullOrWhiteSpace(input.Slug) ? input.Title : input.Slug);

        if (input.Title.Length == 0)
            ModelState.AddModelError("Title", "Title is required.");
        else if (input.Slug.Length == 0)
            ModelState.AddModelError("Slug", "Slug is required.");
        else if (ReservedSlugs().Contains(input.Slug))
            ModelState.AddModelError("Slug", $"/{input.Slug} is already used by another part of the site.");
        else if (await _db.Pages.AnyAsync(p => p.Slug == input.Slug && p.Id != page.Id))
            ModelState.AddModelError("Slug", "A page with this slug already exists.");

        if (!ModelState.IsValid) return false;

        page.Title = input.Title;
        page.Slug = input.Slug;
        page.MetaTitle = input.MetaTitle;
        page.Description = input.Description;
        page.Tagline = input.Tagline;
        page.HeaderImageUrl = string.IsNullOrWhiteSpace(input.HeaderImageUrl) ? null : input.HeaderImageUrl.Trim();
        page.Content = input.Content;
        page.Published = input.Published;
        page.ShowLastUpdated = input.ShowLastUpdated;
        page.UpdatedAt = DateTime.UtcNow;
        return true;
    }

    // First URL segment of every routed action (/about, /blog, /admin, /sitemap.xml...).
    // Pages are served by the fallback route, so one with any of these slugs would
    // save fine and then never be reachable.
    private HashSet<string> ReservedSlugs()
    {
        var reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var action in _actions.ActionDescriptors.Items.OfType<ControllerActionDescriptor>())
        {
            var template = action.AttributeRouteInfo?.Template;
            var segment = template != null
                ? template.Trim('/').Split('/')[0]
                : action.ControllerName;
            if (segment.Length > 0 && !segment.StartsWith('{'))
                reserved.Add(segment);
        }
        return reserved;
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
