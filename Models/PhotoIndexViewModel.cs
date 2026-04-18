namespace Website.Models;

public class PhotoIndexViewModel
{
    public Dictionary<int, List<PhotoEntry>> PhotosByYear { get; set; } = new();
    public List<(string Tag, int Count)> TopTags { get; set; } = new();
    public List<int> Years { get; set; } = new();
    public int TotalCount { get; set; }
    public string? ActiveTag { get; set; }
}
