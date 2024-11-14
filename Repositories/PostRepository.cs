using Website.Models;

namespace Website.Repositories;

using Markdig;
using System.Text.RegularExpressions;

public class PostRepository : IPostRepository
{
    private readonly string _postsFolderPath = Path.Combine("wwwroot", "posts");

    public IEnumerable<Post> GetAllPosts()
    {
        var files = Directory.GetFiles(_postsFolderPath, "*.md");
        return files.Select(ParseMarkdownFile);
    }

    public Post GetPostBySlug(string slug)
    {
        var filePath = Directory.GetFiles(_postsFolderPath, $"*-{slug}.md").FirstOrDefault();
        return filePath != null ? ParseMarkdownFile(filePath) : null;
    }

    private Post ParseMarkdownFile(string filePath)
    {
        var content = File.ReadAllText(filePath);

        // Use regex to extract front matter and Markdown content
        var frontMatterMatch = Regex.Match(content, @"^---\s*(.*?)\s*---\s*(.*)", RegexOptions.Singleline);
        if (!frontMatterMatch.Success)
            throw new Exception("Invalid front matter format.");

        var frontMatter = frontMatterMatch.Groups[1].Value;
        var markdownContent = frontMatterMatch.Groups[2].Value;

        // Parse front matter
        var post = ParseFrontMatter(frontMatter);

        // Set the HTML content after rendering Markdown
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        post.Content = Markdown.ToHtml(markdownContent, pipeline);

        return post;
    }

    private Post ParseFrontMatter(string frontMatter)
    {
        var post = new Post();
        var frontMatterLines = frontMatter.Split('\n');

        foreach (var line in frontMatterLines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            var parts = trimmedLine.Split(':', 2);
            if (parts.Length < 2) continue;

            var key = parts[0].Trim().ToLowerInvariant();
            var value = parts[1].Trim().Trim('"', '\''); // Remove both single and double quotes

            switch (key)
            {
                case "title":
                    post.Title = value;
                    break;
                case "description":
                    post.Description = value;
                    break;
                case "date":
                    post.Date = DateTime.Parse(value);
                    break;
                case "draft":
                    post.Draft = bool.Parse(value);
                    break;
                case "slug":
                    post.Slug = value;
                    break;
                case "tags":
                    post.Tags = value.Split(',').Select(tag => tag.Trim()).ToList();
                    break;
            }
        }

        return post;
    }


}
