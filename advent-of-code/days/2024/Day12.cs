using System.Reflection.Metadata;
using System.Security.AccessControl;
using org.jjohnston.aoc.days;
using org.jjohnston.extensions;

namespace org.jjohnston.aoc.year2024;

public class Day12 : AbstractDay
{
    public class Region
    {
        public char Identifier { get; set; } = '.';
        public HashSet<Coord> Coords { get; set; } = new HashSet<Coord>();

        public Coord? TopLeft { get; set; } = null;

        public bool Contains(Coord c)
        {
            return this.Coords.Contains(c);
        }

        public bool Add(Coord c)
        {
            if (this.TopLeft == null)
            {
                TopLeft = c;
            }
            else
            {
                // row first
                if (c.R < TopLeft.R)
                {
                    this.TopLeft = c;
                }
                // else == row, left-most col
                else if (c.R == TopLeft.R)
                {
                    if (c.C < TopLeft.C)
                    {
                        TopLeft = c;
                    }
                }
            }
            return this.Coords.Add(c);
        }

        public int GetArea()
        {
            return this.Coords.Count();
        }

        public int GetPerimeter()
        {
            int p = 0;

            foreach (Coord c in this.Coords)
            {
                if (!this.Coords.Contains(c.UpFrom()))
                {
                    ++p;
                }
                if (!this.Coords.Contains(c.DownFrom()))
                {
                    ++p;
                }
                if (!this.Coords.Contains(c.LeftFrom()))
                {
                    ++p;
                }
                if (!this.Coords.Contains(c.RightFrom()))
                {
                    ++p;
                }
            }

            return p;
        }

        public enum Direction
        {
            Right,
            Up,
            Down,
            Left,
        }

        public int GetNumSides()
        {
            if (this.TopLeft == null
                || this.Coords.Count() == 0)
            {
                return 0;
            }

            // first edge case... there's only 1 plot in this region. There are therefore 4 sides.
            // actually, if there are only 2 plots, since they have to be orthogonally connected, it
            // ALSO only has 4 sides.
            // 2 edge cases done. 
            if (this.Coords.Count() <= 2)
            {
                return 4;
            }

            int s = 0;

            // start at top left
            Coord curPt = this.TopLeft;
            // go right
            Direction curDir = Direction.Right;

            bool madeAFullCircuit = false;

            while (!madeAFullCircuit)
            {
                // go straight along this edge
                ++s;
                bool keepGoingStraight = true;
                while (keepGoingStraight)
                {
                    Coord rt = curPt.RightFrom();
                    Coord up = curPt.UpFrom();
                    Coord dn = curPt.DownFrom();
                    Coord lt = curPt.LeftFrom();

                    if (curDir == Direction.Right)
                    {
                        // go until :
                        //          UP is in the region, which means we took a left-hand turn
                        //          or, DOWN is in the region, and right is not, which means we took a right-hand turn
                        if (this.Contains(up))
                        {
                            curDir = Direction.Up;
                            curPt = up;
                            keepGoingStraight = false;
                        }
                    }
                }
            }

            return s;
        }
    }

    public class FarmPlots
    {
        public char[][] Plots { get; set; }

        public List<Region> Regions { get; set; } = new List<Region>();

        public FarmPlots(string[] inputs)
        {
            this.Plots = new char[inputs.Length][];
            for (int r = 0; r < this.Plots.Length; r++)
            {
                this.Plots[r] = new char[inputs[r].Length];
                for (int c = 0; c < this.Plots[r].Length; c++)
                {
                    this.Plots[r][c] = inputs[r][c];
                }
            }
        }

        public void Print()
        {
            Console.Out.WriteLine("Farm: ");
            for (int r = 0; r < this.Plots.Length; r++)
            {
                for (int c = 0; c < this.Plots[r].Length; c++)
                {
                    Console.Out.Write(this.Plots[r][c]);
                }
                Console.Out.WriteLine();
            }

            Console.Out.WriteLine("Regions: ");
            foreach (Region r in this.Regions)
            {
                Console.Out.WriteLine($"  -- {r.Identifier} size {r.Coords.Count()}");
                Console.Out.WriteLine($"  --  -- TopLeft = {(r.TopLeft != null ? r.TopLeft.ToString() : "null")}");
            }
        }

        public void FindAndGrowRegions()
        {
            for (int r = 0; r < this.Plots.Length; r++)
            {
                for (int c = 0; c < this.Plots[r].Length; c++)
                {
                    char cropType = this.Plots[r][c];
                    Coord thisCoord = new Coord(r, c);
                    // does this plot belong to a region already?
                    Region? belongsTo = null;
                    foreach (Region reg in this.Regions)
                    {
                        if (reg.Contains(thisCoord))
                        {
                            belongsTo = reg;
                            break;
                        }
                    }
                    if (belongsTo == null)
                    {
                        belongsTo = new Region();
                        belongsTo.Identifier = cropType;
                        this.Regions.Add(belongsTo);
                        this.GrowRegion(belongsTo, thisCoord);
                    }
                }
            }
        }

        public char GetPlotType(Coord c)
        {
            return this.Plots[c.R][c.C];
        }

        public bool IsInBounds(Coord c)
        {
            return c.R >= 0 && c.R < this.Plots.Length
                    && c.C >= 0 && c.C < this.Plots[c.R].Length;
        }

        public void GrowRegion(Region reg, Coord start)
        {
            Queue<Coord> candidatePlots = new Queue<Coord>();
            candidatePlots.Enqueue(start);

            while (candidatePlots.Count() > 0)
            {
                Coord cand = candidatePlots.Dequeue();

                if (this.IsInBounds(cand)
                    && this.GetPlotType(cand) == reg.Identifier
                    && !reg.Contains(cand))
                {
                    reg.Add(cand);
                    candidatePlots.Enqueue(cand.UpFrom());
                    candidatePlots.Enqueue(cand.DownFrom());
                    candidatePlots.Enqueue(cand.LeftFrom());
                    candidatePlots.Enqueue(cand.RightFrom());
                }
            }
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        FarmPlots farm = new FarmPlots(inputs);
        // if (debug) farm.Print();

        farm.FindAndGrowRegions();
        if (debug) farm.Print();

        int totalCost = 0;

        foreach (Region reg in farm.Regions)
        {
            int p = reg.GetPerimeter();
            int a = reg.GetArea();
            int cost = a * p;
            totalCost += cost;

            if (debug) Console.Out.WriteLine($"  -- region {reg.Identifier} :: area {a} * perim {p} == {cost}");
        }

        return "total cost = " + totalCost;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        FarmPlots farm = new FarmPlots(inputs);
        // if (debug) farm.Print();

        farm.FindAndGrowRegions();
        if (debug) farm.Print();

        int totalCost = 0;
        foreach (Region reg in farm.Regions)
        {
            int s = reg.GetNumSides();
            int a = reg.GetArea();
            int cost = a * s;
            totalCost += cost;

            if (debug) Console.Out.WriteLine($"  -- region {reg.Identifier} :: area {a} * sides {s} == {cost}");
        }

        return "total cost = " + totalCost;

    }
}
