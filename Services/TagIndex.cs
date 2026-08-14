using Website.Models;

namespace Website.Services;

// Turns the raw per-post tag lists into the deduplicated, counted index behind
// /blog/tags. Lives here rather than in a repository so both repositories can
// share it and the counting rules stay in one place.
public static class TagIndex
{
    public static IReadOnlyList<TagSummary> Summarize(IEnumerable<List<string>?> postTags)
    {
        return postTags
            .SelectMany(TagsOf)
            .GroupBy(t => t.Slug, StringComparer.Ordinal)
            .Select(group => new TagSummary
            {
                Slug = group.Key,
                Name = CanonicalName(group.Select(t => t.Name)),
                Count = group.Count()
            })
            .OrderByDescending(t => t.Count)
            .ThenBy(t => t.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // A post that lists "AI" and "ai" is still one post tagged ai, so dedupe by slug
    // before anything is counted.
    private static IEnumerable<(string Slug, string Name)> TagsOf(List<string>? tags) =>
        (tags ?? new List<string>())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => (Slug: TagUrl.Slug(t), Name: t.Trim()))
            .Where(t => t.Slug.Length > 0)
            .DistinctBy(t => t.Slug, StringComparer.Ordinal);

    // Spellings vary across posts. Show whichever one is used most, falling back to
    // alphabetical so the picked name doesn't flip around on a tie.
    private static string CanonicalName(IEnumerable<string> spellings) =>
        spellings
            .GroupBy(s => s, StringComparer.Ordinal)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key, StringComparer.Ordinal)
            .First().Key;
}
