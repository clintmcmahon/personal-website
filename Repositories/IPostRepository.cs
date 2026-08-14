using Website.Models;

namespace Website.Repositories;

public interface IPostRepository
{
    IEnumerable<Post> GetAllPosts();
    IEnumerable<Post> GetLatestPosts();
    Post GetPostBySlug(string slug);

    // Matched on the slug, not the raw tag text, so "ASP.NET Core" and "asp.net core"
    // resolve to the same page. See Services/TagUrl.
    IEnumerable<Post> GetPostsByTagSlug(string tagSlug);

    // Every tag on a published post, with counts, for the /blog/tags index.
    IReadOnlyList<TagSummary> GetAllTags();

    // Admin-only: fetch by id, bypassing the draft filter, for previewing unpublished
    // posts. Only DatabasePostRepository needs a real implementation — the file-based
    // migration-only repo inherits this default.
    Post? GetPostByIdIncludingDrafts(int id) =>
        throw new NotSupportedException($"{GetType().Name} does not support draft preview lookups.");
}
