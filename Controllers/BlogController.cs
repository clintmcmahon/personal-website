using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Data;
using Website.Repositories;

namespace Website.Controllers;
public class BlogController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly BlogCommentDbContext _comments;

    public BlogController(IPostRepository postRepository, BlogCommentDbContext comments)
    {
        _postRepository = postRepository;
        _comments = comments;
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

    [Route("[controller]/{slug}")]
    public IActionResult Details(string slug)
    {
        var post = _postRepository.GetPostBySlug(slug);
        if (post == null)
        {
            return NotFound();
        }

        ViewData["BlogSlug"] = slug;
        ViewData["BlogComments"] = _comments.BlogComments
            .Where(c => c.Slug == slug)
            .OrderBy(c => c.CreatedAt)
            .ToList();

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
