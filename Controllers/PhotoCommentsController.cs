using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Website.Data;
using Website.Models;

namespace Website.Controllers
{
    [Route("photos/comments")]
    public class PhotoCommentsController : Controller
    {
        private readonly PhotoCommentDbContext _context;

        public PhotoCommentsController(PhotoCommentDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string photoDate, [FromForm] string name, [FromForm] string content)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Name and comment are required.");
            }
            var comment = new PhotoComment
            {
                PhotoDate = photoDate,
                Name = name,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            _context.PhotoComments.Add(comment);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
