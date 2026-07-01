using System.Diagnostics.CodeAnalysis;

namespace StoryHearth;

[SuppressMessage("Style", "IDE1006:Naming Styles")]

public struct int2box
{
    public int2 min; // inclusive bound, unless same as max
    public int2 size;

    public int2 max // exclusive bound
    {
        get => min + size;
        set => size = value - min;
    }
}
