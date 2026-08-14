namespace Website.Services;

public static class CanonicalUrlHelper
{
    public const string BlogBaseUrl = "https://clintmcmahon.com";

    // Only PhotoRedirectController still needs this: the photoblog is its own app now,
    // and nothing here builds photo URLs beyond forwarding the old /photos/* paths.
    public const string PhotoBaseUrl = "https://photos.clintmcmahon.com";

    public static string BlogPost(string slug) => $"{BlogBaseUrl}/blog/{slug}";

    public static string BlogTag(string tagSlug) => $"{BlogBaseUrl}/blog/tag/{tagSlug}";

    /// <summary>
    /// Builds the canonical URL for a request path. Used as the layout fallback so pages
    /// that do not set ViewData["CanonicalUrl"] self-canonicalize instead of pointing at
    /// the homepage. Query strings are dropped and trailing slashes normalized away.
    /// </summary>
    public static string ForPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == "/")
            return $"{BlogBaseUrl}/";

        var trimmed = path.TrimEnd('/');
        if (trimmed.Length == 0)
            return $"{BlogBaseUrl}/";

        if (!trimmed.StartsWith('/'))
            trimmed = "/" + trimmed;

        return $"{BlogBaseUrl}{trimmed}";
    }
}
