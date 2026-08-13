using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Controllers;

// The photoblog moved to its own app at photos.clintmcmahon.com, where every *page* sits at
// the domain root. These 301s keep the old clintmcmahon.com/photos/* URLs alive: bookmarks,
// old feed entries, Mastodon posts, and anything already indexed.
//
// Two shapes used to live under /photos on this site, and they move differently:
//
//   Pages, which lost the prefix:
//     /photos                  -> https://photos.clintmcmahon.com/
//     /photos/archive          -> https://photos.clintmcmahon.com/archive
//     /photos/2026-08-10       -> https://photos.clintmcmahon.com/2026-08-10
//     /photos/tag/minneapolis  -> https://photos.clintmcmahon.com/tag/minneapolis
//
//   Image files under wwwroot/photos/{year}/…, which kept it, because the directory moved
//   across unchanged and the URLs are baked into photos.db:
//     /photos/2026/2026-08-10/img.jpeg -> https://photos.clintmcmahon.com/photos/2026/2026-08-10/img.jpeg
//
// A bare four-digit first segment is what separates them. Photo permalinks are full dates
// ("2026-08-10"), so they never collide with a year directory ("2026").
public class PhotoRedirectController : Controller
{
    [HttpGet("/photos")]
    [HttpGet("/photos/{**rest}")]
    public IActionResult ToPhotoblog(string? rest)
    {
        if (string.IsNullOrWhiteSpace(rest))
            return RedirectPermanent($"{CanonicalUrlHelper.PhotoBaseUrl}/{Request.QueryString}");

        var firstSegment = rest.Split('/', 2)[0];
        var isImagePath = firstSegment.Length == 4 && firstSegment.All(char.IsAsciiDigit);

        var target = isImagePath
            ? $"{CanonicalUrlHelper.PhotoBaseUrl}/photos/{rest}"
            : $"{CanonicalUrlHelper.PhotoBaseUrl}/{rest}";

        return RedirectPermanent(target + Request.QueryString);
    }
}
