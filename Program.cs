using System.CommandLine;

var rootCommand = new RootCommand("FConverter");
var convertCommand = new Command("fconvert", "convert");

var inputArg = new Argument<string>("input", "source file");
var outputArg = new Argument<string>("output", "target file");
convertCommand.AddArgument(inputArg);
convertCommand.AddArgument(outputArg);

convertCommand.SetHandler((string input, string output) =>
{
    var manager = new ConvertManager();
    manager.Convert(input, output);

}, inputArg, outputArg);

rootCommand.Add(convertCommand);
await rootCommand.InvokeAsync(args);