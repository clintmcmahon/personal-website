using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Website.Models;

namespace Website.Services;

// Current weather for Minneapolis, via Open-Meteo (free, no API key). Cached
// server-side so we're not hitting the API on every page load.
public class WeatherService
{
    private const string CacheKey = "weather:minneapolis";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(20);

    // Minneapolis, MN
    private const double Latitude = 44.9778;
    private const double Longitude = -93.2650;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(IHttpClientFactory httpClientFactory, IMemoryCache cache, ILogger<WeatherService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _logger = logger;
    }

    public async Task<WeatherSnapshot?> GetCurrentAsync()
    {
        if (_cache.TryGetValue(CacheKey, out WeatherSnapshot? cached))
            return cached;

        var result = await FetchAsync();

        // Cache misses too, briefly, so a down API doesn't get hit on every request.
        _cache.Set(CacheKey, result, result != null ? CacheDuration : TimeSpan.FromMinutes(2));
        return result;
    }

    private async Task<WeatherSnapshot?> FetchAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Weather");
            var url = "https://api.open-meteo.com/v1/forecast" +
                      $"?latitude={Latitude}&longitude={Longitude}" +
                      "&current=temperature_2m,weather_code" +
                      "&temperature_unit=fahrenheit" +
                      "&timezone=America%2FChicago";

            using var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (!doc.RootElement.TryGetProperty("current", out var current)) return null;

            var tempF = current.GetProperty("temperature_2m").GetDouble();
            var code = current.GetProperty("weather_code").GetInt32();
            var (description, icon) = WeatherCodeMap.Describe(code);

            return new WeatherSnapshot(Math.Round(tempF), description, icon);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Weather fetch failed");
            return null;
        }
    }
}
