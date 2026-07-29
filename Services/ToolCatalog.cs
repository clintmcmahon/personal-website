using System.Text.RegularExpressions;
using Website.Models;

namespace Website.Services;

// Shared by ToolsController (the /tools index) and SearchController (site search) so
// the metadata-comment parsing logic for wwwroot/tools/*.html lives in one place.
public static class ToolCatalog
{
    public static List<ToolEntry> GetAll(IWebHostEnvironment env)
    {
        var toolsFolder = Path.Combine(env.WebRootPath, "tools");
        if (!Directory.Exists(toolsFolder))
            return new List<ToolEntry>();

        return Directory.EnumerateFiles(toolsFolder, "*.html")
            .Select(Parse)
            .Where(t => t != null)
            .Select(t => t!)
            .OrderByDescending(t => t.Date)
            .ToList();
    }

    private static ToolEntry? Parse(string filePath)
    {
        var content = File.ReadAllText(filePath);
        var match = Regex.Match(content, @"\A\s*<!--\s*(.*?)\s*-->", RegexOptions.Singleline);
        if (!match.Success) return null;

        var entry = new ToolEntry { Slug = Path.GetFileName(filePath) };

        foreach (var line in match.Groups[1].Value.Split('\n'))
        {
            var parts = line.Trim().Split(':', 2);
            if (parts.Length < 2) continue;

            var value = parts[1].Trim();
            switch (parts[0].Trim().ToLowerInvariant())
            {
                case "title":
                    entry.Title = value;
                    break;
                case "description":
                    entry.Description = value;
                    break;
                case "date":
                    DateTime.TryParse(value, out var date);
                    entry.Date = date;
                    break;
            }
        }

        return string.IsNullOrWhiteSpace(entry.Title) ? null : entry;
    }
}
