
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Repositories;

namespace Website.Controllers;

public class SitemapController : Controller
{
    private readonly IPostRepository _postRepository;

    public SitemapController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpGet]
    public IActionResult Sitemap()
    {
        var urls = new List<string>
        {
            Url.Action("Index", "Home", null, Request.Scheme), // Homepage
            Url.Action("Index", "Blog", null, Request.Scheme)  // Blog index page
        };

        // Add individual blog post URLs
        var posts = _postRepository.GetAllPosts().Where(post => !post.Draft);
        urls.AddRange(posts.Select(post => Url.Action("Details", "Blog", new { slug = post.Slug }, Request.Scheme)));

        var sitemap = new XDocument(
            new XElement("urlset",
                new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                urls.Select(url =>
                    new XElement("url",
                        new XElement("loc", url),
                        new XElement("lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")), // Optional: update to post's last modified date
                        new XElement("changefreq", "weekly"), // Suggested frequency
                        new XElement("priority", "0.8")       // Priority of the page
                    )
                )
            )
        );

        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }
}
