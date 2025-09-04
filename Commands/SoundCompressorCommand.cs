using System.CommandLine;
using System.CommandLine.Parsing;
using System.Reflection;
using Fconventer.SoundCompressor;

namespace Fconventer.SoundCompressor
{
    class SoundCompressorCommand
{
    public static Command Build()
    {
        var compress = new Command("c-sound", "Compress audio files");
            var _input = new Argument<string>("input", "Target audio file");
            var _output = new Argument<string>("output", "Output audio file"); //In next versions, I can change here.
            var _bitrate = new Option<int>("--bitrate", () => 128, "Target audio bitrate (e.g. 128k 192k)");

            compress.AddArgument(_input);
            compress.AddArgument(_output);
            compress.AddOption(_bitrate);

            compress.SetHandler<string,string,int>(
                async (string inFile, string outFile, int bitrate) =>
                {
                    var options = new SoundCompressorOptions
                    {
                        bitrate = bitrate,
                        output = outFile,
                        input = inFile
                    };
                    await SoundCompressor.RunAsync(options);
                    Console.WriteLine($"Audio compressed successfully: {options.output}");
                },
                _input, _output, _bitrate
            );

        return compress;
    }
}
}