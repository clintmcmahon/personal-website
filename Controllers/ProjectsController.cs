using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class ProjectsController : Controller
{

    public ProjectsController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }


}
