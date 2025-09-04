using System.Diagnostics;
namespace Fconventer.SoundCompressor
{
    public sealed class SoundCompressorOptions
    {
        public required string input { get; init; }
        public required string output { get; init; } //For now. I will change output argument in next versions. 
        public int bitrate { get; init; } = 128; //Default value is 128


    }
    public static class SoundCompressor
    {
        public static async Task RunAsync(SoundCompressorOptions options, CancellationToken ct = default)
        {
            if (!File.Exists(options.input))
            {
                Console.WriteLine($"File does not exist: {options.input}");
                return;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(options.output))!);

            var args = $"-i\" {options.input}\" -b:a {options.bitrate}\"{options.output}\"";
            var ProcessStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = new Process { StartInfo = ProcessStartInfo };
            process.Start();

            //Reading output for errors and logs
            var stderr = await process.StandardError.ReadToEndAsync();
            var stdout = await process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"FFmpeg audio compression failed: {stderr}");
            }
        }
    }
}