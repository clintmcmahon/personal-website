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
        var viewModel = _photoService.GetLatestPhoto();
        if (viewModel == null || string.IsNullOrEmpty(viewModel.CurrentPhoto.ImageUrl))
            return NotFound("No photos found.");
        return View("PhotoDetail", viewModel);
    }

    [HttpGet]
    [Route("photos/about")]
    public IActionResult About()
    {
        ViewData["Title"] = "About — Clint McMahon Photos";
        return View("PhotoAbout");
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
