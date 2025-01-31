using Website.Models;
using Website.Repositories;

namespace Website.Services;

public class PhotoService
{
    private readonly PhotoRepository _photoRepository;

    public PhotoService(PhotoRepository photoRepository)
    {
        _photoRepository = photoRepository;
    }

    public PhotoViewModel GetLatestPhoto()
    {
        var photos = _photoRepository.GetAllPhotos();
        if (!photos.Any())
        {
            return new PhotoViewModel();
        }

        var latestPhoto = photos.First();
        var index = photos.IndexOf(latestPhoto);
        var previousPhoto = index + 1 < photos.Count ? photos[index + 1] : null;
        var nextPhoto = index - 1 >= 0 ? photos[index - 1] : null;

        return new PhotoViewModel
        {
            CurrentPhoto = latestPhoto,
            PreviousPhoto = previousPhoto,
            NextPhoto = nextPhoto
        };
    }

    public PhotoViewModel? GetPhotoBySlug(string slug)
{
    var photos = _photoRepository.GetAllPhotos();
    var photo = photos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    if (photo == null)
    {
        return null;
    }

    var index = photos.IndexOf(photo);

    // Only assign previous if there is an older photo in the list
    var previousPhoto = index < photos.Count - 1 ? photos[index + 1] : null;

    // Only assign next if there is a newer photo in the list
    var nextPhoto = index > 0 ? photos[index - 1] : null;

    return new PhotoViewModel
    {
        CurrentPhoto = photo,
        PreviousPhoto = previousPhoto,
        NextPhoto = nextPhoto
    };
}


}

