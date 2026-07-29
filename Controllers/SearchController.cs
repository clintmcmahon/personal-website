using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Repositories;
using Website.Services;

namespace Website.Controllers;

public class SearchController : Controller
{
    private readonly IPostRepository _postRepository;
    private readonly IPhotoRepository _photoRepository;
    private readonly IWebHostEnvironment _env;

    public SearchController(IPostRepository postRepository, IPhotoRepository photoRepository, IWebHostEnvironment env)
    {
        _postRepository = postRepository;
        _photoRepository = photoRepository;
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

        var results = new List<SearchResult>();

        results.AddRange(_postRepository.GetAllPosts()
            .Where(p => Matches(p.Title, term) || Matches(p.Description, term) || Matches(StripHtml(p.Content), term))
            .Select(p => new SearchResult
            {
                Type = "Blog",
                Title = p.Title,
                Snippet = Excerpt(p.Description ?? StripHtml(p.Content), term),
                Url = $"/blog/{p.Slug}",
                Date = p.Date
            }));

        results.AddRange(_photoRepository.GetAllPhotos()
            .Where(p => Matches(p.Title, term) || Matches(StripHtml(p.Content), term) || p.Tags.Any(t => Matches(t, term)))
            .Select(p => new SearchResult
            {
                Type = "Photo",
                Title = string.IsNullOrWhiteSpace(p.Title) ? p.Date.ToString("MMMM d, yyyy") : p.Title,
                Snippet = Excerpt(StripHtml(p.Content), term),
                Url = CanonicalUrlHelper.Photo(p.Date),
                Date = p.Date
            }));

        results.AddRange(ToolCatalog.GetAll(_env)
            .Where(t => Matches(t.Title, term) || Matches(t.Description, term))
            .Select(t => new SearchResult
            {
                Type = "Tool",
                Title = t.Title,
                Snippet = t.Description,
                Url = $"/tools/{t.Slug}",
                Date = t.Date
            }));

        return View(results.OrderByDescending(r => r.Date).ToList());
    }

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
