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

    public IActionResult Index()
    {
        var posts = _postRepository.GetAllPosts()
        .Where(post => !post.Draft)
        .OrderByDescending(post => post.Date);
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


}
