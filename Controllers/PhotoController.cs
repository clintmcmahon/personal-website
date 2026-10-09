using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

public class PhotoController : Controller
{
    private readonly PhotoService _photoService;

    public PhotoController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet("/photo")]
    public async Task<IActionResult> Index()
    {
        return View(await _photoService.GetLatestAsync());
    }
}
