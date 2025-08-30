using System.CommandLine;


var rootCommand = new RootCommand("File Conventor & File Compressor");

//Adding compress command
rootCommand.AddCommand(CompressorCommand.Build());