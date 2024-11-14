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

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Rss()
    {
        var posts = _postRepository
            .GetAllPosts()
            .Where(post => !post.Draft)
            .OrderByDescending(post => post.Date)
            .Take(10); // Limit to the latest 10 posts

        var rss = new XDocument(
            new XElement("rss",
                new XAttribute("version", "2.0"),
                new XElement("channel",
                    new XElement("title", "Your Blog Title"),
                    new XElement("link", Url.Action("Index", "Blog", null, Request.Scheme)),
                    new XElement("description", "Your Blog Description"),
                    new XElement("language", "en-us"),
                    posts.Select(post =>
                        new XElement("item",
                            new XElement("title", post.Title),
                            new XElement("link", Url.Action("Details", "Blog", new { slug = post.Slug }, Request.Scheme)),
                            new XElement("description", post.Description),
                            new XElement("pubDate", post.Date.ToString("R")), // RFC1123 format for RSS
                            new XElement("guid", Url.Action("Details", "Blog", new { slug = post.Slug }, Request.Scheme))
                        )
                    )
                )
            )
        );

        // Return the XML content as an RSS feed
        return Content(rss.ToString(), "application/rss+xml", Encoding.UTF8);
    }

}
