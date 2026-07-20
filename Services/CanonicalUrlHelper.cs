namespace Website.Services;

public static class CanonicalUrlHelper
{
    public const string BlogBaseUrl = "https://clintmcmahon.com";
    public const string PhotoBaseUrl = "https://photos.clintmcmahon.com";

    public static string BlogPost(string slug) => $"{BlogBaseUrl}/blog/{slug}";

    public static string Photo(DateTime date) => $"{PhotoBaseUrl}/{date:yyyy-MM-dd}";
}
