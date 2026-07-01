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

    public static int2 operator +(int2 v) => v;
    public static int2 operator -(int2 v) => new(-v.x, -v.y);

    public static int2 operator +(int2 lhs, int2 rhs) => new(lhs.x + rhs.x, lhs.y + rhs.y);
    public static int2 operator -(int2 lhs, int2 rhs) => new(lhs.x - rhs.x, rhs.y - rhs.y);

    public static readonly int2 zero = (0, 0);
}
