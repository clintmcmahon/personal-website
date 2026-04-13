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
    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

    public PhotoRepository(string photosDirectory)
    {
        _photosDirectory = photosDirectory;
    }

    public List<PhotoEntry> GetAllPhotos()
    {
        var photos = new List<PhotoEntry>();

        if (!Directory.Exists(_photosDirectory))
            return photos;

        // Get all image files
        var imageFiles = Directory.GetFiles(_photosDirectory, "*.*", SearchOption.AllDirectories)
            .Where(f => 
            {
                var ext = Path.GetExtension(f);
                return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext.ToLower());
            })
            .ToList();

        // Get markdown override files using unique relative path as key
        var markdownFiles = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var mdFile in Directory.GetFiles(_photosDirectory, "*.md", SearchOption.AllDirectories))
        {
            // Use relative path from photos root (without extension) as key
            var relPath = Path.GetRelativePath(_photosDirectory, mdFile);
            var key = relPath.Substring(0, relPath.Length - Path.GetExtension(relPath).Length).Replace("\\", "/");
            if (!markdownFiles.ContainsKey(key))
            {
                markdownFiles[key] = mdFile;
            }
        }

        foreach (var imageFile in imageFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(imageFile);
            var relImagePath = Path.GetRelativePath(_photosDirectory, imageFile);
            var mdKey = relImagePath.Substring(0, relImagePath.Length - Path.GetExtension(relImagePath).Length).Replace("\\", "/");

            PhotoEntry? entry = null;

            // 1. Check for an image-specific .md override (e.g., soo-line.md)
            if (markdownFiles.TryGetValue(mdKey, out var mdFile))
            {
                var content = File.ReadAllText(mdFile);
                entry = ParseMarkdown(content, imageFile);
            }

            // 2. Fall back to a folder-level .md (e.g., 2026-04-10/2026-04-10.md)
            if (entry == null)
            {
                var dirName = Path.GetFileName(Path.GetDirectoryName(imageFile) ?? "");
                var dirMdKey = $"{dirName}/{dirName}";
                if (markdownFiles.TryGetValue(dirMdKey, out var dirMdFile))
                {
                    var content = File.ReadAllText(dirMdFile);
                    entry = ParseMarkdown(content, imageFile);
                }
            }

            // 3. Generate from image filename
            if (entry == null)
            {
                entry = CreatePhotoFromImage(imageFile);
            }

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

    private PhotoEntry? CreatePhotoFromImage(string imagePath)
    {
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(imagePath);
            var relativePath = imagePath.Replace(_photosDirectory, "/photos").Replace("\\", "/");
            
            // Extract date from directory structure: photos/2025-04/image.jpg
            var pathParts = imagePath.Split(Path.DirectorySeparatorChar);
            var dateFromPath = ExtractDateFromPath(pathParts);
            DateTime photoDate = dateFromPath ?? DateTime.Now;
            
            // Generate title from filename
            var title = GenerateTitleFromFilename(fileName);
            
            return new PhotoEntry
            {
                Title = title,
                Slug = GenerateSlug(fileName),
                Date = photoDate,
                ImageUrl = relativePath,
                Content = string.Empty,
                Tags = new List<string>()
            };
        }
        catch
        {
            return null;
        }
    }

    private DateTime? ExtractDateFromPath(string[] pathParts)
    {
        try
        {
            // Look for year/month pattern in path
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                if (int.TryParse(pathParts[i], out var year) && year >= 2000 && year <= 2100)
                {
                    if (int.TryParse(pathParts[i + 1], out var month) && month >= 1 && month <= 12)
                    {
                        return new DateTime(year, month, 1);
                    }
                }
            }
        }
        catch { }
        
        return null;
    }

    private string GenerateTitleFromFilename(string filename)
    {
        // Remove common camera prefixes (IMG_, DSC_, etc.)
        var cleaned = Regex.Replace(filename, @"^(IMG|DSC|DSCN|DSC_|IMG_|PICT|R\d+)", "", RegexOptions.IgnoreCase);
        
        // If nothing left, use original
        if (string.IsNullOrWhiteSpace(cleaned))
            cleaned = filename;
        
        // Replace underscores and hyphens with spaces
        cleaned = cleaned.Replace("_", " ").Replace("-", " ");
        
        // Capitalize first letter
        if (!string.IsNullOrEmpty(cleaned))
        {
            cleaned = char.ToUpper(cleaned[0]) + cleaned.Substring(1);
        }
        
        return cleaned.Trim();
    }

    private PhotoEntry? ParseMarkdown(string content, string? imagePathFallback = null)
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

        // date is required
        if (!metadata.TryGetValue("date", out var dateString) ||
            !DateTime.TryParse(dateString, out var date))
        {
            return null;
        }

        // title is optional — fall back to the date, never to description
        metadata.TryGetValue("title", out var title);
        if (string.IsNullOrWhiteSpace(title))
            title = date.ToString("MMMM d, yyyy");

        // image: single string  OR  images: [file.jpg, ...]  OR  derive from the actual image file
        string? imageUrl = null;
        if (metadata.TryGetValue("image", out var singleImage) && !string.IsNullOrWhiteSpace(singleImage))
        {
            imageUrl = singleImage;
        }
        else if (metadata.TryGetValue("images", out var imagesValue))
        {
            var first = imagesValue.Trim('[', ']').Split(',').FirstOrDefault()?.Trim().Trim('"').Trim('\'');
            if (!string.IsNullOrWhiteSpace(first))
                imageUrl = first;
        }

        // If image is a bare filename, build the path from the folder
        if (!string.IsNullOrWhiteSpace(imageUrl) && !imageUrl.StartsWith("/") && !imageUrl.StartsWith("http"))
        {
            var folderName = imagePathFallback != null
                ? Path.GetFileName(Path.GetDirectoryName(imagePathFallback) ?? "")
                : "";
            imageUrl = $"/photos/{folderName}/{imageUrl}";
        }

        // Last resort: use the actual image file we were called with
        if (string.IsNullOrWhiteSpace(imageUrl) && imagePathFallback != null)
        {
            imageUrl = imagePathFallback.Replace(_photosDirectory, "/photos").Replace("\\", "/");
        }

        if (string.IsNullOrWhiteSpace(imageUrl)) return null;

        var slug = metadata.ContainsKey("slug") ? metadata["slug"] : GenerateSlug(title);
        var tags = ParseTagsValue(metadata.ContainsKey("tags") ? metadata["tags"] : "");

        // Find the closing --- and take everything after it to avoid
        // Span.End landing on the last dash and generating a stray list item
        var bodyContent = string.Empty;
        var closingDelimiter = content.IndexOf("\n---", 3);
        if (closingDelimiter >= 0)
        {
            var lineEnd = content.IndexOf('\n', closingDelimiter + 1);
            bodyContent = lineEnd >= 0 ? content.Substring(lineEnd + 1) : string.Empty;
        }
        var htmlContent = Markdown.ToHtml(bodyContent.Trim(), pipeline);

        return new PhotoEntry
        {
            Title = title,
            Slug = slug,
            Date = date,
            ImageUrl = imageUrl,
            Content = htmlContent,
            Tags = tags
        };
    }

    // Handles both "tag1, tag2" strings and "[tag1, tag2]" YAML inline arrays
    private static List<string> ParseTagsValue(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return new List<string>();
        return raw.Trim('[', ']')
                  .Split(',')
                  .Select(t => t.Trim().Trim('"').Trim('\''))
                  .Where(t => !string.IsNullOrWhiteSpace(t))
                  .ToList();
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
