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

        // Get markdown override files
        var markdownFiles = Directory.GetFiles(_photosDirectory, "*.md", SearchOption.AllDirectories)
            .ToDictionary(f => Path.GetFileNameWithoutExtension(f), f => f, StringComparer.OrdinalIgnoreCase);

        foreach (var imageFile in imageFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(imageFile);
            
            // Check if there's a markdown override
            PhotoEntry? entry = null;
            if (markdownFiles.TryGetValue(fileName, out var mdFile))
            {
                var content = File.ReadAllText(mdFile);
                entry = ParseMarkdown(content);
            }
            
            // If no markdown or parsing failed, create from image
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

    public List<PhotoAlbum> GetAllAlbums()
    {
        var albums = new List<PhotoAlbum>();

        if (!Directory.Exists(_photosDirectory))
            return albums;

        // Get all subdirectories (each is an album)
        var albumDirs = Directory.GetDirectories(_photosDirectory);

        foreach (var albumDir in albumDirs)
        {
            var folderName = Path.GetFileName(albumDir);
            
            // Check for album.md file
            var albumMdPath = Path.Combine(albumDir, "album.md");
            PhotoAlbum? album = null;

            if (File.Exists(albumMdPath))
            {
                var content = File.ReadAllText(albumMdPath);
                album = ParseAlbumMetadata(content, albumDir, folderName);
            }

            // If no album.md, create from folder
            if (album == null)
            {
                album = CreateAlbumFromFolder(albumDir, folderName);
            }

            if (album != null)
            {
                albums.Add(album);
            }
        }

        return albums.OrderByDescending(a => a.Date).ToList();
    }

    public List<PhotoEntry> GetPhotosForAlbum(string slug)
    {
        var albums = GetAllAlbums();
        var album = albums.FirstOrDefault(a => a.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        
        if (album == null)
            return new List<PhotoEntry>();

        var albumPath = album.FolderPath;
        var imageFiles = Directory.GetFiles(albumPath, "*.*")
            .Where(f => 
            {
                var ext = Path.GetExtension(f);
                return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext.ToLower());
            })
            .ToList();

        var photos = new List<PhotoEntry>();
        foreach (var imageFile in imageFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(imageFile);
            var relativePath = imageFile.Replace(_photosDirectory, "/photos").Replace("\\", "/");
            
            photos.Add(new PhotoEntry
            {
                Title = fileName,
                Slug = GenerateSlug(fileName),
                Date = album.Date,
                ImageUrl = relativePath,
                Content = string.Empty,
                Tags = album.Tags
            });
        }

        return photos;
    }

    private PhotoAlbum? ParseAlbumMetadata(string content, string folderPath, string folderName)
    {
        if (string.IsNullOrWhiteSpace(content)) return null;

        // Manually extract YAML front matter
        if (!content.TrimStart().StartsWith("---")) return null;
        
        var lines = content.Split('\n');
        var yamlLines = new List<string>();
        var contentLines = new List<string>();
        bool inYaml = false;
        bool yamlEnded = false;
        
        foreach (var line in lines)
        {
            if (line.Trim() == "---")
            {
                if (!inYaml)
                {
                    inYaml = true;
                }
                else
                {
                    inYaml = false;
                    yamlEnded = true;
                }
                continue;
            }
            
            if (inYaml)
            {
                yamlLines.Add(line);
            }
            else if (yamlEnded)
            {
                contentLines.Add(line);
            }
        }

        var yamlText = string.Join('\n', yamlLines);
        var metadata = ParseYamlMetadata(yamlText);

        if (!metadata.TryGetValue("title", out var title) ||
            !metadata.TryGetValue("date", out var dateString) ||
            !DateTime.TryParse(dateString, out var date))
        {
            return null;
        }

        var slug = metadata.ContainsKey("slug") ? metadata["slug"] : folderName;
        var description = metadata.ContainsKey("description") ? metadata["description"] : string.Empty;
        var thumbnail = metadata.ContainsKey("thumbnail") ? metadata["thumbnail"] : string.Empty;
        var tags = metadata.ContainsKey("tags") 
            ? metadata["tags"].Split(',').Select(tag => tag.Trim().Trim('"').Trim('\'')).ToList() 
            : new List<string>();

        // Parse markdown content (everything after YAML front matter)
        var markdownContent = string.Join('\n', contentLines).Trim();
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
        var htmlContent = !string.IsNullOrWhiteSpace(markdownContent) ? Markdown.ToHtml(markdownContent, pipeline) : string.Empty;

        // Get photo count
        var imageFiles = Directory.GetFiles(folderPath, "*.*")
            .Where(f => 
            {
                var ext = Path.GetExtension(f);
                return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext.ToLower());
            })
            .ToList();

        // Build thumbnail URL
        var thumbnailUrl = string.Empty;
        if (!string.IsNullOrEmpty(thumbnail))
        {
            thumbnailUrl = $"/photos/{folderName}/{thumbnail}";
        }
        else if (imageFiles.Any())
        {
            var firstImage = Path.GetFileName(imageFiles.First());
            thumbnailUrl = $"/photos/{folderName}/{firstImage}";
        }

        return new PhotoAlbum
        {
            Title = title,
            Description = description,
            Slug = slug,
            Date = date,
            PhotoCount = imageFiles.Count,
            ThumbnailUrl = thumbnailUrl,
            Tags = tags,
            FolderPath = folderPath,
            Content = htmlContent
        };
    }

    private PhotoAlbum CreateAlbumFromFolder(string folderPath, string folderName)
    {
        var imageFiles = Directory.GetFiles(folderPath, "*.*")
            .Where(f => 
            {
                var ext = Path.GetExtension(f);
                return !string.IsNullOrEmpty(ext) && ImageExtensions.Contains(ext.ToLower());
            })
            .ToList();

        var thumbnailUrl = string.Empty;
        if (imageFiles.Any())
        {
            var firstImage = Path.GetFileName(imageFiles.First());
            thumbnailUrl = $"/photos/{folderName}/{firstImage}";
        }

        // Try to extract date from folder name (e.g., "2025-04" or "seattle-2025")
        var dateMatch = Regex.Match(folderName, @"(\d{4})[-_]?(\d{2})?");
        var date = DateTime.Now;
        if (dateMatch.Success && int.TryParse(dateMatch.Groups[1].Value, out var year))
        {
            var month = dateMatch.Groups[2].Success && int.TryParse(dateMatch.Groups[2].Value, out var m) ? m : 1;
            date = new DateTime(year, month, 1);
        }

        // Generate title from folder name
        var title = GenerateTitleFromFilename(folderName);

        return new PhotoAlbum
        {
            Title = title,
            Description = string.Empty,
            Slug = folderName,
            Date = date,
            PhotoCount = imageFiles.Count,
            ThumbnailUrl = thumbnailUrl,
            Tags = new List<string>(),
            FolderPath = folderPath
        };
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
