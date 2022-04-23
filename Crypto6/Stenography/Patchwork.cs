using System.Drawing;

namespace Crypto6.Stenography;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class Patchwork
{
    private int Key { get; }
    private const int N = 30000;
    private const int Q = 5;

    public Patchwork()
    {
        Key = 2022;
    }

    public Patchwork(int key)
    {
        Key = key;
    }

    public void SetWaterMarkToImage(Bitmap image)
    {
        var random = new Random(Key);

        for (var i = 0; i < N; i++)
        {
            var (color, x, y) = GetRandomPixelImage(image, random);
            var (color1, i1, y1) = GetRandomPixelImage(image, random);

            image.SetPixel(x, y, GetColorWithChangedBrightness(color, Q));
            image.SetPixel(i1, y1, GetColorWithChangedBrightness(color1, -Q));
        }
    }

    public static bool CheckPictureHasWaterMark(Bitmap originalImage, Bitmap imageWithWaterMark, int key)
    {
        return 2 * N * Q - 10000 <= GetSumOfDifferencesPixelsOfImage(imageWithWaterMark, key) -
            GetSumOfDifferencesPixelsOfImage(originalImage, key);
    }

    public static bool CheckPictureHasWaterMark(Bitmap imageWithWaterMark, int key)
    {
        return 2 * N * Q - N <= GetSumOfDifferencesPixelsOfImage(imageWithWaterMark, key);
    }

    private static double GetSumOfDifferencesPixelsOfImage(Bitmap image, int key)
    {
        double sumOfDifferences = 0;
        var random = new Random(key);

        for (var i = 0; i < N; i++)
        {
            var pixel1 = GetRandomPixelImage(image, random);
            var pixel2 = GetRandomPixelImage(image, random);
            sumOfDifferences += pixel1.color.GetBrightness() * 255 - pixel2.color.GetBrightness() * 255;
        }

        return sumOfDifferences;
    }

    private static (Color color, int x, int y) GetRandomPixelImage(Bitmap image, Random random)
    {
        var x = random.Next(0, image.Width);
        var y = random.Next(0, image.Height);

        return (image.GetPixel(x, y), x, y);
    }

    private static Color GetColorWithChangedBrightness(Color color, int increasingValue)
    {
        return Color.FromArgb(
            Clamp(color.R, increasingValue),
            Clamp(color.G, increasingValue),
            Clamp(color.B, increasingValue));
    }

    private static int Clamp(int value, int increasingValue)
    {
        value += increasingValue;

        return value < 0 ? 0 : value > 255 ? 255 : value;
    }
}