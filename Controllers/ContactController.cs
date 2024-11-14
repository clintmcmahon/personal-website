using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class ContactController : Controller
{

    public ContactController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }


}
