public class ConvertManager
{
    private readonly List<IFConverter> _converters = new List<IFConverter>(); //Adding all converter class

    public ConvertManager()
    {
        _converters.Add(new CvsToJsonConventer());
        _converters.Add(new ImageConverter());
        _converters.Add(new MP4ToMP3Converter());
        _converters.Add(new TextToPdfConverter());
        //If you want to add new conventer. You should add it to this list.
    }
    public void Convert(string inputPath, string outputPath)
    {
        string inputExtension = Path.GetExtension(inputPath).ToLower();
        string outputExtension = Path.GetExtension(outputPath).ToLower();

        var converter = _converters.FirstOrDefault(c => c.CanConvert(inputExtension, outputExtension));

        if (converter == null)
        {
            Console.WriteLine("This format is not yet supported.");
            return;
        }

        converter.Convert(inputPath, outputPath);
    }
}