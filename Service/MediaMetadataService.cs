using System.Diagnostics;
using System.Text.Json;
public static class MediaMetadataService
{
    // utilize the Process class in a tuple method
    public static (string? Artist, TimeSpan? Duration) GetMetadata(string filePath)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "ffprobe",
            ArgumentList = { "-v", "quiet", "-print_format", "json", "-show_format", "-show_streams", filePath },
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(psi);
        var json = process!.StandardOutput.ReadToEnd();
        process.WaitForExit();

        try
        {
            var document = JsonDocument.Parse(json);
            var format = document.RootElement.GetProperty("format");

            string? artist = null;
            TimeSpan? duration = null;
            // we have to specify that we doing a "try" operation, by using the TryGetProperty rather than the naivé GetProperty, better error handling
            if (format.TryGetProperty("tags", out var tags) && tags.TryGetProperty("artist", out var artistProp))
            {
                artist = artistProp.GetString();
            }
            if (format.TryGetProperty("duration", out var dur))
            {
                if (double.TryParse(dur.GetString(), out var seconds))
                {
                    duration = TimeSpan.FromSeconds(seconds);
                }
            }
            return (artist, duration);
        }
        catch
        {
            return (null, null);
        }
    }
}