using System.Text;

namespace Website.Services;

// Tags are authored as free text in the admin form ("ASP.NET Core", "coffee"), so the
// display spelling can't go straight into a URL. Slug() is the one place that maps a
// tag to its URL form, and lookups compare slugs rather than raw text — that way
// "ASP.NET Core" and "asp.net core" land on the same page instead of two.
public static class TagUrl
{
    public static string Slug(string? tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return string.Empty;

        var sb = new StringBuilder(tag.Length);
        foreach (var ch in tag.Trim().ToLowerInvariant())
        {
            if (char.IsAsciiLetterOrDigit(ch))
                sb.Append(ch);
            else if (sb.Length > 0 && sb[^1] != '-')
                sb.Append('-');
        }

        return sb.ToString().Trim('-');
    }

    public static string Path(string? tag) => $"/blog/tag/{Slug(tag)}";
}
