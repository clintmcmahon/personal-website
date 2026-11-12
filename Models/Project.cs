namespace Website.Models;

// One card on /projects, edited at /admin/projects.
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }

    // App-icon image shown on the card. Takes priority over IconClass.
    public string? ImageUrl { get; set; }

    // Bootstrap Icons class ("bi-globe2") for projects without an image.
    public string? IconClass { get; set; }

    // Comma-separated, rendered as the small tech chips.
    public string? Tech { get; set; }

    // Heading the card is grouped under ("Live", "On Hold"). Sections appear in the
    // order of their lowest SortOrder.
    public string Section { get; set; } = "Live";

    public int SortOrder { get; set; }

    public IEnumerable<string> TechList =>
        (Tech ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
