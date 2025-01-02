using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class PortfolioController : Controller
{

    public PortfolioController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }


}
