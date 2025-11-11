public abstract class MediaItemBase
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? Title { get; protected set; } = string.Empty;
    public string? FilePath { get; protected set; } = string.Empty;
    public TimeSpan? Duration { get; protected set; }
    public string? Artist { get; protected set; } = string.Empty;

    protected MediaItemBase(string title, string filePath)
    {
        Title = title;
        FilePath = filePath;

        // try to get metadata by using our ffprobe process service class (MediaMetadataService)
        var (artist, duration) = MediaMetadataService.GetMetadata(filePath);
        Artist = artist ?? "Unknown Artist";
        Duration = duration;
    }

    public abstract void Play(IPlayerService player);

    public override string ToString()
    {
        return $"{Title} ({GetType().Name})";
    }
}