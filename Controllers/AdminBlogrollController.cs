using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Services;

namespace Website.Controllers;

// Replaces the blogroll with the Blogs and Photoblogs folders from a NetNewsWire
// OPML export (File > Export Subscriptions).
[Authorize]
public class AdminBlogrollController : Controller
{
    private readonly BlogDbContext _db;

    public AdminBlogrollController(BlogDbContext db)
    {
        _db = db;
    }

    [HttpGet("/admin/blogroll")]
    public async Task<IActionResult> Index()
    {
        await LoadSummaryAsync();
        return View();
    }

    [HttpPost("/admin/blogroll")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(2_000_000)]
    public async Task<IActionResult> Upload(IFormFile? opml)
    {
        if (opml == null || opml.Length == 0)
            return await Fail("Choose an OPML file to upload.");

        List<Models.BlogrollFeed> feeds;
        try
        {
            await using var stream = opml.OpenReadStream();
            feeds = BlogrollOpml.Parse(stream);
        }
        catch (Exception ex) when (ex is XmlException or FormatException)
        {
            return await Fail($"Couldn't read that file: {ex.Message}");
        }

        // An export without the expected folders would otherwise wipe the blogroll.
        if (feeds.Count == 0)
            return await Fail($"No feeds found in a {string.Join(" or ", BlogrollOpml.Categories)} folder. Nothing was changed.");

        await using var tx = await _db.Database.BeginTransactionAsync();
        await _db.BlogrollFeeds.ExecuteDeleteAsync();
        _db.BlogrollFeeds.AddRange(feeds);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        var counts = string.Join(", ", feeds.GroupBy(f => f.Category).Select(g => $"{g.Count()} {g.Key}"));
        TempData["Success"] = $"Blogroll replaced: {counts}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> Fail(string message)
    {
        ViewData["Error"] = message;
        await LoadSummaryAsync();
        return View(nameof(Index));
    }

    private async Task LoadSummaryAsync()
    {
        ViewData["Counts"] = await _db.BlogrollFeeds
            .GroupBy(f => f.Category)
            .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
            .ToListAsync();
        ViewData["ImportedAt"] = await _db.BlogrollFeeds.MaxAsync(f => (DateTime?)f.ImportedAt);
    }
}
