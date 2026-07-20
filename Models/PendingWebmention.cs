using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public class PendingWebmention
{
    [Key]
    public int Id { get; set; }

    // "Blog" or "Photo" — which table to re-read the live content from at send time.
    public string EntityType { get; set; } = "";

    // Post.Slug for blog entries, or the photo's yyyy-MM-dd date for photo entries.
    public string EntityKey { get; set; } = "";

    public string SourceUrl { get; set; } = "";
    public DateTime ScheduledFor { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool Sent { get; set; }
    public DateTime? SentAt { get; set; }
}
