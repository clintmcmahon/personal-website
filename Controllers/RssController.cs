using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Repositories;

namespace Website.Controllers;
public class RssController : Controller
{

    private readonly IPostRepository _postRepository;

    public RssController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    [Route("rss")]
    [Route("feed")]
    public IActionResult Index()
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
