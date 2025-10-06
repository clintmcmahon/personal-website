using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class FeedsController : Controller
    {
        [HttpGet]
        [Route("feeds")]
        public IActionResult Index()
        {
            return View(); // Returns Views/Feeds/Index.cshtml
        }
    }
}
