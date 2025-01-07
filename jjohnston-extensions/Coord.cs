namespace org.jjohnston.extensions;

public class Coord : IEquatable<Coord>
{
    public int R { get; set; } = -1;
    public int C { get; set; } = -1;

    public Coord() : this(-1, -1)
    {

    }

    public Coord(int r, int c)
    {
        this.R = r;
        this.C = c;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Coord other)
        {
            return this.Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(R, C);
    }

    public override string ToString()
    {
        return $"({R}, {C})";
    }

    public bool Equals(Coord? other)
    {
        if (other != null)
        {
            return this.R == other.R && this.C == other.C;
        }

        return false;
    }

    public Coord UpFrom()
    {
        return new Coord(this.R - 1, this.C);
    }

    public Coord DownFrom()
    {
        return new Coord(this.R + 1, this.C);
    }

    public Coord LeftFrom()
    {
        return new Coord(this.R, this.C - 1);
    }

    public Coord RightFrom()
    {
        return new Coord(this.R, this.C + 1);
    }
}