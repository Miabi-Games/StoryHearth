namespace StoryHearth.Engine;

public record class ApplicationSettings
{
    // thse properties will eventually come from the application's main data package
    public int2 CanvasTargetSize { get; } = (1920, 1080);
    public int2 WindowMinSize { get; } = (320, 180);
    public string WindowTitle { get; } = "A StoryHearth Game";
}
