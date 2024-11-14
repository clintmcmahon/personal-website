using Microsoft.AspNetCore.Mvc;
using Website.Repositories;

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
