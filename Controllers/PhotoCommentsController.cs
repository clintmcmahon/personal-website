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
        public IActionResult Post()
        {
            return StatusCode(403);
        }
    }
}
