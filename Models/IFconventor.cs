//This is an interface for converters.

public interface IFConvencor
{
    bool CanConvert(string inputExtension, string outputExtension);
    void Convert(string inputPath, string outputPath);
}