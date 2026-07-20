using Microsoft.AspNetCore.Mvc;
using Website.Data;
using Website.Models;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class AdminController : Controller
{
    private readonly PhotoDbContext _db;
    private readonly ImageProcessingService _imageProcessor;
    private readonly IWebHostEnvironment _env;
    private readonly PhotoRepository _fileRepo;
    private readonly MastodonService _mastodon;
    private readonly WebmentionService _webmention;

    public AdminController(
        PhotoDbContext db,
        ImageProcessingService imageProcessor,
        IWebHostEnvironment env,
        PhotoRepository fileRepo,
        MastodonService mastodon,
        WebmentionService webmention)
    {
        _db = db;
        _imageProcessor = imageProcessor;
        _env = env;
        _fileRepo = fileRepo;
        _mastodon = mastodon;
        _webmention = webmention;
    }

    // ── Admin landing ────────────────────────────────────────────────────────

    [HttpGet("/admin")]
    public IActionResult Index()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin");

        return View();
    }

    // ── Photo list ───────────────────────────────────────────────────────────

    [HttpGet("/admin/photos")]
    public IActionResult Photos()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/photos");

        var photos = _db.Photos
            .OrderByDescending(p => p.Date)
            .ToList();

        return View(photos);
    }

    // ── Publish toggle ───────────────────────────────────────────────────────

    [HttpPost("/admin/photos/{id:int}/publish")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PublishPhoto(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var photo = _db.Photos.Find(id);
        if (photo == null) return NotFound();

        var wasDraft = photo.Draft;
        photo.Draft = !photo.Draft;
        photo.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        TempData["Success"] = photo.Draft
            ? $"\"{photo.Title}\" unpublished."
            : $"\"{photo.Title}\" published.";

        if (wasDraft && !photo.Draft)
        {
            photo.SyndicationUrl = await _mastodon.PostPhotoAsync(photo);
            await _db.SaveChangesAsync();

            await _webmention.ScheduleAsync("Photo", photo.Date.ToString("yyyy-MM-dd"), CanonicalUrlHelper.Photo(photo.Date));
        }

        return RedirectToAction(nameof(Photos));
    }

    // ── Delete ───────────────────────────────────────────────────────────────

    [HttpPost("/admin/photos/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var photo = _db.Photos.Find(id);
        if (photo == null) return NotFound();

        _db.Photos.Remove(photo);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{photo.Title}\" deleted.";
        return RedirectToAction(nameof(Photos));
    }

    // ── Edit photo post ──────────────────────────────────────────────────────

    [HttpGet("/admin/photos/{id:int}/edit")]
    public IActionResult EditPhoto(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect($"/auth/login?returnUrl=/admin/photos/{id}/edit");

        var photo = _db.Photos.Find(id);
        if (photo == null) return NotFound();

        return View(photo);
    }

    [HttpPost("/admin/photos/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPhoto(
        int id,
        string title,
        string? slug,
        string? postDate,
        string? tags,
        string? content,
        bool draft,
        bool layoutRows,
        List<string> altTexts)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var photo = _db.Photos.Find(id);
        if (photo == null) return NotFound();

        if (string.IsNullOrWhiteSpace(title))
        {
            TempData["Error"] = "Title is required.";
            return View(photo);
        }

        photo.Title = title.Trim();
        photo.Slug = string.IsNullOrWhiteSpace(slug) ? photo.Slug : slug.Trim();
        photo.Draft = draft;
        photo.FullRows = layoutRows;
        photo.Content = content ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(postDate) && DateTime.TryParse(postDate, out var date))
            photo.Date = date;

        photo.Tags = string.IsNullOrWhiteSpace(tags)
            ? new List<string>()
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                  .Select(t => t.ToLowerInvariant())
                  .ToList();

        // Rebuild rows with updated alt texts, preserving URLs and row structure
        var altIndex = 0;
        var updatedRows = photo.Rows
            .Select(row => row
                .Select(img => new PhotoImage(img.Url, altIndex < altTexts.Count ? altTexts[altIndex++] : img.Alt))
                .ToList())
            .ToList();
        photo.Rows = updatedRows;
        photo.ImageUrl = updatedRows.FirstOrDefault()?.FirstOrDefault()?.Url ?? photo.ImageUrl;

        // EF won't detect mutations on collection value-converters without a comparer — force it
        _db.Entry(photo).Property(p => p.Rows).IsModified = true;
        _db.Entry(photo).Property(p => p.Tags).IsModified = true;

        photo.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{photo.Title}\" updated.";
        return RedirectToAction(nameof(EditPhoto), new { id });
    }

    // ── New photo post ───────────────────────────────────────────────────────

    [HttpGet("/admin/photos/new")]
    public IActionResult NewPhoto()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        return View();
    }

    [HttpPost("/admin/photos/new")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    public async Task<IActionResult> NewPhoto(
        string title,
        string? postDate,
        string? tags,
        string? body,
        bool layoutRows,
        bool draft,
        List<IFormFile> images,
        List<string> altTexts)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        if (string.IsNullOrWhiteSpace(title))
        {
            ModelState.AddModelError("title", "Title is required.");
            return View();
        }

        if (images == null || images.Count == 0)
        {
            ModelState.AddModelError("images", "At least one image is required.");
            return View();
        }

        for (int i = 0; i < images.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(i < altTexts.Count ? altTexts[i] : null))
            {
                ModelState.AddModelError("altTexts", $"Alt text is required for image {i + 1}.");
                return View();
            }
        }

        var date = DateTime.Today;
        if (!string.IsNullOrWhiteSpace(postDate) && DateTime.TryParse(postDate, out var parsed))
            date = parsed;

        var folderName = date.ToString("yyyy-MM-dd");
        var year = date.Year.ToString();
        var folder = Path.Combine(_env.WebRootPath, "photos", year, folderName);
        Directory.CreateDirectory(folder);

        var rows = new List<List<PhotoImage>>();

        for (int i = 0; i < images.Count; i++)
        {
            var img = images[i];
            var alt = i < altTexts.Count ? altTexts[i] : string.Empty;

            var baseName = SanitizeFileName(Path.GetFileNameWithoutExtension(img.FileName));
            var fileName = baseName + ".jpeg";
            var fullPath = Path.Combine(folder, fileName);

            var counter = 1;
            while (System.IO.File.Exists(fullPath))
                fullPath = Path.Combine(folder, $"{baseName}-{counter++}.jpeg");

            fileName = Path.GetFileName(fullPath);

            using var stream = img.OpenReadStream();
            var resized = await _imageProcessor.ResizeAsync(stream);
            await System.IO.File.WriteAllBytesAsync(fullPath, resized);

            rows.Add(new List<PhotoImage> { new PhotoImage($"/photos/{year}/{folderName}/{fileName}", alt) });
        }

        var slug = date.ToString("yyyy-MM-dd");
        var baseSlug = slug;
        var slugCounter = 1;
        while (_db.Photos.Any(p => p.Slug == slug))
            slug = $"{baseSlug}-{slugCounter++}";

        var tagList = string.IsNullOrWhiteSpace(tags)
            ? new List<string>()
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                  .Select(t => t.ToLowerInvariant())
                  .ToList();

        var entry = new PhotoEntry
        {
            Title = title.Trim(),
            Slug = slug,
            Date = date,
            ImageUrl = rows.FirstOrDefault()?.FirstOrDefault()?.Url ?? string.Empty,
            Content = string.IsNullOrWhiteSpace(body) ? string.Empty : $"<p>{System.Web.HttpUtility.HtmlEncode(body.Trim())}</p>",
            Tags = tagList,
            Rows = rows,
            FullRows = layoutRows,
            Draft = draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.Photos.Add(entry);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Photo post \"{title}\" saved.";

        if (!draft)
        {
            entry.SyndicationUrl = await _mastodon.PostPhotoAsync(entry);
            await _db.SaveChangesAsync();

            await _webmention.ScheduleAsync("Photo", entry.Date.ToString("yyyy-MM-dd"), CanonicalUrlHelper.Photo(entry.Date));
        }

        return RedirectToAction(nameof(NewPhoto));
    }

    // ── Migrate filesystem photos to database (run once) ─────────────────────

    [HttpGet("/admin/photos/migrate")]
    public IActionResult MigrateConfirm()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        ViewData["FileCount"] = _fileRepo.GetAllPhotos().Count;
        ViewData["DbCount"] = _db.Photos.Count();
        return View();
    }

    [HttpPost("/admin/photos/migrate")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MigrateRun()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login");

        var existing = _db.Photos.Select(p => p.Slug).ToHashSet();
        var photos = _fileRepo.GetAllPhotos();
        int imported = 0, skipped = 0;
        var errors = new List<string>();

        foreach (var photo in photos)
        {
            if (existing.Contains(photo.Slug))
            {
                skipped++;
                continue;
            }

            existing.Add(photo.Slug);

            try
            {
                photo.CreatedAt = photo.Date;
                photo.UpdatedAt = photo.Date;
                _db.Photos.Add(photo);
                await _db.SaveChangesAsync();
                imported++;
            }
            catch (Exception ex)
            {
                _db.ChangeTracker.Clear();
                errors.Add($"{photo.Slug}: {ex.Message}");
            }
        }

        TempData["Success"] = $"Imported {imported} photos. Skipped {skipped} already in database.";
        if (errors.Any())
            TempData["Errors"] = string.Join("\n", errors);

        return RedirectToAction(nameof(Index));
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string SanitizeFileName(string name)
    {
        var safe = name.ToLowerInvariant();
        safe = System.Text.RegularExpressions.Regex.Replace(safe, @"[^a-z0-9\-_]", "-");
        safe = System.Text.RegularExpressions.Regex.Replace(safe, @"-{2,}", "-");
        return safe.Trim('-');
    }
}
