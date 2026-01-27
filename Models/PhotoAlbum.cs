namespace Website.Models;

public class PhotoAlbum
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int PhotoCount { get; set; }
    public string ThumbnailUrl { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public string FolderPath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
