using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers;

public class AdminBackupController : Controller
{
    private readonly IWebHostEnvironment _env;

    // Relative to ContentRootPath. Excludes bulk/data content that's already
    // protected during deploy (uploads, logs) and dev-only artifacts that only
    // exist when running locally, not on the deployed server.
    private static readonly string[] ExcludedDirPrefixes =
    {
        "bin", "obj", ".git", ".github", ".vs", "node_modules",
        "uploads", "logs"
    };

    private static readonly string[] DatabaseNames =
    {
        "blog.db", "webmentions.db", "guestbook.db"
    };

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
