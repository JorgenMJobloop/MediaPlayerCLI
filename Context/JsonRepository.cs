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
            return _lib;
        }

        var json = File.ReadAllText(_path);
        var data = JsonSerializer.Deserialize<MediaLibrary>(json, _options);
        // we return a ternary condition
        // if(data == null) return _lib || if(data != null) return data
        return data ?? _lib;
    }

    public void Save(MediaLibrary lib)
    {
        var jsonData = JsonSerializer.Serialize(lib, _options);
        File.WriteAllText(_path, jsonData);
    }
}