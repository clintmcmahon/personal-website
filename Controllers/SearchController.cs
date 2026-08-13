using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class SearchController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly IWebHostEnvironment _env;

    // Photos are no longer indexed here. They live in a separate app with its own
    // database, and reaching across to it would re-couple the two deployments.
    public SearchController(IPostRepository postRepository, IWebHostEnvironment env)
    {
        _postRepository = postRepository;
        _env = env;
    }

    [HttpGet("/search")]
    public IActionResult Index(string? q)
    {
        ViewData["Title"] = "Search | Clint McMahon";
        ViewData["Query"] = q ?? string.Empty;

        var term = q?.Trim() ?? string.Empty;
        if (term.Length == 0)
            return View(new List<SearchResult>());

        // "tag:dotnet" narrows to posts carrying that tag instead of scanning prose.
        // It's the form the tag links themselves teach, so it's worth honoring here.
        if (term.StartsWith("tag:", StringComparison.OrdinalIgnoreCase))
            return View(TagOnlyResults(term[4..].Trim()));

        var results = new List<SearchResult>();

        // Tag hits come first: someone typing a topic usually wants the whole set,
        // not one post that happens to mention the word.
        results.AddRange(_postRepository.GetAllTags()
            .Where(t => Matches(t.Name, term))
            .Select(t => new SearchResult
            {
                Type = "Tag",
                Title = $"#{t.Name}",
                Snippet = $"{t.Count} post{(t.Count == 1 ? "" : "s")} tagged {t.Name}",
                Url = TagUrl.Path(t.Name)
            }));

        var posts = _postRepository.GetAllPosts()
            .Where(p => Matches(p.Title, term)
                     || Matches(p.Description, term)
                     || MatchesAnyTag(p.Tags, term)
                     || Matches(StripHtml(p.Content), term))
            .Select(p => new SearchResult
            {
                Type = "Blog",
                Title = p.Title,
                Snippet = Excerpt(p.Description ?? StripHtml(p.Content), term),
                Url = $"/blog/{p.Slug}",
                Date = p.Date
            });

        var tools = ToolCatalog.GetAll(_env)
            .Where(t => Matches(t.Title, term) || Matches(t.Description, term))
            .Select(t => new SearchResult
            {
                Type = "Tool",
                Title = t.Title,
                Snippet = t.Description,
                Url = $"/tools/{t.Slug}",
                Date = t.Date
            });

        results.AddRange(posts.Concat(tools).OrderByDescending(r => r.Date));

        return View(results);
    }

    private List<SearchResult> TagOnlyResults(string tag)
    {
        if (tag.Length == 0)
            return new List<SearchResult>();

        return _postRepository.GetPostsByTagSlug(tag)
            .OrderByDescending(p => p.Date)
            .Select(p => new SearchResult
            {
                Type = "Blog",
                Title = p.Title,
                Snippet = p.Description ?? Excerpt(StripHtml(p.Content), tag),
                Url = $"/blog/{p.Slug}",
                Date = p.Date
            })
            .ToList();
    }

    private static bool MatchesAnyTag(List<string>? tags, string term) =>
        tags != null && tags.Any(t => Matches(t, term));

    private static bool Matches(string? haystack, string term) =>
        !string.IsNullOrWhiteSpace(haystack) && haystack.Contains(term, StringComparison.OrdinalIgnoreCase);

    private static string StripHtml(string? html) =>
        string.IsNullOrWhiteSpace(html) ? string.Empty : Regex.Replace(html, "<[^>]+>", " ");

    private static string Excerpt(string text, string term)
    {
        text = Regex.Replace(text, @"\s+", " ").Trim();
        if (text.Length <= 160)
            return text;

        var idx = text.IndexOf(term, StringComparison.OrdinalIgnoreCase);
        if (idx < 0)
            return text[..160] + "…";

        var start = Math.Max(0, idx - 60);
        var snippet = text.Substring(start, Math.Min(160, text.Length - start));
        return (start > 0 ? "…" : string.Empty) + snippet + "…";
    }
}
