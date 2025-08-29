//This is an interface for converters.

public interface IFConverter
{
    bool CanConvert(string inputExtension, string outputExtension);
    void Convert(string inputPath, string outputPath);
}