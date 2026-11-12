using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;

namespace Website.Controllers;

// Cards come from the Projects table, edited at /admin/projects.
public class ProjectsController : Controller
{
    private readonly BlogDbContext _db;

    public ProjectsController(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var sections = (await _db.Projects.AsNoTracking().ToListAsync())
            .GroupBy(p => p.Section)
            .OrderBy(g => g.Min(p => p.SortOrder))
            .Select(g => g.OrderBy(p => p.SortOrder).ThenBy(p => p.Name).ToList())
            .ToList();

        return View(sections);
    }
}
