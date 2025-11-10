using System.Diagnostics;

public class PlayerService : IPlayerService
{
    /// <summary>
    /// Play a media file, that is given as an argument to this service. The class also utilizes the Linux process FFMpeg & FFPlay
    /// </summary>
    /// <param name="item">A media file .wav .mp3 .mp4 etc</param>
    /// <param name="volume">the volume of the player</param>
    /// <param name="ct">a cancellation token</param>
    /// <returns>Task</returns>
    public async Task PlayAsync(MediaItemBase item, int? volume, CancellationToken ct)
    {
        // declaring our process
        var process = new ProcessStartInfo
        {
            FileName = "ffplay",
            UseShellExecute = false,
            ArgumentList = { "-autoexit" }
        };

        if (item is AudioItem)
        {
            process.ArgumentList.Add("-nodisp");
        }

        if (volume.HasValue)
        {
            process.ArgumentList.Add("-volume");
            process.ArgumentList.Add(volume.Value.ToString());
        }

        process.ArgumentList.Add(item.FilePath!);

        // call the process by using the Process class
        using var proc = Process.Start(process);
        await proc!.WaitForExitAsync();
    }
}