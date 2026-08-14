using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;
public class BlogController : Controller
{
    private readonly IPostRepository _postRepository;

    public BlogController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public IActionResult Index(int page = 1, int pageSize = 20)
    {
        var posts = _postRepository.GetAllPosts()
            .Where(post => !post.Draft)
            .OrderByDescending(post => post.Date)
            .ToList();

        var totalPosts = posts.Count;
        var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);
        var pagedPosts = posts.Skip((page - 1) * pageSize).Take(pageSize);

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;
        return View(pagedPosts);
    }

    // Both of these sit under /blog alongside the "[controller]/{slug}" route below.
    // A literal segment outranks a parameter in ASP.NET Core's route table, so
    // "/blog/tags" reaches Tags() rather than Details("tags").
    [HttpGet("/blog/tags")]
    public IActionResult Tags()
    {
        ViewData["Title"] = "Tags | Clint McMahon";
        return View(_postRepository.GetAllTags());
    }

    [HttpGet("/blog/tag/{tagSlug}")]
    public IActionResult Tag(string tagSlug)
    {
        var posts = _postRepository.GetPostsByTagSlug(tagSlug)
            .Where(post => !post.Draft)
            .OrderByDescending(post => post.Date)
            .ToList();

        if (posts.Count == 0)
            return NotFound();

        // The tag index already resolves each slug to its display spelling. Reuse that
        // rather than title-casing the slug, so "asp-net-core" renders as "ASP.NET Core".
        var name = _postRepository.GetAllTags()
            .FirstOrDefault(t => t.Slug == TagUrl.Slug(tagSlug))?.Name ?? tagSlug;

        ViewData["Title"] = $"Posts tagged {name} | Clint McMahon";
        ViewData["TagName"] = name;
        ViewData["CanonicalUrl"] = CanonicalUrlHelper.BlogTag(TagUrl.Slug(tagSlug));
        return View(posts);
    }

    [Route("[controller]/{slug}")]
    public IActionResult Details(string slug)
    {
        var post = _postRepository.GetPostBySlug(slug);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpGet("/admin/blog/{id:int}/preview")]
    public IActionResult Preview(int id)
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect($"/auth/login?returnUrl=/admin/blog/{id}/preview");

        var post = _postRepository.GetPostByIdIncludingDrafts(id);
        if (post == null) return NotFound();

        ViewData["IsPreview"] = true;
        return View("Details", post);
    }

}
