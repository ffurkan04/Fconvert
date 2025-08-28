using PdfSharp.Pdf;
using PdfSharp.Drawing;

public class TextToPdfConverter : IFConvertor
{
    public bool CanConvert(string inputExtension, string outputExtension)
    {
        return inputExtension == ".txt" && outputExtension == ".pdf";
    }
    public void Convert(string inputPath, string outputPath)
    {
        string text = File.ReadAllText(inputPath);
        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Verdana", 12);

        gfx.DrawString(text, font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Default);

        document.Save(outputPath);

        Console.WriteLine($"[OK] {inputPath}=>{outputPath} converted.");

    }
}