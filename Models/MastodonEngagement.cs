namespace Website.Models;

public record MastodonAccountView(string Name, string? Avatar, string Url);

public record MastodonReplyView(
    string AuthorName,
    string? AuthorAvatar,
    string AuthorUrl,
    string Content,
    DateTime PublishedAt,
    string Url);

public class MastodonEngagement
{
    public int Likes { get; set; }
    public int Boosts { get; set; }
    public int RepliesCount { get; set; }
    public List<MastodonAccountView> Likers { get; set; } = new();
    public List<MastodonAccountView> Boosters { get; set; } = new();
    public List<MastodonReplyView> Replies { get; set; } = new();
}
