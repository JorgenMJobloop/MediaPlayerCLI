using Spectre.Console;
public class CLI
{
    private const string DatabasePath = "Context/database.json";
    static JsonRepository repository = new JsonRepository(DatabasePath);
    PlayerService mediaPlayer = new PlayerService();

    public async Task RunCLI()
    {
        var library = repository.Load();

        // here, we create the main CLI loop, using a while(true) infinite loop
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]CLI Media Player[/]")
                    .AddChoices("Play audio", "Play video", "Show library", "Save to library", "Exit")
            );

            switch (choice)
            {
                case "Play audio":
                    await AudioMenu(library, mediaPlayer);
                    break;
                case "Play video":
                    await VideoMenu(library, mediaPlayer);
                    break;
                case "Show library":
                    ShowLibrary(library);
                    break;
                case "Save to library":
                    AddMediaToLibrary(library);
                    repository.Save(library);
                    break;
                case "Exit":
                    return;
            }
        }
    }

    // Helper methods

    /// <summary>
    /// Show the library menu in the CLI/TUI
    /// </summary>
    private static void ShowLibrary(MediaLibrary library)
    {
        var table = new Table().Border(TableBorder.Rounded)
            .AddColumn("Title")
            .AddColumn("Artist")
            .AddColumn("Duration")
            .AddColumn("Type");

        foreach (var media in library.GetAllItems())
        {
            var artist = string.IsNullOrWhiteSpace(media.Artist) ? "Unknown" : media.Artist;
            var duration = media.Duration?.ToString(@"mm\:ss") ?? "?";
            table.AddRow(media.Title!, artist, duration, media.GetType().Name);
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
    }

    private static void AddMediaToLibrary(MediaLibrary library)
    {
        var path = AnsiConsole.Ask<string>("Path to your file:");
        var type = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Pick type:").AddChoices("Audio", "Video")
        );

        bool isVideo = type == "Video";
        library.ScanAndAddItemsToLibrary(path, isVideo);
        AnsiConsole.MarkupLine("[green]Added to your library.[/]");
        Thread.Sleep(800);
    }

    private static async Task AudioMenu(MediaLibrary lib, IPlayerService player)
    {
        var getAudioFiles = lib.GetAllItems().OfType<AudioItem>().ToList();
        if (getAudioFiles.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No audio files was found in your library.[/]");
            return;
        }

        // we create a new selection prompt below
        var selectedItems = AnsiConsole.Prompt(
            new SelectionPrompt<AudioItem>()
                .Title("Choose a [cyan]song[/] to play:")
                .UseConverter(audio => $"{audio.Title} - {audio.Artist} ({audio.Duration?.ToString(@"mm\:ss") ?? "?"})")
                .AddChoices(getAudioFiles)
        );

        AnsiConsole.Status().Start($"Playing [green]{selectedItems.Title}[/]...", context =>
        {
            selectedItems.Play(player);
            DrawSpectrogram();
        });
    }

    private static async Task VideoMenu(MediaLibrary lib, IPlayerService player)
    {
        var getVideoFiles = lib.GetAllItems().OfType<VideoItem>().ToList();
        if (getVideoFiles.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No video files was found in your library..[/]");
            return;
        }

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<VideoItem>()
                .Title("Select a [green]video[/] to play:")
                .UseConverter(videos => $"{videos.Title} ({videos.Duration?.ToString(@"mm\:ss") ?? "?"})")
                .AddChoices(getVideoFiles)
        );

        selected.Play(player);
    }

    // test out a DrawSpectrogram method
    private static void DrawSpectrogram()
    {
        for (int i = 0; i < 15; i++)
        {
            var bars = string.Concat(Enumerable.Range(0, 30)).Select(_ => new string('|', Random.Shared.Next(1, 8)) + " ");

            AnsiConsole.MarkupLine($"[cyan]{bars}[/]");
            Thread.Sleep(150);
        }
    }

}