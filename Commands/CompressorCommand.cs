using System.CommandLine;
using System.CommandLine.Invocation;
using System.Security.Cryptography.X509Certificates;
using FFMpegCore.Arguments;
using Fconventer.VideoCrfCompressorCommand;
class CompressorCommand
{
    public static Command Build()
    {
        //This system compress file with CRF. 
        var compress = new Command("c-video", "Compress video files");

        var inputArg = new Argument<string>("input", "Target file (mp4/mkv/avi...)");
        var outputArg = new Argument<string>("output", "Output file (.mp4/.mkv)");

        //This option allows the user to select the video's codec. 
        //Default value is h265
        var codecOpt=new Option<string>(name:"--codec", getDefaultValue:()=>"h265",description:"Video codec: h264 | h265");

        //This option allow the user specify CRF value
        //Default value is 22
        var crfOpt = new Option<int>(name: "--crf", getDefaultValue: () => 22,
        description: "The lower this value is, the better the quality and the larger the file size. Recommended value range for h264 between 18-23, for h265 between 20-28");

        //This option allow the user change preset options
        //Default value is medium
        var presetOpt = new Option<string>(name: "--preset",getDefaultValue:()=>"medium",
        description:"ultrafast, superfast, veryfast, faster, fast, medium, slow, slower, veryslow");

        //This option allow the user to recode the audio.
        //Default value is true 
        var copyAudio = new Option<bool>(name: "--copy-audio", getDefaultValue: () => true,
        description: "Recode the audio (true=copy).");

        //This option allow the user to change kbps value
        //Default value is 128
        var audioKbps = new Option<int>(name: "--ab",getDefaultValue:()=>128,
        description:"If you are re-encoding the audio to AAC, use kbps (e.g. 128).");

        //This option allow the user to remove subtitles
        //Default value is false
        var noSubsOpt = new Option<bool>(name: "--no-subs",getDefaultValue: ()=>false,
        description: "Removes subtitles (true = removes subtitles)");

        //This option allow the user to specify FFmpeg path.
        var ffmpegOpt = new Option<string>(name: "--ffmpeg",
        description: "FFmpeg path (if empty, “ffmpeg” in System PATH is used).");
        
        //Adding Arguments
        compress.AddArgument(inputArg);
        compress.AddArgument(outputArg);
        //Adding Options
        compress.AddOption(codecOpt);
        compress.AddOption(crfOpt);
        compress.AddOption(presetOpt);
        compress.AddOption(copyAudio);
        compress.AddOption(audioKbps);
        compress.AddOption(noSubsOpt);
        compress.AddOption(ffmpegOpt);

        compress.SetHandler(async (InvocationContext ctx) =>
        {
            var input  = ctx.ParseResult.GetValueForArgument(inputArg);
            var output = ctx.ParseResult.GetValueForArgument(outputArg);
            var options = new VideoCrfOptions
            {
                input = input,
                output = output,
                Codec = ctx.ParseResult.GetValueForOption(codecOpt) ?? "libx264",
                Crf = ctx.ParseResult.GetValueForOption(crfOpt),
                preset = ctx.ParseResult.GetValueForOption(presetOpt)?? "medium",
                copyAudio = ctx.ParseResult.GetValueForOption(copyAudio),
                audioKbps = ctx.ParseResult.GetValueForOption(audioKbps),
                DropSubtitles = ctx.ParseResult.GetValueForOption(noSubsOpt),
                ffmpeg = string.IsNullOrWhiteSpace(ctx.ParseResult.GetValueForOption(ffmpegOpt))
                    ? "ffmpeg"
                    : ctx.ParseResult.GetValueForOption(ffmpegOpt)!

            };

            var ok=await VideoCrfCompressor.RunAsync(options);

            Console.WriteLine(ok ? "File compressed" : "Compression failed");
        });


        return compress;
    }
}