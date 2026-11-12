using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Data;
using Website.Models;
using Website.Repositories;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostRepository _postRepository;
    private readonly BlogDbContext _db;

    public HomeController(ILogger<HomeController> logger, IPostRepository postRepository, BlogDbContext db)
    {
        _logger = logger;
        _postRepository = postRepository;
        _db = db;
    }

    public IActionResult Index()
    {
        var posts = _postRepository.GetAllPosts()
           .Where(post => !post.Draft)
           .OrderByDescending(post => post.Date)
           .Take(10);

        // The homepage status line is the /now page's tagline, edited at /admin/pages.
        ViewData["NowBlurb"] = _db.Pages
            .Where(p => p.Slug == "now" && p.Published)
            .Select(p => p.Tagline)
            .FirstOrDefault();

        return View(posts);
    }

    // Target of UseStatusCodePagesWithReExecute in Program.cs. Only 404 gets a page;
    // every other code keeps its status with an empty body, as before.
    [Route("/error/{code:int}")]
    public IActionResult StatusCodePage(int code)
    {
        if (code != StatusCodes.Status404NotFound)
            return StatusCode(code);

        var feature = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IStatusCodeReExecuteFeature>();
        ViewData["MissingPath"] = feature?.OriginalPath ?? Request.Path.Value;

        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("NotFound");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
