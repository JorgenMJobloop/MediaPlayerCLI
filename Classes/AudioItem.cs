using System.Threading.Tasks;

public class AudioItem : MediaItemBase
{
    public AudioItem(string title, string filePath) : base(title, filePath)
    {

    }
    // Here we use polymorphism to change the behaviour of our method
    public override void Play(IPlayerService player)
    {
        Console.WriteLine($"Playing audio file: {Title}");
        player.PlayAsync(this, volume: 30, default).Wait();
    }
}