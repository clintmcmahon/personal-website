using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

public class AdminController : Controller
{
    private readonly PhotoPostService _photoPostService;

    public AdminController(PhotoPostService photoPostService)
    {
        _photoPostService = photoPostService;
    }

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
            var alt = i < altTexts.Count ? altTexts[i] : string.Empty;
            if (string.IsNullOrWhiteSpace(alt))
            {
                ModelState.AddModelError("altTexts", $"Alt text is required for image {i + 1}.");
                return View();
            }
        }

        var date = DateTime.Today;
        if (!string.IsNullOrWhiteSpace(postDate) &&
            DateTime.TryParse(postDate, out var parsed))
        {
            date = parsed;
        }

        var request = new PhotoPostRequest
        {
            Date = date,
            Title = title.Trim(),
            Tags = tags,
            Body = body,
            LayoutRows = layoutRows,
            Draft = draft,
            Images = images.Select((_, i) => new PhotoPostImage
            {
                FileName = images[i].FileName,
                AltText = i < altTexts.Count ? altTexts[i] : string.Empty
            }).ToList()
        };

        var uploads = images.Select(f => (f.FileName, f.OpenReadStream()));

        try
        {
            await _photoPostService.PublishAsync(request, uploads);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Failed to publish: {ex.Message}");
            return View();
        }
        finally
        {
            foreach (var f in images)
                f.OpenReadStream().Dispose();
        }

        TempData["Success"] = $"Photo post \"{title}\" committed to GitHub. It will be live in about a minute.";
        return RedirectToAction(nameof(NewPhoto));
    }
}
