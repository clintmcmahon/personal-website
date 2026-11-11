namespace Website.Services;

// Lightweight, dependency-free spam filter for public comment/guestbook forms:
// an invisible honeypot field plus a minimum time-since-page-load check. A public
// comment form shipped once with zero protection and had to be shut down, so new
// public forms get at least this baseline rather than none.
public static class SpamGuard
{
    public static bool LooksLikeSpam(string? honeypotValue, long loadedAtUnixSeconds, int minSeconds = 3)
    {
        if (!string.IsNullOrWhiteSpace(honeypotValue))
            return true;

        var elapsed = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - loadedAtUnixSeconds;
        return elapsed < minSeconds;
    }

    // Link drops are what guestbook spam exists for. Covers bare URLs, www hosts,
    // HTML anchors and BBCode, which is how bots try to get a link rendered.
    private static readonly System.Text.RegularExpressions.Regex LinkPattern = new(
        @"https?://|www\.|<\s*a\b|\[/?url\b|\[/?link\b",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

    public static bool ContainsLink(params string?[] values) =>
        values.Any(v => !string.IsNullOrEmpty(v) && LinkPattern.IsMatch(v));
}
