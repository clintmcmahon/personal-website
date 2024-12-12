using Website.Models;

namespace Website.Repositories;

public interface IPostRepository
{
    IEnumerable<Post> GetAllPosts();
    IEnumerable<Post> GetLatestPosts();
    Post GetPostBySlug(string slug);
}
