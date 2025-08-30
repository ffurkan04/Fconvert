using System.CommandLine;

var rootCommand = new RootCommand("File Converter");

//Adding Convert Command
rootCommand.Add(ConvertCommand.Build());
await rootCommand.InvokeAsync(args);