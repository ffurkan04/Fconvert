using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
//I am using these libaries for cross-platform support. 
public class ImageConverter : IFConverter
{
    public bool CanConvert(string inputExtension, string outputExtension)
    {
        return inputExtension == ".png" && outputExtension == ".jpg" ||
        inputExtension==".jpg"&&outputExtension==".png"; //We are checking file extentions
    }
    public void Convert(string inputPath, string outputPath)
    {
        using var img = Image.Load(inputPath);
        if (outputPath.EndsWith(".jpg"))
        {
            img.Save(outputPath, new JpegEncoder());
        }
        else if (outputPath.EndsWith(".png"))
        {
            img.Save(outputPath, new PngEncoder());
        }
            Console.WriteLine($"[OK] {inputPath}=>{outputPath} converted");
    }
}