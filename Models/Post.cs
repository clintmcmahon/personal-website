namespace Website.Models;
public class Post
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public bool Draft { get; set; }
    public string Slug { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
    public string Content { get; set; }
}
