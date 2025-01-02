using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;
public class ServicesController : Controller
{

    public ServicesController()
    {

    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("[controller]/custom-software-development")]
     public IActionResult CustomSoftware()
    {
        return View();
    }

    [Route("[controller]/azure-b2c-integration")]
     public IActionResult AzureB2CIntegration()
    {
        return View();
    }

    [Route("[controller]/cloud-implementation")]
     public IActionResult CloudImplementation()
    {
        return View();
    }

    [Route("[controller]/legacy-systems")]
     public IActionResult LegacySystems()
    {
        return View();
    }

    [Route("[controller]/rescue-recovery")]
     public IActionResult RescueRecovery()
    {
        return View();
    }

}
