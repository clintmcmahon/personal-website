using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class NowController : Controller
{

    public NowController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }


}
