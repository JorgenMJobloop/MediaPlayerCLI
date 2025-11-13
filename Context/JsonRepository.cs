using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class JsonRepository : IRepository
{
    /// <summary>
    /// a given filepath to the database file
    /// </summary>
    private readonly string _path;
    private readonly JsonSerializerOptions _options;
    private readonly MediaLibrary _lib;

    public JsonRepository(string path)
    {
        _path = path;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };
        _lib = null!;
    }

    public MediaLibrary Load()
    {
        // check whether or not a file exists.
        if (!File.Exists(_path))
        {
            try
            {
                var json = File.ReadAllText(_path);
                var lib = JsonSerializer.Deserialize<MediaLibrary>(json, _options);
                if(lib != null)
                {
                    Console.WriteLine($"Library successfully loaded from: {_path}");
                    return lib;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error: Could not read the database file: {e.Message}");
            }
        }
        Console.WriteLine("No database found (empty) - scanning the Media/ folder for items.");
        return LibraryLoader.LoadFromFileSystem();
    }

    public void Save(MediaLibrary lib)
    {
        var jsonData = JsonSerializer.Serialize(lib, _options);
        File.WriteAllText(_path, jsonData);
    }
}