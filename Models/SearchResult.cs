namespace Website.Models;

public class SearchResult
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
