using Website.Models;

namespace Website.Repositories;

public interface IPostRepository
{
    IEnumerable<Post> GetAllPosts();
    Post GetPostBySlug(string slug);
}
