using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class PendingWebmention
{
    [Key]
    public int Id { get; set; }

    // Which table to re-read the live content from at send time. "Blog" is the only
    // type this app schedules; rows written before the photoblog split can still say
    // "Photo", and WebmentionDispatcherService drains those without sending.
    public string EntityType { get; set; } = "";

    // Post.Slug for blog entries.
    public string EntityKey { get; set; } = "";

    public string SourceUrl { get; set; } = "";
    public DateTime ScheduledFor { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Sent { get; set; }
    public DateTime? SentAt { get; set; }
}
