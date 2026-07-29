using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

// Tools live as self-contained static HTML files in wwwroot/tools (served directly by
// UseStaticFiles, same as Simon Willison's tools.simonwillison.net). This controller just
// builds the index from ToolCatalog, which reads a small metadata comment off the top of
// each file — no DB, no admin UI. Add a tool by dropping a new .html file in wwwroot/tools.
public class ToolsController : Controller
{
    private readonly IWebHostEnvironment _env;

    public ToolsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet("/tools")]
    public IActionResult Index()
    {
        return View(ToolCatalog.GetAll(_env));
    }
}
