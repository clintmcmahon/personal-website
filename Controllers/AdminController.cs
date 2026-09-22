using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

// Photo management moved to the photoblog app at photos.clintmcmahon.com/admin.
// What's left here is the admin landing page; blog posts live in AdminBlogController
// and backups in AdminBackupController.
[Authorize]
public class AdminController : Controller
{
    [HttpGet("/admin")]
    public IActionResult Index()
    {
        return View();
    }
}
