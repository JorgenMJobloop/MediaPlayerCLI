public class VideoItem : MediaItemBase
{
    public VideoItem(string title, string filePath) : base(title, filePath)
    {
    }

    public override void Play(IPlayerService player)
    {
        Console.WriteLine($"Playing video file: {Title}");
        player.PlayAsync(this, volume: null, default).Wait();
    }
}