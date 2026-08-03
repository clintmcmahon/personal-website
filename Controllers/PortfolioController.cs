using Microsoft.AspNetCore.Mvc;
using Website.Services;

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
