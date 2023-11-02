using SkiaSharp;

namespace PoundPupLegacy.Services.Implementation;

public class ImagingService: IImagingService
{
    public byte[] Resize(Stream stream, int height)
    {
        stream.Position = 0;
        var image = SKBitmap.Decode(stream);
        var width = Convert.ToInt32(Convert.ToDouble(image.Width) / Convert.ToDouble(image.Height) * height);
        using SKBitmap scaledBitmap = image.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
        using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);
        using SKData data = scaledImage.Encode();
        return data.ToArray();
    }
}
