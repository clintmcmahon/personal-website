namespace Website.Models;

public class StaticInstagramPost
{
    public string ImageUrl { get; set; } = string.Empty;
    public string PostUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
}

public static class InstagramData
{
    public static List<StaticInstagramPost> GetLatestPosts()
    {
        return new List<StaticInstagramPost>
        {
            new() { ImageUrl = "/images/instagram/post1.jpg", PostUrl = "https://instagram.com/p/POST_ID", Caption = "Caption" },
            new() { ImageUrl = "/images/instagram/post2.jpg", PostUrl = "https://instagram.com/p/POST_ID", Caption = "Caption" },
            // Add more posts...
        };
    }
}
