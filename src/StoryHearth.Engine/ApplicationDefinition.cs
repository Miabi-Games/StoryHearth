namespace StoryHearth.Engine;

/// <summary>
///     Hard-coded application settings that define how the platform runs the
///     application, before configuration files and command-line arguments are
///     considered. These are the defaults.
/// </summary>
/// <remarks>
///     <para>
///         Applications should derive from this class to set their own defaults
///         for the properties that matter to them (e.g., the window title).
///     </para>
///     <para>
///         The deafult values returned by these properties are not part of the
///         API's contract and are likely to change as future needs dictate. If
///         the specific values matter, then these properties should be
///         overriden by the application.
///     </para>
/// </remarks>
public abstract class ApplicationDefinition
{
    public static string DefaultWindowTitle => "A StoryHearth Game";

    public virtual int2 CanvasTargetSize => new(1920, 1080);
    public virtual int2 ScreenMinSize => new(320, 180);
    public virtual string WindowTitle => DefaultWindowTitle;
}
