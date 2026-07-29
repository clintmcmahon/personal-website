using Microsoft.AspNetCore.Mvc;
using Website.Data;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

[Route("blog/comments")]
public class BlogCommentsController : Controller
{
    private readonly BlogCommentDbContext _context;

    public BlogCommentsController(BlogCommentDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromForm] string slug,
        [FromForm] string name,
        [FromForm] string content,
        [FromForm] string? hp_website,
        [FromForm] long loadedAt)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return BadRequest("Missing post reference.");

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(content))
            return BadRequest("Name and comment are required.");

        if (!SpamGuard.LooksLikeSpam(hp_website, loadedAt))
        {
            _context.BlogComments.Add(new BlogComment
            {
                Slug = slug,
                Name = name.Trim(),
                Content = content.Trim(),
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        // Redirect back to the post itself rather than trusting the Referer header.
        return RedirectToAction("Details", "Blog", new { slug });
    }
}
