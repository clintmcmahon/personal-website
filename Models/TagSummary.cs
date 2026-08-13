namespace Website.Models;

// One row of the tag index: the tag's URL slug, the spelling to show, and how many
// published posts carry it.
public class TagSummary
{
    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}
