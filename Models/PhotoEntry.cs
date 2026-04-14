namespace Website.Models;

public record PhotoImage(string Url, string Alt);

public class PhotoEntry
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    // Each inner list is one row. Single-image rows are landscape; multi-image rows render side by side.
    // Use "filename.jpg | Alt text here" in the markdown to set per-image alt text.
    public List<List<PhotoImage>> Rows { get; set; } = new();
}
