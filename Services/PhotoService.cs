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
}
