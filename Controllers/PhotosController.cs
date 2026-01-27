using Microsoft.AspNetCore.Mvc;
using Website.Services;
using Website.Repositories;

namespace Website.Controllers;
public class PhotosController : Controller
{
    private readonly PhotoService _photoService;
    private readonly PhotoRepository _photoRepository;

    public PhotosController(PhotoService photoService, PhotoRepository photoRepository)
    {
        _photoService = photoService;
        _photoRepository = photoRepository;
    }

    [Route("photos")]
    public IActionResult Index()
    {
        var albums = _photoRepository.GetAllAlbums();
        ViewData["Title"] = "Photos - Clint McMahon";
        ViewData["Description"] = "A collection of my photography organized by album.";
        return View(albums);
    }

    [Route("photos/{slug}")]
    public IActionResult Album(string slug)
    {
        var albums = _photoRepository.GetAllAlbums();
        var album = albums.FirstOrDefault(a => a.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        
        if (album == null)
        {
            return NotFound("Album not found.");
        }

        var photos = _photoRepository.GetPhotosForAlbum(slug);
        if (photos.Count == 0)
        {
            return NotFound("No photos found in this album.");
        }

        ViewData["Title"] = $"{album.Title} - Photos - Clint McMahon";
        ViewData["Description"] = album.Description;
        ViewBag.AlbumTitle = album.Title;
        ViewBag.AlbumDescription = album.Description;
        ViewBag.AlbumContent = album.Content;
        
        return View("Month", photos);
    }

    [Route("photos/latest")]
    public IActionResult LatestPhoto()
    {
        var viewModel = _photoService.GetLatestPhoto();
        if (viewModel.CurrentPhoto.Title == string.Empty)
        {
            return NotFound("No photos found.");
        }

        return View("PhotoDetail", viewModel);
    }

    [Route("photos/photo/{slug}")]
    public IActionResult PhotoBySlug(string slug)
    {
        var viewModel = _photoService.GetPhotoBySlug(slug);
        if (viewModel == null)
        {
            return NotFound("Photo not found.");
        }

        return View("PhotoDetail", viewModel);
    }
}
