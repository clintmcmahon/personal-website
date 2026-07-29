using Microsoft.AspNetCore.Mvc;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class OgImageController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly OgImageService _ogImageService;

    public OgImageController(IPostRepository postRepository, OgImageService ogImageService)
    {
        _postRepository = postRepository;
        _ogImageService = ogImageService;
    }

    [HttpGet("/blog/{slug}/og.png")]
    public IActionResult BlogCard(string slug)
    {
        var post = _postRepository.GetPostBySlug(slug);
        if (post == null) return NotFound();

        var png = _ogImageService.RenderBlogCard(post.Title, post.Date.ToString("MMMM d, yyyy"));

        Response.Headers["Cache-Control"] = "public, max-age=86400";
        return File(png, "image/png");
    }
}
