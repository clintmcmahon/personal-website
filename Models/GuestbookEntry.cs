namespace Website.Models;

public class GuestbookEntry
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Nothing is public until it has been approved at /admin/guestbook. The
    // honeypot and timing checks alone did not keep bots out.
    public bool IsApproved { get; set; }
}
