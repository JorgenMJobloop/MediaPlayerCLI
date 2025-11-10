public abstract class MediaItemBase
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Title { get; protected set; } = string.Empty;
    public string? FilePath { get; protected set; } = string.Empty;
    public TimeSpan? Duration { get; protected set; }

    protected MediaItemBase(string title, string filePath)
    {
        Title = title;
        FilePath = filePath;
    }

    public abstract void Play(IPlayerService player);

    public override string ToString()
    {
        return $"{Title} ({GetType().Name})";
    }
}