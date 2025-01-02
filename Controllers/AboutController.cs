using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class AboutController : Controller
{

    public AboutController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }


}
