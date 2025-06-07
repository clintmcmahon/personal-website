using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Website.Repositories;

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


}
