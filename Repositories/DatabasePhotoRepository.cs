using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Repositories;

public class DatabasePhotoRepository : IPhotoRepository
{
    private readonly PhotoDbContext _db;

    public DatabasePhotoRepository(PhotoDbContext db)
    {
        _db = db;
    }

    public List<PhotoEntry> GetAllPhotos() =>
        _db.Photos.AsNoTracking()
            .Where(p => !p.Draft)
            .OrderByDescending(p => p.Date)
            .ToList();

    public List<PhotoEntry> GetPhotosByTag(string tag)
    {
        var normalized = tag.ToLowerInvariant().Trim();
        return GetAllPhotos()
            .Where(p => p.Tags.Any(t => t.ToLowerInvariant().Trim() == normalized))
            .ToList();
    }

    public PhotoEntry? GetPhotoBySlug(string slug) =>
        _db.Photos.AsNoTracking()
            .FirstOrDefault(p => p.Slug == slug && !p.Draft);
}
