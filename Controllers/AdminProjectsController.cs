using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers;

// Editor for the cards on /projects.
[Authorize]
public class AdminProjectsController : Controller
{
    private readonly BlogDbContext _db;

    public AdminProjectsController(BlogDbContext db)
    {
        _db = db;
    }

    [HttpGet("/admin/projects")]
    public async Task<IActionResult> Index()
    {
        var projects = await _db.Projects.AsNoTracking().ToListAsync();
        var ordered = projects
            .GroupBy(p => p.Section)
            .OrderBy(g => g.Min(p => p.SortOrder))
            .SelectMany(g => g.OrderBy(p => p.SortOrder).ThenBy(p => p.Name))
            .ToList();
        return View(ordered);
    }

    [HttpGet("/admin/projects/new")]
    public async Task<IActionResult> New()
    {
        // Lands at the end of the list unless reordered.
        var next = (await _db.Projects.MaxAsync(p => (int?)p.SortOrder) ?? 0) + 10;
        ViewData["Sections"] = await SectionsAsync();
        return View("Edit", new Project { SortOrder = next });
    }

    [HttpPost("/admin/projects/new")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(Project input)
    {
        var project = new Project();
        if (!Apply(project, input))
        {
            ViewData["Sections"] = await SectionsAsync();
            return View("Edit", input);
        }

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{project.Name}\" added.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("/admin/projects/{id:int}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (project == null) return NotFound();

        ViewData["Sections"] = await SectionsAsync();
        return View(project);
    }

    [HttpPost("/admin/projects/{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Project input)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null) return NotFound();

        if (!Apply(project, input))
        {
            input.Id = id;
            ViewData["Sections"] = await SectionsAsync();
            return View(input);
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{project.Name}\" updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/admin/projects/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project == null) return NotFound();

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"\"{project.Name}\" deleted.";
        return RedirectToAction(nameof(Index));
    }

    private bool Apply(Project project, Project input)
    {
        ModelState.Clear();

        input.Name = input.Name?.Trim() ?? string.Empty;
        input.Url = input.Url?.Trim() ?? string.Empty;
        input.Section = string.IsNullOrWhiteSpace(input.Section) ? "Live" : input.Section.Trim();

        if (input.Name.Length == 0)
            ModelState.AddModelError("Name", "Name is required.");
        if (input.Url.Length == 0)
            ModelState.AddModelError("Url", "URL is required.");

        if (!ModelState.IsValid) return false;

        project.Name = input.Name;
        project.Url = input.Url;
        project.Description = input.Description?.Trim();
        project.ImageUrl = string.IsNullOrWhiteSpace(input.ImageUrl) ? null : input.ImageUrl.Trim();
        project.IconClass = string.IsNullOrWhiteSpace(input.IconClass) ? null : input.IconClass.Trim();
        project.Tech = input.Tech?.Trim();
        project.Section = input.Section;
        project.SortOrder = input.SortOrder;
        return true;
    }

    private Task<List<string>> SectionsAsync() =>
        _db.Projects.Select(p => p.Section).Distinct().OrderBy(s => s).ToListAsync();
}
