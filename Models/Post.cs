using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public DateTime Date { get; set; }
    public bool Draft { get; set; }
    public string Slug { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [NotMapped]
    public string TagsRaw
    {
        get => Tags != null ? string.Join(", ", Tags) : string.Empty;
        set => Tags = string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : value.Split(',').Select(t => t.Trim()).Where(t => !string.IsNullOrEmpty(t)).ToList();
    }
}
