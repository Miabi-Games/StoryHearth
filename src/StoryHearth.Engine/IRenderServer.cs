namespace StoryHearth.Engine;

public interface IRenderServer : IRenderTarget
{
    int2 ScreenSize { get; }
}
