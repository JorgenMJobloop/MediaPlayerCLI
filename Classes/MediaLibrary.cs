public class MediaLibrary
{
    // scoping & visibility
    // composition
    private readonly List<MediaItemBase> _items = new List<MediaItemBase>();
    private readonly IPlayerService _player;

    public MediaLibrary(IPlayerService player)
    {
        _player = player;
    }

    public void AddItem(MediaItemBase item)
    {
        _items.Add(item); // add an item to our list
    }

    public void RemoveItem(Guid guid)
    {
        var item = _items.FirstOrDefault(id => id.Id == guid);
        if (item != null)
        {
            _items.Remove(item); // remove an item from our list
        }
    }

    public IEnumerable<MediaItemBase> GetAllItems()
    {
        return _items;
    }

    public void Play(Guid guid)
    {
        var item = _items.FirstOrDefault(id => id.Id == guid); // get an item from our _items list, by targeting it's Id
        if (item == null)
        {
            Console.WriteLine("No media file was found!");
            return;
        }

        item.Play(_player);
    }

    public void ScanAndAddItemsToLibrary(string filePath, bool isVideo)
    {
        var title = Path.GetFileNameWithoutExtension(filePath);
        MediaItemBase item = isVideo ? new VideoItem(title, filePath) : new AudioItem(title, filePath);
        AddItem(item);
    }
}