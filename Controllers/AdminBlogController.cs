using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class AdminBlogController : Controller
{
    private readonly BlogDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly MastodonService _mastodon;
    private readonly WebmentionService _webmention;

    public AdminBlogController(BlogDbContext db, IWebHostEnvironment env, MastodonService mastodon, WebmentionService webmention)
    {
        _db = db;
        _env = env;
        _mastodon = mastodon;
        _webmention = webmention;
    }

    // ── List ────────────────────────────────────────────────────────────────

    [HttpGet("/admin/blog")]
    public IActionResult Index()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/blog");

        var posts = _db.Posts
            .AsNoTracking()
            .OrderByDescending(p => p.Date)
            .ToList();

        return View(posts);
    }

    // ── New ─────────────────────────────────────────────────────────────────

    [HttpGet("/admin/blog/new")]
    public IActionResult New()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/blog/new");

        return View(new Post { Date = DateTime.Today, Draft = true });
    }

    [HttpPost("/admin/blog/new")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(Post post)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        if (string.IsNullOrWhiteSpace(post.Title))
            ModelState.AddModelError("Title", "Title is required.");

        if (string.IsNullOrWhiteSpace(post.Slug))
            post.Slug = Slugify(post.Title);

        if (_db.Posts.Any(p => p.Slug == post.Slug))
            ModelState.AddModelError("Slug", "A post with this slug already exists.");

        if (!ModelState.IsValid)
            return View(post);

        post.TagsRaw = Request.Form["TagsRaw"];
        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{post.Title}\" saved.";
        return RedirectToAction(nameof(Edit), new { id = post.Id });
    }

    // ── Edit ────────────────────────────────────────────────────────────────

    [HttpGet("/admin/blog/{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect($"/auth/login?returnUrl=/admin/blog/{id}/edit");

        var post = _db.Posts.Find(id);
        if (post == null) return NotFound();

        return View(post);
    }

    [HttpPost("/admin/blog/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Post updated)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var post = _db.Posts.Find(id);
        if (post == null) return NotFound();

        if (string.IsNullOrWhiteSpace(updated.Title))
            ModelState.AddModelError("Title", "Title is required.");

        if (!ModelState.IsValid)
        {
            updated.Id = id;
            return View(updated);
        }

        post.Title = updated.Title;
        post.Slug = string.IsNullOrWhiteSpace(updated.Slug) ? Slugify(updated.Title) : updated.Slug;
        post.Description = updated.Description;
        post.Keywords = updated.Keywords;
        post.Content = updated.Content;
        post.Date = updated.Date;
        post.Draft = updated.Draft;
        post.TagsRaw = Request.Form["TagsRaw"];
        post.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{post.Title}\" updated.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    // ── Publish toggle ───────────────────────────────────────────────────────

    [HttpPost("/admin/blog/{id:int}/publish")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var post = _db.Posts.Find(id);
        if (post == null) return NotFound();

        var wasDraft = post.Draft;
        post.Draft = !post.Draft;
        post.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        TempData["Success"] = post.Draft ? $"\"{post.Title}\" unpublished." : $"\"{post.Title}\" published.";

        if (wasDraft && !post.Draft)
        {
            post.SyndicationUrl = await _mastodon.PostBlogPostAsync(post);
            await _db.SaveChangesAsync();

            await _webmention.ScheduleAsync("Blog", post.Slug, CanonicalUrlHelper.BlogPost(post.Slug));
        }

        return RedirectToAction(nameof(Index));
    }

    // ── Delete ───────────────────────────────────────────────────────────────

    [HttpPost("/admin/blog/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var post = _db.Posts.Find(id);
        if (post == null) return NotFound();

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{post.Title}\" deleted.";
        return RedirectToAction(nameof(Index));
    }

    // ── Image upload ─────────────────────────────────────────────────────────

    [HttpPost("/admin/blog/upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile image)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Unauthorized();

        if (image == null || image.Length == 0)
            return BadRequest(new { error = new { message = "No file received." } });

        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!allowed.Contains(ext))
            return BadRequest(new { error = new { message = "File type not allowed." } });

        var year = DateTime.Today.Year.ToString();
        var folder = Path.Combine(_env.WebRootPath, "images", year);
        Directory.CreateDirectory(folder);

        var safeName = Path.GetFileNameWithoutExtension(image.FileName)
            .ToLowerInvariant()
            .Replace(" ", "-");
        safeName = System.Text.RegularExpressions.Regex.Replace(safeName, @"[^a-z0-9\-]", "");
        var fileName = $"{safeName}{ext}";

        // avoid overwriting existing files
        var counter = 1;
        while (System.IO.File.Exists(Path.Combine(folder, fileName)))
            fileName = $"{safeName}-{counter++}{ext}";

        var fullPath = Path.Combine(folder, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
            await image.CopyToAsync(stream);

        return Ok(new { data = new { filePath = $"/images/{year}/{fileName}" } });
    }

    // ── Migrate from markdown files (run once) ───────────────────────────────

    [HttpGet("/admin/blog/migrate")]
    public IActionResult MigrateConfirm()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var fileCount = Directory
            .EnumerateFiles(Path.Combine(_env.ContentRootPath, "wwwroot", "posts"), "*.md", SearchOption.AllDirectories)
            .Count();

        ViewData["FileCount"] = fileCount;
        ViewData["DbCount"] = _db.Posts.Count();
        return View();
    }

    [HttpPost("/admin/blog/migrate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MigrateRun()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var fileRepo = new PostRepository();
        var existing = _db.Posts.Select(p => p.Slug).ToHashSet();

        var posts = fileRepo.GetAllPostsRaw();
        int imported = 0;
        int skipped = 0;
        var errors = new List<string>();

        foreach (var post in posts)
        {
            if (existing.Contains(post.Slug))
            {
                skipped++;
                continue;
            }

            // track within this batch so duplicate slugs in the files don't collide
            existing.Add(post.Slug);

            try
            {
                post.CreatedAt = post.Date;
                post.UpdatedAt = post.Date;
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
                imported++;
            }
            catch (Exception ex)
            {
                // detach the failed entity so EF doesn't keep retrying it
                _db.ChangeTracker.Clear();
                errors.Add($"{post.Slug}: {ex.Message}");
            }
        }

        TempData["Success"] = $"Imported {imported} posts. Skipped {skipped} already in database.";
        if (errors.Any())
            TempData["Errors"] = string.Join("\n", errors);

        return RedirectToAction(nameof(Index));
    }

    private static string Slugify(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return string.Empty;
        return System.Text.RegularExpressions.Regex
            .Replace(title.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-")
            .Trim('-');
    }
}
