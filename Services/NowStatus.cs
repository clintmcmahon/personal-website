namespace Website.Services;

// Single source of truth for the short "what I'm up to" line shown on both
// /now and the homepage terminal's `cat ~/now`. Update this whenever /now
// changes — keep it short (~40 chars) so it fits the terminal panel.
public static class NowStatus
{
    public const string Blurb = "deep in AI, coffee, and a new photoblog";

    // Whatever's in the hopper. Shown in the homepage terminal's `cat ~/brewing`.
    public const string CurrentlyBrewing = "Bombona Espresso by Coffee Collective";
}
