using SkiaSharp;

namespace Website.Services;

// Renders a branded 1200x630 social-share card for blog posts so links shared on
// Mastodon/etc. get a real preview image instead of the same generic default image
// every page currently falls back to.
public class OgImageService
{
    private const int Width = 1200;
    private const int Height = 630;

    private readonly SKTypeface _typeface;

    public OgImageService(IWebHostEnvironment env)
    {
        var fontPath = Path.Combine(env.WebRootPath, "fonts", "Inter-Variable.ttf");
        _typeface = File.Exists(fontPath) ? SKTypeface.FromFile(fontPath) : SKTypeface.Default;
    }

    public byte[] RenderBlogCard(string title, string dateLabel)
    {
        using var surface = SKSurface.Create(new SKImageInfo(Width, Height));
        var canvas = surface.Canvas;

        using var bgPaint = new SKPaint { Color = new SKColor(0x0f, 0x14, 0x1a) };
        canvas.DrawRect(new SKRect(0, 0, Width, Height), bgPaint);

        using var accentPaint = new SKPaint { Color = new SKColor(0x4f, 0x9d, 0xff) };
        canvas.DrawRect(new SKRect(0, 0, Width, 12), accentPaint);

        using var titleFont = new SKFont(_typeface, 64);
        using var titlePaint = new SKPaint { Color = SKColors.White, IsAntialias = true };

        var lines = WrapText(title, titleFont, Width - 160);
        var firstBaseline = Height / 2f - (lines.Count - 1) * 40f;
        var y = firstBaseline;
        foreach (var line in lines)
        {
            canvas.DrawText(line, 80, y, titleFont, titlePaint);
            y += 80;
        }

        using var footerFont = new SKFont(_typeface, 32);
        using var footerPaint = new SKPaint { Color = new SKColor(0x9c, 0xa8, 0xb5), IsAntialias = true };

        canvas.DrawText("clintmcmahon.com", 80, Height - 70, footerFont, footerPaint);
        var dateWidth = footerFont.MeasureText(dateLabel);
        canvas.DrawText(dateLabel, Width - 80 - dateWidth, Height - 70, footerFont, footerPaint);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private static List<string> WrapText(string text, SKFont font, float maxWidth)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lines = new List<string>();
        var current = "";

        foreach (var word in words)
        {
            var candidate = string.IsNullOrEmpty(current) ? word : $"{current} {word}";
            if (font.MeasureText(candidate) > maxWidth && !string.IsNullOrEmpty(current))
            {
                lines.Add(current);
                current = word;
            }
            else
            {
                current = candidate;
            }
        }
        if (!string.IsNullOrEmpty(current))
            lines.Add(current);

        if (lines.Count > 4)
        {
            lines = lines.Take(4).ToList();
            lines[3] = lines[3].TrimEnd() + "…";
        }

        return lines;
    }
}
