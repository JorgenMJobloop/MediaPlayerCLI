public class AudioItem : MediaItemBase
{
    public AudioItem(string title, string filePath) : base(title, filePath)
    {

    }

    public override void Play(IPlayerService player)
    {
        throw new NotImplementedException();
    }
}