using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Website.Data;
using Website.Models;
using Website.Services;

namespace Website.Controllers;

[Route("guestbook")]
public class GuestbookController : Controller
{
    private const int MaxNameLength = 60;
    private const int MaxMessageLength = 1000;

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

        return View(ApprovedEntries());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [EnableRateLimiting("guestbook")]
    public async Task<IActionResult> Post(
        [FromForm] string name,
        [FromForm] string message,
        [FromForm] string? hp_website,
        [FromForm] long loadedAt)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(message))
            return BadRequest("Name and message are required.");

        name = name.Trim();
        message = message.Trim();

        // These two are told to the visitor, since a real person can fix them.
        string? error = null;
        if (name.Length > MaxNameLength || message.Length > MaxMessageLength)
            error = $"Keep your name under {MaxNameLength} characters and your message under {MaxMessageLength}.";
        else if (SpamGuard.ContainsLink(name, message))
            error = "Links aren't allowed in the guestbook. Take the link out and try again.";

        if (error != null)
        {
            ViewData["Title"] = "Guestbook | Clint McMahon";
            ViewData["GuestbookError"] = error;
            ViewData["GuestbookName"] = name;
            ViewData["GuestbookMessage"] = message;
            return View(nameof(Index), ApprovedEntries());
        }

        // Honeypot and timing failures are dropped without a word, so a bot gets
        // the same response as a real signature and learns nothing.
        if (!SpamGuard.LooksLikeSpam(hp_website, loadedAt))
        {
            _context.GuestbookEntries.Add(new GuestbookEntry
            {
                Name = name,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            });
            await _context.SaveChangesAsync();
        }

        TempData["GuestbookThanks"] = true;
        return RedirectToAction(nameof(Index));
    }

    private List<GuestbookEntry> ApprovedEntries() =>
        _context.GuestbookEntries
            .Where(e => e.IsApproved)
            .OrderByDescending(e => e.CreatedAt)
            .Take(200)
            .ToList();
}
