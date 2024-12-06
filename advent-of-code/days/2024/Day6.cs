using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day6 : AbstractDay
{
    public class Coord
    {
        public int R { get; set; } = 0;
        public int C { get; set; } = 0;

        public Coord() : this(0, 0)
        {
        }

        public Coord(int nR, int nC)
        {
            this.R = nR;
            this.C = nC;
        }

        public void Move(Deltas d)
        {
            R += d.dR;
            C += d.dC;
        }

        public Coord PeekForward(Deltas d)
        {
            Coord fwd = this.Clone();
            fwd.Move(d);
            return fwd;
        }

        public Coord PeekBackward(Deltas d)
        {
            Coord bkwd = this.Clone();
            bkwd.Move(d.Negate());
            return bkwd;
        }

        public Coord Clone()
        {
            Coord newC = new Coord(this.R, this.C);
            return newC;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Coord other)
            {
                return this.C == other.C && this.R == other.R;
            }

            return false;
        }

        public override string ToString()
        {
            return $"({R}, {C})";
        }

        public static Coord NotInitialized()
        {
            return new Coord(-1, -1);
        }
    }

    public class Deltas
    {
        public int dR { get; set; } = 0;
        public int dC { get; set; } = 0;
        public string Name { get; set; } = "unk";

        public Deltas() : this(0, 0)
        { }

        public Deltas(int nDR, int nDC)
        {
            this.dR = nDR;
            this.dC = nDC;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public Deltas Negate()
        {
            return new Deltas(this.dR * -1, this.dC * -1) { Name = this.Name + " negated" };
        }
    }
    public Deltas Up = new Deltas() { dR = -1, dC = 0, Name = "Up" };
    public Deltas Down = new Deltas() { dR = 1, dC = 0, Name = "Down" };
    public Deltas Left = new Deltas() { dR = 0, dC = -1, Name = "Left" };
    public Deltas Right = new Deltas() { dR = 0, dC = 1, Name = "Right" };

    public class Map
    {
        public char[][] Positions { get; set; }
        public Coord CurrentPosition { get; set; }

        public Map(string[] inputs)
        {
            this.Positions = new char[inputs.Length][];
            CurrentPosition = Coord.NotInitialized();

            for (int r = 0; r < Positions.Length; r++)
            {
                this.Positions[r] = new char[inputs[r].Length];

                for (int c = 0; c < Positions[r].Length; c++)
                {
                    Positions[r][c] = inputs[r][c];

                    if (
                        Positions[r][c] == '^'
                        || Positions[r][c] == '>'
                        || Positions[r][c] == 'v'
                        || Positions[r][c] == '<'
                    )
                    {
                        CurrentPosition = new Coord(r, c);
                    }
                }
            }
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int countTouchedSpaces = 0;

        // find the starting point. This is either ^, <, >, v
        Map map = new Map(inputs);
        if (debug)
        {
            if (map.CurrentPosition.Equals(Coord.NotInitialized()))
            {
                Console.Out.WriteLine("No starting position found");
            }
            else
            {
                Console.Out.WriteLine($"Starting position found at {map.CurrentPosition.ToString()}");
            }
        }


        return "number spaces = " + countTouchedSpaces;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        throw new NotImplementedException();
    }
}