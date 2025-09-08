using Website.Models;
using Website.Services;

namespace Website.Models;

public class HomeViewModel
{
    public IEnumerable<Post> LatestPosts { get; set; } = new List<Post>();
    public List<InstagramEmbed> InstagramPosts { get; set; } = new List<InstagramEmbed>();
}
