public static class LibraryLoader
{
    private static readonly string[] AudioFileExtensions = [".mp3", ".wav"];
    private static readonly string[] VideoFileExtensions = [".mp4", ".mkv", ".avi"];


    public static MediaLibrary LoadFromFileSystem(string rootPath = "Media/")
    {
        var lib = new MediaLibrary();

        // we can combine paths, by first looking at the root path -- Media/ and then use Path.Combine() to combine the file paths
        var audioPath = Path.Combine(rootPath, "Audio");
        var videoPath = Path.Combine(rootPath, "Video");

        Console.WriteLine($"Scanning files in: {audioPath}");
        var audioFiles = Directory.EnumerateFiles(audioPath, "*.*", SearchOption.AllDirectories).ToList();
        foreach(var f in audioFiles)
        {
            Console.WriteLine($"filename: {f}");
        }

        // with the code lines below, we scan through the directories and look for specific files, that are declared by their extension on line: 3
        foreach(var file in Directory.EnumerateFiles(audioPath, "*.*", SearchOption.AllDirectories).Where(f => AudioFileExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase)))
        {
            var title = Path.GetFileNameWithoutExtension(file);
            var (artist, duration) = MediaMetadataService.GetMetadata(file);
            var audio = new AudioItem(title, file);
            audio.GetType().GetProperty("Artist")?.SetValue(audio, artist ?? "Unknown artist");
            audio.GetType().GetProperty("Duration")?.SetValue(audio, duration);
            lib.AddItem(audio);
        }

        return lib;
    }
}