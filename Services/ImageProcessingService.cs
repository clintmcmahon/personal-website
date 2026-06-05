using SkiaSharp;

namespace Website.Services;

public class ImageProcessingService
{
    private const int MaxWidth = 1000;
    private const int JpegQuality = 85;

    public async Task<byte[]> ResizeAsync(Stream inputStream)
    {
        var inputBytes = await ReadAllBytesAsync(inputStream);

        using var original = SKBitmap.Decode(inputBytes);
        if (original == null)
            throw new InvalidOperationException("Could not decode image.");

        SKBitmap bitmap;
        if (original.Width > MaxWidth)
        {
            var targetHeight = (int)((float)original.Height / original.Width * MaxWidth);
            bitmap = original.Resize(new SKImageInfo(MaxWidth, targetHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        }
        else
        {
            bitmap = original;
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, JpegQuality);

        if (bitmap != original)
            bitmap.Dispose();

        return data.ToArray();
    }

    private static async Task<byte[]> ReadAllBytesAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }
}
