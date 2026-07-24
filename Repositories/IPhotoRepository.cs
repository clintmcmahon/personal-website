using Website.Models;

namespace Website.Repositories;

public interface IPhotoRepository
{
    List<PhotoEntry> GetAllPhotos();
    List<PhotoEntry> GetPhotosByTag(string tag);
    PhotoEntry? GetPhotoBySlug(string slug);

    // Admin-only: fetch by id, bypassing the draft filter, for previewing unpublished
    // photos. Only DatabasePhotoRepository needs a real implementation — the file-based
    // migration-only repo inherits this default.
    PhotoEntry? GetPhotoByIdIncludingDrafts(int id) =>
        throw new NotSupportedException($"{GetType().Name} does not support draft preview lookups.");
}
