using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Services;
using Website.Repositories;

namespace Website.Controllers;

[IgnoreAntiforgeryToken]
public class PhotosController : Controller
{
    private readonly PhotoService _photoService;
    private readonly PhotoRepository _photoRepository;

    public PhotosController(PhotoService photoService, PhotoRepository photoRepository)
    {
        _photoService = photoService;
        _photoRepository = photoRepository;
    }

    [HttpGet]
    [Route("photos")]
    public IActionResult Index()
    {
        var viewModel = _photoService.GetPhotosForIndex();
        return View("Index", viewModel);
    }

    [HttpGet]
    [Route("photos/tag/{tag}")]
    public IActionResult Tag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return NotFound();
        var viewModel = _photoService.GetPhotosByTag(tag);
        if (viewModel.TotalCount == 0)
            return NotFound($"No photos found for tag '{tag}'.");
        return View("TagIndex", viewModel);
    }

    [HttpGet]
    [Route("photos/about")]
    public IActionResult About()
    {
        ViewData["Title"] = "About — Clint McMahon Photos";
        return View("PhotoAbout");
    }

    [HttpGet]
    [Route("photos/sitemap.xml")]
    public IActionResult Sitemap()
    {
        var photos = _photoRepository.GetAllPhotos();
        var baseUrl = "https://photos.clintmcmahon.com";

        var allTags = photos
            .SelectMany(p => p.Tags)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.ToLowerInvariant().Trim())
            .Distinct();

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        var urls = new List<XElement>
        {
            new XElement(ns + "url",
                new XElement(ns + "loc", baseUrl + "/"),
                new XElement(ns + "changefreq", "weekly"),
                new XElement(ns + "priority", "1.0"))
        };

        foreach (var photo in photos)
        {
            urls.Add(new XElement(ns + "url",
                new XElement(ns + "loc", $"{baseUrl}/{photo.Date:yyyy-MM-dd}"),
                new XElement(ns + "lastmod", photo.Date.ToString("yyyy-MM-dd")),
                new XElement(ns + "changefreq", "yearly"),
                new XElement(ns + "priority", "0.7")));
        }

        foreach (var tag in allTags)
        {
            urls.Add(new XElement(ns + "url",
                new XElement(ns + "loc", $"{baseUrl}/tag/{Uri.EscapeDataString(tag)}"),
                new XElement(ns + "changefreq", "monthly"),
                new XElement(ns + "priority", "0.5")));
        }

        var sitemap = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(ns + "urlset", urls)
        );

        return Content(sitemap.ToString(), "application/xml", System.Text.Encoding.UTF8);
    }

    [HttpGet]
    [Route("photos/rss")]
    public IActionResult Rss()
    {
        var photos = _photoRepository.GetAllPhotos()
            .OrderByDescending(p => p.Date)
            .Take(30);

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var rss = new XDocument(
            new XElement("rss",
                new XAttribute("version", "2.0"),
                new XElement("channel",
                    new XElement("title", "Clint McMahon — Photos"),
                    new XElement("link", baseUrl),
                    new XElement("description", "Photographs from the life of Clint McMahon, a software developer in Minneapolis."),
                    new XElement("language", "en-us"),
                    photos.Select(photo =>
                    {
                        var absoluteImage = photo.ImageUrl.StartsWith("http")
                            ? photo.ImageUrl
                            : $"{baseUrl}{photo.ImageUrl}";
                        var title = string.IsNullOrWhiteSpace(photo.Title)
                            ? photo.Date.ToString("MMMM d, yyyy")
                            : photo.Title;
                        var description = $"<img src=\"{absoluteImage}\" alt=\"{title}\" />" +
                            (string.IsNullOrWhiteSpace(photo.Content) ? "" : $"<p>{photo.Content}</p>");

                        return new XElement("item",
                            new XElement("title", title),
                            new XElement("link", $"{baseUrl}/{photo.Date:yyyy-MM-dd}"),
                            new XElement("description", new XCData(description)),
                            new XElement("pubDate", photo.Date.ToString("R")),
                            new XElement("guid", $"{baseUrl}/{photo.Date:yyyy-MM-dd}")
                        );
                    })
                )
            )
        );

        return Content(rss.ToString(), "application/rss+xml", Encoding.UTF8);
    }

    [HttpGet]
    [Route("photos/{date:regex(^\\d{{4}}-\\d{{2}}-\\d{{2}}$)}")]
    public IActionResult PhotoByDate(string date)
    {
        if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return NotFound("Invalid date format.");
        }
        var viewModel = _photoService.GetPhotoByDate(parsedDate);
        if (viewModel == null)
        {
            return NotFound("Photo not found.");
        }
        return View("PhotoDetail", viewModel);
    }
}
