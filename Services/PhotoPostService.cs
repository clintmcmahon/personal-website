using System.Text;

namespace Website.Services;

public class PhotoPostRequest
{
    public DateTime Date { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Tags { get; set; }
    public string? Body { get; set; }
    public bool LayoutRows { get; set; }
    public bool Draft { get; set; }
    public List<PhotoPostImage> Images { get; set; } = new();
}

public class PhotoPostImage
{
    public string FileName { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public byte[] ResizedBytes { get; set; } = Array.Empty<byte>();
}

public class PhotoPostService
{
    private readonly GitHubService _github;
    private readonly ImageProcessingService _imageProcessor;

    public PhotoPostService(GitHubService github, ImageProcessingService imageProcessor)
    {
        _github = github;
        _imageProcessor = imageProcessor;
    }

    public async Task PublishAsync(PhotoPostRequest request, IEnumerable<(string OriginalFileName, Stream Stream)> uploads)
    {
        var date = request.Date;
        var folderName = date.ToString("yyyy-MM-dd");
        var year = date.Year.ToString();
        var folderPath = $"wwwroot/photos/{year}/{folderName}";

        var images = new List<PhotoPostImage>();
        var filesToCommit = new List<(string RepoPath, byte[] Content)>();

        foreach (var (originalFileName, stream) in uploads)
        {
            var safeFileName = SanitizeFileName(Path.GetFileNameWithoutExtension(originalFileName)) + ".jpeg";
            var resized = await _imageProcessor.ResizeAsync(stream);

            images.Add(new PhotoPostImage
            {
                FileName = safeFileName,
                AltText = string.Empty,
                ResizedBytes = resized
            });

            filesToCommit.Add(($"{folderPath}/{safeFileName}", resized));
        }

        // Pair alt texts from the request with the images in order
        for (int i = 0; i < images.Count && i < request.Images.Count; i++)
        {
            images[i].AltText = request.Images[i].AltText;
            // Override filename if provided
            if (!string.IsNullOrWhiteSpace(request.Images[i].FileName))
                images[i].FileName = SanitizeFileName(Path.GetFileNameWithoutExtension(request.Images[i].FileName)) + ".jpeg";
        }

        // Update commit file paths to use final filenames
        for (int i = 0; i < filesToCommit.Count; i++)
        {
            filesToCommit[i] = ($"{folderPath}/{images[i].FileName}", filesToCommit[i].Content);
        }

        var infoMd = BuildInfoMd(request, images, date);
        filesToCommit.Add(($"{folderPath}/info.md", Encoding.UTF8.GetBytes(infoMd)));

        var commitMessage = $"Add photo post: {request.Title} ({folderName})";
        await _github.CommitFilesAsync(filesToCommit, commitMessage);
    }

    private static string BuildInfoMd(PhotoPostRequest request, List<PhotoPostImage> images, DateTime date)
    {
        var sb = new StringBuilder();
        sb.AppendLine("---");
        sb.AppendLine($"date: \"{date:yyyy-MM-dd}\"");
        sb.AppendLine($"title: \"{EscapeYaml(request.Title)}\"");

        if (!string.IsNullOrWhiteSpace(request.Tags))
        {
            var tags = request.Tags
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(t => t.ToLowerInvariant());
            sb.AppendLine($"tags: [{string.Join(", ", tags)}]");
        }

        if (request.LayoutRows)
            sb.AppendLine("layout: rows");

        if (request.Draft)
            sb.AppendLine("draft: true");

        sb.AppendLine("rows:");
        foreach (var img in images)
        {
            sb.AppendLine($"  - [{img.FileName} | {EscapeYaml(img.AltText)}]");
        }

        sb.AppendLine("---");

        if (!string.IsNullOrWhiteSpace(request.Body))
        {
            sb.AppendLine();
            sb.AppendLine(request.Body.Trim());
        }

        return sb.ToString();
    }

    private static string SanitizeFileName(string name)
    {
        var safe = name.ToLowerInvariant();
        safe = System.Text.RegularExpressions.Regex.Replace(safe, @"[^a-z0-9\-_]", "-");
        safe = System.Text.RegularExpressions.Regex.Replace(safe, @"-{2,}", "-");
        return safe.Trim('-');
    }

    private static string EscapeYaml(string value) =>
        value.Replace("\"", "\\\"");
}
