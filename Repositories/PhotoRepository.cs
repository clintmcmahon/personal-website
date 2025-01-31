namespace Website.Repositories;

using System.Globalization;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using System.Text.RegularExpressions;
using Website.Models;

public class PhotoRepository
{
    private readonly string _photosDirectory;

    public PhotoRepository(string photosDirectory)
    {
        _photosDirectory = photosDirectory;
    }

    public List<PhotoEntry> GetAllPhotos()
    {
        var photos = new List<PhotoEntry>();

        if (!Directory.Exists(_photosDirectory))
            return photos;

        var files = Directory.GetFiles(_photosDirectory, "*.md", SearchOption.AllDirectories);
        
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            var entry = ParseMarkdown(content);
            if (entry != null)
            {
                photos.Add(entry);
            }
        }

        return photos.OrderByDescending(p => p.Date).ToList();
    }

    public PhotoEntry? GetPhotoBySlug(string slug)
    {
        var photos = GetAllPhotos();
        return photos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
    }

    private PhotoEntry? ParseMarkdown(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return null;

        var pipeline = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseAdvancedExtensions()
            .Build();

        var document = Markdown.Parse(content, pipeline);
        var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

        if (yamlBlock == null) return null;

        var yamlText = content.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length).Trim();
        var metadata = ParseYamlMetadata(yamlText);

        if (!metadata.TryGetValue("title", out var title) ||
            !metadata.TryGetValue("date", out var dateString) ||
            !metadata.TryGetValue("image", out var image) ||
            !DateTime.TryParse(dateString, out var date))
        {
            return null;
        }

        var slug = metadata.ContainsKey("slug") ? metadata["slug"] : GenerateSlug(title);
        var tags = metadata.ContainsKey("tags") 
            ? metadata["tags"].Split(',').Select(tag => tag.Trim().Trim('"').Trim('\'')).ToList() 
            : new List<string>();

        var markdownContent = content.Substring(yamlBlock.Span.End).Trim();
        var htmlContent = Markdown.ToHtml(markdownContent, pipeline);

        return new PhotoEntry
        {
            Title = title,
            Slug = slug,
            Date = date,
            ImageUrl = image,
            Content = htmlContent,
            Tags = tags
        };
    }

    private static Dictionary<string, string> ParseYamlMetadata(string yaml)
    {
        var metadata = new Dictionary<string, string>();
        var lines = yaml.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var match = Regex.Match(line, @"^(?<key>[^:]+):\s*(?<value>.+)$");
            if (match.Success)
            {
                var key = match.Groups["key"].Value.Trim();
                var value = match.Groups["value"].Value.Trim();

                value = value.Trim('"').Trim('\'');
                metadata[key] = value;
            }
        }

        return metadata;
    }

    private static string GenerateSlug(string title)
    {
        return title.ToLower().Replace(" ", "-").Replace("'", "").Replace("\"", "");
    }
}
