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
        post.Content = Markdown.ToHtml(post.Content ?? string.Empty, _pipeline);
        return post;
    }
}
