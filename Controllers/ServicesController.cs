using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

/// <summary>
/// Client services are sold through Parka Software, not this site. The service pages
/// that used to live here were indexed, so each URL sends a 301 to the closest page
/// on parkasoftware.com rather than dropping to a 404. Where Parka has no direct
/// equivalent the services index is the destination.
///
/// The one page that stays is /freelance-developer-minneapolis. It ranks for a query
/// Parka's own pages do not target, and it hands readers on to Parka.
/// </summary>
public class ServicesController : Controller
{
    private const string Parka = "https://parkasoftware.com";

    [Route("[controller]")]
    public IActionResult Index() => RedirectPermanent($"{Parka}/services");

    [Route("[controller]/rescue-recovery")]
    public IActionResult RescueRecovery() => RedirectPermanent($"{Parka}/services/project-rescue");

    // Parka's migration page covers both .NET Framework moves and Umbraco version upgrades.
    [Route("[controller]/legacy-systems")]
    [Route("[controller]/umbraco-consulting")]
    public IActionResult LegacyMigration() => RedirectPermanent($"{Parka}/services/legacy-net-migration");

    // No one-to-one page on Parka, including the services retired from this site earlier.
    [Route("[controller]/custom-software-development")]
    [Route("[controller]/azure-b2c-integration")]
    [Route("[controller]/cloud-implementation")]
    [Route("[controller]/health-data-platforms")]
    [Route("[controller]/wordpress-hosting")]
    [Route("[controller]/shopify-ecommerce-development")]
    [Route("[controller]/website-care-plans")]
    public IActionResult ServicesIndex() => RedirectPermanent($"{Parka}/services");

    // Root-level route on purpose. This is the local-search landing page, and an
    // exact-match URL is worth more than tidy nesting under /services.
    [Route("freelance-developer-minneapolis")]
    public IActionResult FreelanceDeveloperMinneapolis()
    {
        return View();
    }
}
