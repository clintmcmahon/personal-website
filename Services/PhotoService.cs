using Website.Models;
using Website.Repositories;
using Website.Data;

namespace Website.Services;

public class PhotoService
{
    private readonly IPhotoRepository _photoRepository;
    private readonly PhotoCommentDbContext _dbContext;

    public PhotoService(IPhotoRepository photoRepository, PhotoCommentDbContext dbContext)
    {
        _photoRepository = photoRepository;
        _dbContext = dbContext;
    }

    public PhotoIndexViewModel GetPhotosForIndex() =>
        BuildIndexViewModel(_photoRepository.GetAllPhotos(), null);

    public PhotoIndexViewModel GetPhotosByTag(string tag)
    {
        var all = _photoRepository.GetAllPhotos();
        var filtered = _photoRepository.GetPhotosByTag(tag);
        return BuildIndexViewModel(filtered, tag, all);
    }

    public PhotoViewModel GetLatestPhoto()
    {
        var photos = _photoRepository.GetAllPhotos();
        if (!photos.Any())
            return new PhotoViewModel();

        var latest = photos.First();
        var index = photos.IndexOf(latest);

        return new PhotoViewModel
        {
            CurrentPhoto = latest,
            PreviousPhoto = index + 1 < photos.Count ? photos[index + 1] : null,
            NextPhoto = index - 1 >= 0 ? photos[index - 1] : null,
            Comments = _dbContext.PhotoComments
                .Where(c => c.PhotoDate == latest.Date.ToString("yyyy-MM-dd"))
                .OrderByDescending(c => c.CreatedAt)
                .ToList()
        };
    }

    public PhotoViewModel? GetPhotoBySlug(string slug)
    {
        var photos = _photoRepository.GetAllPhotos();
        var photo = photos.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        if (photo == null) return null;

        var index = photos.IndexOf(photo);
        return new PhotoViewModel
        {
            CurrentPhoto = photo,
            PreviousPhoto = index < photos.Count - 1 ? photos[index + 1] : null,
            NextPhoto = index > 0 ? photos[index - 1] : null,
            Comments = _dbContext.PhotoComments
                .Where(c => c.PhotoDate == photo.Date.ToString("yyyy-MM-dd"))
                .OrderByDescending(c => c.CreatedAt)
                .ToList()
        };
    }

    public PhotoViewModel? GetPhotoByDate(DateTime date)
    {
        var photos = _photoRepository.GetAllPhotos();
        var photo = photos.FirstOrDefault(p => p.Date.Date == date.Date);
        if (photo == null) return null;

        var index = photos.IndexOf(photo);
        return new PhotoViewModel
        {
            CurrentPhoto = photo,
            PreviousPhoto = index < photos.Count - 1 ? photos[index + 1] : null,
            NextPhoto = index > 0 ? photos[index - 1] : null,
            Comments = _dbContext.PhotoComments
                .Where(c => c.PhotoDate == photo.Date.ToString("yyyy-MM-dd"))
                .OrderByDescending(c => c.CreatedAt)
                .ToList()
        };
    }

    private static PhotoIndexViewModel BuildIndexViewModel(
        List<PhotoEntry> photos, string? activeTag, List<PhotoEntry>? tagSource = null)
    {
        var byYear = photos
            .GroupBy(p => p.Date.Year)
            .OrderByDescending(g => g.Key)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(p => p.Date).ToList());

        var topTags = (tagSource ?? photos)
            .SelectMany(p => p.Tags)
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .GroupBy(t => t.ToLowerInvariant().Trim())
            .OrderByDescending(g => g.Count())
            .Take(20)
            .Select(g => (Tag: g.Key, Count: g.Count()))
            .ToList();

        return new PhotoIndexViewModel
        {
            PhotosByYear = byYear,
            TopTags = topTags,
            TotalCount = photos.Count,
            Years = byYear.Keys.OrderByDescending(y => y).ToList(),
            ActiveTag = activeTag?.ToLowerInvariant().Trim()
        };
    }
}
