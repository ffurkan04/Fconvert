using System.CommandLine;

class ConvertCommand
{
    public static Command Build()
    {
        var convertCommand = new Command("convert", "Convert files from one format to another");

        var inputArg = new Argument<string>("input", "Source file");
        var outputArg = new Argument<string>("output", "Target file");
        convertCommand.AddArgument(inputArg);
        convertCommand.AddArgument(outputArg);

        convertCommand.SetHandler((string input, string output) =>
        {
            var manager = new ConvertManager();
            manager.Convert(input, output);

        }, inputArg, outputArg);
        return convertCommand;
    }
}