using System.Drawing;
using System.Drawing.Imaging;
using Crypto6.Stenography;

namespace Crypto6;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal static class Program
{
    private static void Main(string[] args)
    {
        const string soundFile = "YOUR_SOUND_FILE";
        const string soundResultFile = @"YOUR_RESULT_SOUND_FILE";
        const string secretText = "SecretText";

        var bytes = GetBytesFromFile(soundFile);
        var resultBytes = Lsb.HideTextInBytes(bytes, secretText);
        WriteBytesToFile(resultBytes, soundResultFile);

        var result = GetBytesFromFile(soundResultFile);
        var text = Lsb.GetHiddenTextFromBytes(result);

        Console.WriteLine(secretText == text ? "Сообщение верно" : "Сообщение не является верным");
        Console.WriteLine(secretText + "1" == text ? "Сообщение верно" : "Сообщение не является верным");

        Console.WriteLine();

        const string imageFile = "YOUR_IMAGE_FILE";
        const string imageResultFile = "YOUR_RESULT_IMAGE_FILE";
        
        var patchwork = new Patchwork();
        var image = new Bitmap(imageFile);

        patchwork.SetWaterMarkToImage(image);
        image.Save(imageResultFile, ImageFormat.Tiff);

        Console.WriteLine(Patchwork.CheckPictureHasWaterMark(
            new Bitmap(imageFile),
            new Bitmap(imageResultFile),
            2022)
            ? "Имеет водяной знак"
            : "Не имеет водяной знак");

        Console.WriteLine(Patchwork.CheckPictureHasWaterMark(
            new Bitmap(imageResultFile), 2022)
            ? "Имеет водяной знак"
            : "Не имеет водяной знак");
    }

    private static byte[] GetBytesFromFile(string fileName)
    {
        using var file = File.OpenRead(fileName);
        var buffer = new byte[file.Length];
        file.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    private static void WriteBytesToFile(byte[] bytes, string fileName)
    {
        File.Create(fileName).Close();
        File.WriteAllBytes(fileName, bytes);
    }
}