using System.CommandLine;


var rootCommand = new RootCommand("File Conventor & File Compressor");

//Addin compress command
rootCommand.AddCommand(CompressorCommand.Build());