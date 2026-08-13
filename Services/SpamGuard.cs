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
}
