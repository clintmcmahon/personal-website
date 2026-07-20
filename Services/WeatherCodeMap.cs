namespace Website.Services;

// Maps WMO weather codes (as returned by Open-Meteo) to a short description + emoji.
// https://open-meteo.com/en/docs — see "WMO Weather interpretation codes"
public static class WeatherCodeMap
{
    public static (string Description, string Icon) Describe(int code) => code switch
    {
        0 => ("Clear", "☀️"),
        1 => ("Mostly clear", "🌤️"),
        2 => ("Partly cloudy", "⛅"),
        3 => ("Overcast", "☁️"),
        45 or 48 => ("Foggy", "🌫️"),
        51 or 53 or 55 => ("Drizzle", "🌦️"),
        56 or 57 => ("Freezing drizzle", "🌧️"),
        61 or 63 or 65 => ("Rain", "🌧️"),
        66 or 67 => ("Freezing rain", "🌧️"),
        71 or 73 or 75 => ("Snow", "❄️"),
        77 => ("Snow grains", "❄️"),
        80 or 81 or 82 => ("Rain showers", "🌧️"),
        85 or 86 => ("Snow showers", "🌨️"),
        95 => ("Thunderstorm", "⛈️"),
        96 or 99 => ("Thunderstorm w/ hail", "⛈️"),
        _ => ("Weather", "🌡️"),
    };
}
