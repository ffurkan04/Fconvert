using FFMpegCore;
public class MP4ToMP3Converter : IFConvertor
{
    public bool CanConvert(string inputExtension, string outputExtension)
    {
        return inputExtension==".mp4"&&outputExtension==".mp3"; //For now
    }
    public void Convert(string inputPath, string outputPath)
    {
        FFMpeg.ExtractAudio(inputPath, outputPath);
        Console.WriteLine($"[OK] {inputPath}=>{outputPath} converted.");
    }
}