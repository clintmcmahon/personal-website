using Markdig;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Services;

namespace Website.Repositories;

public class DatabasePostRepository : IPostRepository
{
    private readonly BlogDbContext _db;
    private static readonly MarkdownPipeline _pipeline =
        new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public DatabasePostRepository(BlogDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Post> GetAllPosts()
    {
        return _db.Posts
            .AsNoTracking()
            .Where(p => !p.Draft)
            .OrderByDescending(p => p.Date)
            .ToList()
            .Select(RenderContent);
    }

    public IEnumerable<Post> GetLatestPosts()
    {
        return _db.Posts
            .AsNoTracking()
            .Where(p => !p.Draft)
            .OrderByDescending(p => p.Date)
            .Take(5)
            .ToList()
            .Select(RenderContent);
    }

    public Post GetPostBySlug(string slug)
    {
        var post = _db.Posts
            .AsNoTracking()
            .FirstOrDefault(p => p.Slug == slug && !p.Draft);

        return post != null ? RenderContent(post) : null;
    }

    // Tags are persisted as one delimited string (see BlogDbContext's value converter),
    // so there is nothing for SQL to filter on — the match has to happen in memory
    // after materializing. Same reason GetAllTags projects the column out by itself.
    public IEnumerable<Post> GetPostsByTagSlug(string tagSlug)
    {
        var slug = TagUrl.Slug(tagSlug);
        if (slug.Length == 0)
            return Enumerable.Empty<Post>();

        return _db.Posts
            .AsNoTracking()
            .Where(p => !p.Draft)
            .OrderByDescending(p => p.Date)
            .ToList()
            .Where(p => p.Tags != null && p.Tags.Any(t => TagUrl.Slug(t) == slug))
            .Select(RenderContent);
    }

    public IReadOnlyList<TagSummary> GetAllTags()
    {
        var tagLists = _db.Posts
            .AsNoTracking()
            .Where(p => !p.Draft)
            .Select(p => p.Tags)
            .ToList();

        return TagIndex.Summarize(tagLists);
    }

    public Post? GetPostByIdIncludingDrafts(int id)
    {
        var post = _db.Posts.AsNoTracking().FirstOrDefault(p => p.Id == id);
        return post != null ? RenderContent(post) : null;
    }

    private static Post RenderContent(Post post)
    {
        post.Content = RenderMarkdown(post.Content);
        return post;
    }

    // Shared with the admin editor's live preview so unsaved drafts render through the
    // same pipeline as published posts.
    public static string RenderMarkdown(string? markdown) =>
        UpgradeLegacyCodeBlocks(Markdown.ToHtml(markdown ?? string.Empty, _pipeline));

    // Posts imported from WordPress carry code as
    //   <pre class="EnlighterJSRAW" data-enlighter-language="yaml">...</pre>
    // which only meant something to the EnlighterJS plugin. Rewritten here to the
    // <pre><code class="language-x"> shape Markdig emits for fenced blocks, so old
    // and new posts get the same highlighting without editing every old post.
    private static readonly System.Text.RegularExpressions.Regex LegacyCodeBlock = new(
        @"<pre\b(?<attrs>[^>]*\bEnlighterJSRAW\b[^>]*)>(?<code>.*?)</pre>",
        System.Text.RegularExpressions.RegexOptions.Singleline |
        System.Text.RegularExpressions.RegexOptions.IgnoreCase |
        System.Text.RegularExpressions.RegexOptions.Compiled);

    private static readonly System.Text.RegularExpressions.Regex LegacyLanguage = new(
        @"data-enlighter-language\s*=\s*""(?<lang>[\w+#-]*)""",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase |
        System.Text.RegularExpressions.RegexOptions.Compiled);

    private static string UpgradeLegacyCodeBlocks(string html) =>
        LegacyCodeBlock.Replace(html, m =>
        {
            var lang = LegacyLanguage.Match(m.Groups["attrs"].Value).Groups["lang"].Value.ToLowerInvariant();

            // Enlighter names that Prism calls something else, or has no grammar for.
            lang = lang switch
            {
                "" or "generic" or "raw" or "no-highlight" => "none",
                "html" or "xml" => "markup",
                "shell" => "bash",
                "c#" => "csharp",
                _ => lang
            };

            // A newline straight after <code> would render as an empty first line.
            var code = m.Groups["code"].Value.TrimStart('\r', '\n').TrimEnd();
            return $"<pre><code class=\"language-{lang}\">{code}</code></pre>";
        });
}
