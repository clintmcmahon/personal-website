using System.ComponentModel.DataAnnotations;

namespace Website.Models;

public record PhotoImage(string Url, string Alt);

public class PhotoEntry
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public List<List<PhotoImage>> Rows { get; set; } = new();
    public bool FullRows { get; set; }
    public bool Draft { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
