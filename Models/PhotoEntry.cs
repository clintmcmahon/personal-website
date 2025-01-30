namespace Website.Models;

public class PhotoEntry
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}
