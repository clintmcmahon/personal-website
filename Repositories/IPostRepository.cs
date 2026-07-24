using Website.Models;

namespace Website.Repositories;

public interface IPostRepository
{
    IEnumerable<Post> GetAllPosts();
    IEnumerable<Post> GetLatestPosts();
    Post GetPostBySlug(string slug);

    // Admin-only: fetch by id, bypassing the draft filter, for previewing unpublished
    // posts. Only DatabasePostRepository needs a real implementation — the file-based
    // migration-only repo inherits this default.
    Post? GetPostByIdIncludingDrafts(int id) =>
        throw new NotSupportedException($"{GetType().Name} does not support draft preview lookups.");
}
