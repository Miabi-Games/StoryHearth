using System.Diagnostics.CodeAnalysis;

namespace StoryHearth;

[SuppressMessage("Style", "IDE1006:Naming Styles")]
public record struct int2
{
    public int x;
    public int y;

    public int2(int x, int y) { this.x = x; this.y = y; }

    public void Deconstruct(out int x, out int y) { x = this.x; y = this.y; }

    public static implicit operator int2((int x, int y) v) => new(v.x, v.y);
}
