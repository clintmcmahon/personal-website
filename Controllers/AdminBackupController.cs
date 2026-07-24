using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers;

public class AdminBackupController : Controller
{
    private readonly IWebHostEnvironment _env;

    // Relative to ContentRootPath. Excludes bulk/data content that's already
    // protected during deploy (photos, uploads, logs) and dev-only artifacts
    // that only exist when running locally, not on the deployed server.
    private static readonly string[] ExcludedDirPrefixes =
    {
        "bin", "obj", ".git", ".github", ".vs", "node_modules",
        Path.Combine("wwwroot", "photos"), "uploads", "logs"
    };

    private static readonly string[] DatabaseNames =
    {
        "blog.db", "photos.db", "photocomments.db", "webmentions.db"
    };

    // Photos backup only goes back this far — older years are large and already
    // backed up elsewhere, so they're skipped to keep the download manageable.
    private static readonly DateTime PhotosCutoff = new(2026, 7, 1);

    public AdminBackupController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet("/admin/backup")]
    public IActionResult Index()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/backup");

        var databases = DatabaseNames
            .Select(name => new FileInfo(Path.Combine(_env.ContentRootPath, name)))
            .Where(f => f.Exists)
            .Select(f => new BackupFileInfo(f.Name, Math.Round(f.Length / 1024.0 / 1024.0, 2), f.LastWriteTimeUtc))
            .ToList();

        return View(databases);
    }

    [HttpGet("/admin/backup/app.zip")]
    public IActionResult AppZip()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/backup");

        var root = _env.ContentRootPath;
        var fileName = $"clintmcmahon-app-{DateTime.UtcNow:yyyy-MM-dd}.zip";

        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var fullPath in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(root, fullPath);
                if (ShouldExcludeFromAppZip(relative)) continue;

                var entry = archive.CreateEntry(relative.Replace('\\', '/'), CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                fileStream.CopyTo(entryStream);
            }
        }

        return File(ms.ToArray(), "application/zip", fileName);
    }

    [HttpGet("/admin/backup/databases.zip")]
    public IActionResult DatabasesZip()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/backup");

        var root = _env.ContentRootPath;
        var fileName = $"clintmcmahon-databases-{DateTime.UtcNow:yyyy-MM-dd}.zip";

        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var dbName in DatabaseNames)
            {
                // Main file plus WAL-mode companions, when present.
                foreach (var suffix in new[] { "", "-shm", "-wal" })
                {
                    var fullPath = Path.Combine(root, dbName + suffix);
                    if (!System.IO.File.Exists(fullPath)) continue;

                    var entry = archive.CreateEntry(Path.GetFileName(fullPath), CompressionLevel.Optimal);
                    using var entryStream = entry.Open();
                    using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        return File(ms.ToArray(), "application/zip", fileName);
    }

    [HttpGet("/admin/backup/photos.zip")]
    public IActionResult PhotosZip()
    {
        if (!AuthController.IsLoggedIn(HttpContext))
            return Redirect("/auth/login?returnUrl=/admin/backup");

        var photosRoot = Path.Combine(_env.WebRootPath, "photos");
        var fileName = $"clintmcmahon-photos-{DateTime.UtcNow:yyyy-MM-dd}.zip";

        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            if (Directory.Exists(photosRoot))
            {
                foreach (var fullPath in Directory.EnumerateFiles(photosRoot, "*", SearchOption.AllDirectories))
                {
                    var relative = Path.GetRelativePath(photosRoot, fullPath);
                    if (!IsOnOrAfterPhotosCutoff(relative, fullPath)) continue;

                    var entry = archive.CreateEntry(relative.Replace('\\', '/'), CompressionLevel.Optimal);
                    using var entryStream = entry.Open();
                    using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    fileStream.CopyTo(entryStream);
                }
            }
        }

        return File(ms.ToArray(), "application/zip", fileName);
    }

    // Photo folders are laid out as {year}/{yyyy-MM-dd}/filename, and that
    // folder name is the real post date — more reliable than file mtime,
    // which can just reflect whenever the file was last copied/touched.
    private static bool IsOnOrAfterPhotosCutoff(string relativePath, string fullPath)
    {
        var parts = relativePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (parts.Length >= 2 && DateTime.TryParseExact(parts[1], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var postDate))
            return postDate >= PhotosCutoff;

        return System.IO.File.GetLastWriteTimeUtc(fullPath) >= PhotosCutoff;
    }

    private static bool ShouldExcludeFromAppZip(string relativePath)
    {
        foreach (var prefix in ExcludedDirPrefixes)
        {
            if (relativePath.StartsWith(prefix + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        if (relativePath.EndsWith(".db", StringComparison.OrdinalIgnoreCase)) return true;
        if (relativePath.EndsWith(".db-shm", StringComparison.OrdinalIgnoreCase)) return true;
        if (relativePath.EndsWith(".db-wal", StringComparison.OrdinalIgnoreCase)) return true;

        return false;
    }
}
