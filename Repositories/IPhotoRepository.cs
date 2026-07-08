using Website.Models;

namespace Website.Repositories;

public interface IPhotoRepository
{
    List<PhotoEntry> GetAllPhotos();
    List<PhotoEntry> GetPhotosByTag(string tag);
    PhotoEntry? GetPhotoBySlug(string slug);
}
