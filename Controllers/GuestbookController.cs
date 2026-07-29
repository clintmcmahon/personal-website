using Microsoft.AspNetCore.Mvc;
using Website.Data;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

[Route("guestbook")]
public class GuestbookController : Controller
{
    private readonly GuestbookDbContext _context;

    public GuestbookController(GuestbookDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Guestbook | Clint McMahon";
        ViewData["Description"] = "Sign the guestbook — say hi, leave a note, tell me what brought you here.";

        var entries = _context.GuestbookEntries
            .OrderByDescending(e => e.CreatedAt)
            .Take(200)
            .ToList();

        return View(entries);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromForm] string name,
        [FromForm] string message,
        [FromForm] string? hp_website,
        [FromForm] long loadedAt)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(message))
            return BadRequest("Name and message are required.");

        if (!SpamGuard.LooksLikeSpam(hp_website, loadedAt))
        {
            _context.GuestbookEntries.Add(new GuestbookEntry
            {
                Name = name.Trim(),
                Message = message.Trim(),
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
