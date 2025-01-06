
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

    [HttpGet("sitemap")]
    public IActionResult Sitemap()
    {
        var urls = new List<string>
    {
        Url.Action("Index", "Home", null, Request.Scheme),
        Url.Action("Index", "Services", null, Request.Scheme),
        Url.Action("Index", "About", null, Request.Scheme)
        Url.Action("Index", "Blog", null, Request.Scheme),
        Url.Action("Index", "Rss", null, Request.Scheme),
        Url.Action("Index", "Contact", null, Request.Scheme)
    };

        // Add individual blog post URLs
        var posts = _postRepository.GetAllPosts().Where(post => !post.Draft);
        urls.AddRange(posts.Select(post => Url.Action("Details", "Blog", new { slug = post.Slug }, Request.Scheme)));

        // Define the XML namespace
        XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        var sitemap = new XDocument(
            new XElement(xmlns + "urlset",  // Apply the namespace here
                urls.Select(url =>
                    new XElement(xmlns + "url",  // Apply the namespace here
                        new XElement(xmlns + "loc", url),  // Apply the namespace here
                        new XElement(xmlns + "lastmod", DateTime.UtcNow.ToString("yyyy-MM-dd")), // Optional: update to post's last modified date
                        new XElement(xmlns + "changefreq", "weekly"), // Suggested frequency
                        new XElement(xmlns + "priority", "0.8")       // Priority of the page
                    )
                )
            )
        );

        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }

}
