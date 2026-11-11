using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;
public class PortfolioController : Controller
{

    public PortfolioController()
    {

    }

    // The full portfolio lives on parkasoftware.com. The two long-form case studies
    // below stay here until they have a home on that site.
    public IActionResult Index() => RedirectPermanent("https://parkasoftware.com/portfolio");

    [Route("[controller]/minnesota-secretary-of-state")]
    public IActionResult MinnesotaSecretaryOfState()
    {
        if (!CaseStudies.MinnesotaSecretaryOfState.Published) return NotFound();
        return View();
    }

    [Route("[controller]/srtr-interactive-reports")]
    public IActionResult SrtrInteractiveReports()
    {
        if (!CaseStudies.SrtrInteractiveReports.Published) return NotFound();
        return View();
    }

}
