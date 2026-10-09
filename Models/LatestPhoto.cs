namespace Website.Models;

// ImageCount > 1 means the post has more than one image; the page shows only the first
// and links to the permalink for the rest.
public record LatestPhoto(
    string Title,
    string Url,
    string ImageUrl,
    string? CaptionHtml,
    string? Location,
    DateTime Published,
    int ImageCount);
