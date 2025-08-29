using CsvHelper;
using System.Globalization;
using System.Text.Json;
public class CvsToJsonConventer : IFConverter
{
    public bool CanConvert(string inputExtension, string outputExtension)
    {
        return inputExtension == ".csv" && outputExtension == ".json";
    }
    public void Convert(string inputPath, string outputPath)
    {
        using var reader = new StreamReader(inputPath);
        using var cvs = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = cvs.GetRecord<dynamic>();

        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(outputPath, json);

        Console.WriteLine($"[OK] {inputPath}=> {outputPath} converted.");
    }
}