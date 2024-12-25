using System.Reflection.PortableExecutable;
using System.Runtime.ConstrainedExecution;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day10 : AbstractDay
{
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

    public class TrailHead
    {
        public Coord Start { get; set; }
        public int Score { get; set; } = -1;

        public TrailHead(int r, int c)
        {
            this.Start = new Coord(r, c);
        }
    }

    public class TopoMap
    {
        public int[][] Positions { get; set; }

        public List<TrailHead> TrailHeads { get; set; } = new List<TrailHead>();

        public TopoMap(string[] inputs)
        {
            this.Positions = new int[inputs.Length][];

            for (int r = 0; r < this.Positions.Length; r++)
            {
                this.Positions[r] = new int[inputs[r].Length];
                for (int c = 0; c < this.Positions[r].Length; c++)
                {
                    char ch = inputs[r][c];
                    int val = -1;
                    if (ch != '.')
                    {
                        val = ch - '0';
                    }
                    this.Positions[r][c] = val;

                    if (val == 0)
                    {
                        this.TrailHeads.Add(new TrailHead(r, c));
                    }
                }
            }
        }

        public void PrintMap()
        {
            for (int r = 0; r < this.Positions.Length; r++)
            {
                for (int c = 0; c < this.Positions[r].Length; c++)
                {
                    Console.Out.Write(Positions[r][c] < 0 ? "." : Positions[r][c]);
                }
                Console.Out.WriteLine();
            }
        }

        public int ValueAt(int r, int c)
        {
            return this.Positions[r][c];
        }

        public int ValueAt(Coord c)
        {
            return this.ValueAt(c.R, c.C);
        }

        public bool IsInBounds(Coord c)
        {
            return c.R >= 0 && c.R < this.Positions.Length
                && c.C >= 0 && c.C < this.Positions[0].Length;
        }

        public int ScoreAllTrailheads(bool debug = false)
        {
            int sumScores = 0;
            foreach (TrailHead th in this.TrailHeads)
            {
                if (th.Score < 0)
                {
                    th.Score = this.ScoreTrailhead(th, debug);
                }
                sumScores += th.Score;
            }
            return sumScores;
        }

        public int RateAllTrailheads(bool debug = false)
        {
            int sumScores = 0;
            foreach (TrailHead th in this.TrailHeads)
            {
                if (th.Score < 0)
                {
                    th.Score = this.RateTrailhead(th, debug);
                }
                sumScores += th.Score;
            }
            return sumScores;
        }

        public int ScoreTrailhead(TrailHead th, bool debug = false)
        {
            HashSet<Coord> ninesFromTh = new HashSet<Coord>();

            Queue<Coord> paths = new Queue<Coord>();
            paths.Enqueue(th.Start);
            if (debug) Console.Out.WriteLine($"  --  -- starting from {th.Start}");

            // find this digit, breadth-first, starting from the trailhead
            while (paths.Count > 0)
            {
                Coord curC = paths.Dequeue();
                int curVal = this.ValueAt(curC);
                if (curVal == 9)
                {
                    ninesFromTh.Add(curC);
                }
                else
                {
                    int target = curVal + 1;

                    // up
                    Coord cUp = curC.UpFrom();
                    if (this.IsInBounds(cUp) && this.ValueAt(cUp) == target)
                    {
                        paths.Enqueue(cUp);
                        if (debug) Console.Out.WriteLine($"  --  -- up to {target} at {cUp}");
                    }

                    // down
                    Coord cDown = curC.DownFrom();
                    if (this.IsInBounds(cDown) && this.ValueAt(cDown) == target)
                    {
                        paths.Enqueue(cDown);
                        if (debug) Console.Out.WriteLine($"  --  -- down to {target} at {cDown}");
                    }

                    // left
                    Coord cLeft = curC.LeftFrom();
                    if (this.IsInBounds(cLeft) && this.ValueAt(cLeft) == target)
                    {
                        paths.Enqueue(cLeft);
                        if (debug) Console.Out.WriteLine($"  --  -- left to {target} at {cLeft}");
                    }

                    // right
                    Coord cRight = curC.RightFrom();
                    if (this.IsInBounds(cRight) && this.ValueAt(cRight) == target)
                    {
                        paths.Enqueue(cRight);
                        if (debug) Console.Out.WriteLine($"  --  -- right to {target} at {cRight}");
                    }
                }
            }

            return ninesFromTh.Count();
        }

        public int RateTrailhead(TrailHead th, bool debug = false)
        {
            List<Coord> ninesFromTh = new List<Coord>();

            Queue<Coord> paths = new Queue<Coord>();
            paths.Enqueue(th.Start);
            if (debug) Console.Out.WriteLine($"  --  -- starting from {th.Start}");

            // find this digit, breadth-first, starting from the trailhead
            while (paths.Count > 0)
            {
                Coord curC = paths.Dequeue();
                int curVal = this.ValueAt(curC);
                if (curVal == 9)
                {
                    ninesFromTh.Add(curC);
                }
                else
                {
                    int target = curVal + 1;

                    // up
                    Coord cUp = curC.UpFrom();
                    if (this.IsInBounds(cUp) && this.ValueAt(cUp) == target)
                    {
                        paths.Enqueue(cUp);
                        if (debug) Console.Out.WriteLine($"  --  -- up to {target} at {cUp}");
                    }

                    // down
                    Coord cDown = curC.DownFrom();
                    if (this.IsInBounds(cDown) && this.ValueAt(cDown) == target)
                    {
                        paths.Enqueue(cDown);
                        if (debug) Console.Out.WriteLine($"  --  -- down to {target} at {cDown}");
                    }

                    // left
                    Coord cLeft = curC.LeftFrom();
                    if (this.IsInBounds(cLeft) && this.ValueAt(cLeft) == target)
                    {
                        paths.Enqueue(cLeft);
                        if (debug) Console.Out.WriteLine($"  --  -- left to {target} at {cLeft}");
                    }

                    // right
                    Coord cRight = curC.RightFrom();
                    if (this.IsInBounds(cRight) && this.ValueAt(cRight) == target)
                    {
                        paths.Enqueue(cRight);
                        if (debug) Console.Out.WriteLine($"  --  -- right to {target} at {cRight}");
                    }
                }
            }

            return ninesFromTh.Count();
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int sumTrailheadScores = 0;

        TopoMap topoMap = new TopoMap(inputs);

        if (debug) topoMap.PrintMap();
        sumTrailheadScores = topoMap.ScoreAllTrailheads();

        if (debug)
        {
            foreach (TrailHead th in topoMap.TrailHeads)
            {
                Console.Out.WriteLine($"  -- Trailhead at {th.Start}, score of {th.Score}");
            }
        }

        return "sum trailhead scores = " + sumTrailheadScores;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int sumTrailheadScores = 0;

        TopoMap topoMap = new TopoMap(inputs);

        if (debug) topoMap.PrintMap();
        sumTrailheadScores = topoMap.RateAllTrailheads(debug);

        if (debug)
        {
            foreach (TrailHead th in topoMap.TrailHeads)
            {
                Console.Out.WriteLine($"  -- Trailhead at {th.Start}, score of {th.Score}");
            }
        }

        return "sum trailhead scores = " + sumTrailheadScores;
    }
}