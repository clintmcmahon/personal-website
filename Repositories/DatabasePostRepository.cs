using Markdig;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

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

    public IEnumerable<Post> GetPostsByTag(string tag)
    {
        return _db.Posts
            .AsNoTracking()
            .Where(p => !p.Draft)
            .ToList()
            .Where(p => p.Tags != null && p.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase))
            .Select(RenderContent);
    }

    private static Post RenderContent(Post post)
    {
        post.Content = Markdown.ToHtml(post.Content ?? string.Empty, _pipeline);
        return post;
    }
}
