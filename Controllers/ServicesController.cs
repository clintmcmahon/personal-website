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



     [Route("[controller]/umbraco-consulting")]
    public IActionResult UmbracoConsultant()
    {
        return View();
    }

    [Route("[controller]/health-data-platforms")]
    public IActionResult HealthDataPlatforms()
    {
        return View();
    }


    // Retired services. The pages are gone but the URLs were indexed, so they
    // send a 301 to the services index rather than dropping to a 404.
    [Route("[controller]/wordpress-hosting")]
    [Route("[controller]/shopify-ecommerce-development")]
    [Route("[controller]/website-care-plans")]
    public IActionResult RetiredService() => RedirectPermanent("/services");

    // Root-level route on purpose. This is the local-search landing page, and an
    // exact-match URL is worth more than tidy nesting under /services.
    [Route("freelance-developer-minneapolis")]
    public IActionResult FreelanceDeveloperMinneapolis()
    {
        return View();
    }

}
