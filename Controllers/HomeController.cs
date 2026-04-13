using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Repositories;

namespace Website.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostRepository _postRepository;

    public HomeController(ILogger<HomeController> logger, IPostRepository postRepository)
    {
        _logger = logger;
        _postRepository = postRepository;
    }

    public IActionResult Index()
    {
        var posts = _postRepository.GetAllPosts()
           .Where(post => !post.Draft)
           .OrderByDescending(post => post.Date)
           .Take(10);
        return View(posts);
    }

    [Route("/privacy")]
     public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
