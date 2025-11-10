public interface IPlayerService
{
    Task PlayAsync(MediaItemBase item, int? volume, CancellationToken ct);
}