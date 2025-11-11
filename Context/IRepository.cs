public interface IRepository
{
    /// <summary>
    /// Uses our MediaLibrary class, and let's the IRepository interface load elements from it
    /// </summary>
    /// <returns></returns>
    MediaLibrary Load();
    /// <summary>
    /// Save changes to our database file
    /// </summary>
    void Save(MediaLibrary lib);
}