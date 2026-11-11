using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;

namespace Website.Controllers;

// Moderation queue for the public guestbook. Entries are saved unapproved and
// only show on /guestbook once approved here.
[Authorize]
public class AdminGuestbookController : Controller
{
    private readonly GuestbookDbContext _context;

    public AdminGuestbookController(GuestbookDbContext context)
    {
        _context = context;
    }

    [HttpGet("/admin/guestbook")]
    public async Task<IActionResult> Index()
    {
        var entries = await _context.GuestbookEntries
            .OrderBy(e => e.IsApproved)
            .ThenByDescending(e => e.CreatedAt)
            .Take(500)
            .ToListAsync();

        ViewData["PendingTotal"] = await _context.GuestbookEntries.CountAsync(e => !e.IsApproved);
        return View(entries);
    }

    [HttpPost("/admin/guestbook/{id:int}/approve")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var entry = await _context.GuestbookEntries.FindAsync(id);
        if (entry != null)
        {
            entry.IsApproved = true;
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Approved the entry from {entry.Name}.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("/admin/guestbook/{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entry = await _context.GuestbookEntries.FindAsync(id);
        if (entry != null)
        {
            _context.GuestbookEntries.Remove(entry);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Entry deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    // Clears the whole queue in one go. Approved entries are never touched.
    [HttpPost("/admin/guestbook/delete-pending")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePending()
    {
        var deleted = await _context.GuestbookEntries.Where(e => !e.IsApproved).ExecuteDeleteAsync();
        TempData["Success"] = $"Deleted {deleted} pending {(deleted == 1 ? "entry" : "entries")}.";
        return RedirectToAction(nameof(Index));
    }
}
