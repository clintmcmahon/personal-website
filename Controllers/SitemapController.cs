
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class SitemapController : Controller
{
    private readonly IPostRepository _postRepository;

    public SitemapController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    // robots.txt and llms.txt both advertise /sitemap.xml, which is also the URL
    // Search Console expects. Serve both spellings from the same action.
    [HttpGet("sitemap")]
    [HttpGet("sitemap.xml")]
    public IActionResult Sitemap()
    {
        // Build absolute URLs from the canonical base rather than from the request.
        // Url.Action with Request.Scheme produced "http://" behind the proxy and the
        // controller's declared casing ("/Services"), so every listed URL differed
        // from the page's own canonical tag. Route data still drives the path.
        string Abs(string? routePath) => CanonicalUrlHelper.ForPath(routePath?.ToLowerInvariant());

        var urls = new List<string>
    {
        Abs(Url.Action("Index", "Home")),
        Abs(Url.Action("FreelanceDeveloperMinneapolis", "Services")),
        Abs(Url.Action("Index", "Services")),
        Abs(Url.Action("RescueRecovery", "Services")),
        Abs(Url.Action("LegacySystems", "Services")),
        Abs(Url.Action("CustomSoftware", "Services")),
        Abs(Url.Action("AzureB2CIntegration", "Services")),
        Abs(Url.Action("CloudImplementation", "Services")),
        Abs(Url.Action("UmbracoConsultant", "Services")),
        Abs(Url.Action("HealthDataPlatforms", "Services")),
        Abs(Url.Action("WebsiteCarePlans", "Services")),
        Abs(Url.Action("Shopify", "Services")),
        Abs(Url.Action("WordpressHosting", "Services")),
        Abs(Url.Action("Index", "Portfolio")),
        Abs(Url.Action("Index", "About")),
        Abs(Url.Action("Index", "Blog")),
        Abs(Url.Action("Index", "Rss")),
        Abs(Url.Action("Index", "Contact"))
    };

        // Case studies only enter the sitemap once they are actually written.
        urls.AddRange(CaseStudies.All
            .Where(cs => cs.Published)
            .Select(cs => CanonicalUrlHelper.ForPath(cs.Url)));

        // Blog posts use the same helper the canonical tags use, so they cannot drift.
        var posts = _postRepository.GetAllPosts().Where(post => !post.Draft);
        urls.AddRange(posts.Select(post => CanonicalUrlHelper.BlogPost(post.Slug)));

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
