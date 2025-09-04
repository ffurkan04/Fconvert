using System.Diagnostics;

namespace Fconventer.VideoCrfCompressorCommand
{
    public sealed class VideoCrfOptions
{
    public required string input { get; init; }
    public required string output { get; init; }
    public string Codec { get; init; } = "h265"; //h264 | h265
    public int Crf { get; init; } = 22;
    public string preset { get; init; } = "medium";
    public bool copyAudio { get; init; } = true;
    public int audioKbps { get; init; } = 128;
    public bool DropSubtitles { get; init; } = false;
    public string ffmpeg { get; init; } = "ffmpeg";
}
public static class VideoCrfCompressor
{
    public static async Task<bool> RunAsync(VideoCrfOptions options, CancellationToken ct = default)
    {
        if (!File.Exists(options.input))
        {
            Console.WriteLine($"File does not exist: {options.input}");
            return false;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(options.output))!);

        string vcodec = options.Codec.ToLower() switch
        {
            "h264" => "libx264",
            "h265" => "libx265",
            _ => "libx265"
        };
        //if DropSubtitles is false returns "-sn" and remove subtitles.
        var mapSubs = options.DropSubtitles ? "-sn" : "";
        var audioPart = options.copyAudio ? "-c:a copy" : $"-c:a aac -b:a {options.audioKbps}k";
        var ext = Path.GetExtension(options.output).ToLowerInvariant();
        var mp4Compat = ext == ".mp4" ? "-movflags +faststart -pix_fmt yuv420p" : "";


        var args =
            $"-hide_banner -y -i \"{options.input}\" -map 0 {mapSubs} " +
            $"-c:v {vcodec} -crf {options.Crf} -preset {options.preset} " +
            $"{audioPart} {mp4Compat} " +
            $"\"{options.output}\"";

        var psi = new ProcessStartInfo
        {
            FileName = options.ffmpeg,
            Arguments = args,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        Console.WriteLine($"FFmpeg: {options.ffmpeg} {args}");
        using var p = Process.Start(psi);
        if (p == null) { Console.WriteLine("FFmeg could not start"); return false; }

        _ = Task.Run(async () =>
        {
            while (!p.StandardError.EndOfStream)
            {
                var line = await p.StandardError.ReadLineAsync();
                //Log 
                if (!string.IsNullOrWhiteSpace(line) && (line.Contains("frame=") || line.Contains("bitrate=") || line.Contains("time=")))
                    Console.WriteLine(line);
            }
        }, ct);

        await p.WaitForExitAsync(ct);
        if (p.ExitCode != 0)
        {
            Console.WriteLine($"FFmpeg erorr code: {p.ExitCode}");
            return false;
        }
        try
        {
            var inSize  = new FileInfo(options.input).Length / (1024.0 * 1024.0);
            var outSize = new FileInfo(options.output).Length / (1024.0 * 1024.0);
            Console.WriteLine($"Size: {inSize:F1} MB → {outSize:F1} MB");
        }
        catch { /* pass */ }

        return true;

    }
}
}
