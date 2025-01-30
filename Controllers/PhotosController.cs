using Microsoft.AspNetCore.Mvc;
using Website.Services;
namespace Website.Controllers;
public class PhotosController : Controller
{
    private readonly PhotoService _photoService;

    public PhotosController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [Route("photos")]
    public IActionResult LatestPhoto()
    {
        var viewModel = _photoService.GetLatestPhoto();
        if (viewModel.CurrentPhoto.Title == string.Empty)
        {
            return NotFound("No photos found.");
        }

        return View("PhotoDetail", viewModel);
    }
}
