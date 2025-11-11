namespace MediaPlayer;

class Program
{
    static async Task Main(string[] args)
    {
        GuidExample example = new GuidExample();

        example.guid = Guid.NewGuid();
        Console.WriteLine($"Globally Unique Identifier: {example.guid}");

        Console.WriteLine("Running CLI");

        // Current working implementation
        RunProgram();
        // todo: implement a database loader/library loader
        /*
        CLI cli = new CLI();

        await cli.RunCLI();
        */
    }

    /// <summary>
    /// Run our main program seperately, since we are opening up a second process.
    /// </summary>
    static void RunProgram()
    {
        PlayerService player = new PlayerService();
        MediaLibrary library = new MediaLibrary(player);

        // composition - the library owns the player instance object and the media file elements
        library.AddItem(new AudioItem("Daft Punk - Fresh", "Media/Audio/fresh.wav"));

        Console.WriteLine("Playing media file");
        foreach (var media in library.GetAllItems())
        {
            Console.WriteLine($" - {media}");
        }

        Console.WriteLine("Select the ID of the file you want to play (or press Enter to play the first item available in the library...)");
        var idString = Console.ReadLine();
        var id = Guid.TryParse(idString, out var guid) ? guid : library.GetAllItems().First().Id;


        library.Play(id);
    }
}