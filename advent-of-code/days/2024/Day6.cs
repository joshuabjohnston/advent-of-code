using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;
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

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
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
        public enum Dirs
        {
            Up,
            Right,
            Down,
            Left
        };
        public Dirs Direction { get; set; } = Dirs.Up;

        public Deltas() : this(0, 0)
        { }

        public Deltas(int nDR, int nDC)
        {
            this.dR = nDR;
            this.dC = nDC;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Deltas d)
            {
                return this.dR == d.dR && this.dC == d.dC;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return this.Direction.ToString();
        }

        public Deltas Negate()
        {
            Dirs newDir = Dirs.Up;
            if (this.Direction == Dirs.Up) newDir = Dirs.Down;
            else if (this.Direction == Dirs.Left) newDir = Dirs.Right;
            else if (this.Direction == Dirs.Right) newDir = Dirs.Left;
            return new Deltas(this.dR * -1, this.dC * -1) { Direction = newDir };
        }

        public Deltas TurnRight()
        {
            if (this.Direction == Dirs.Up)
            {
                return Deltas.Right();
            }
            if (this.Direction == Dirs.Right)
            {
                return Deltas.Down();
            }
            if (this.Direction == Dirs.Down)
            {
                return Deltas.Left();
            }
            return Deltas.Up();

        }

        public Deltas Clone()
        {
            return new Deltas(this.dR, this.dC) { Direction = this.Direction };
        }

        public static Deltas Up()
        {
            return new Deltas() { dR = -1, dC = 0, Direction = Dirs.Up };
        }

        public static Deltas Down()
        {
            return new Deltas() { dR = 1, dC = 0, Direction = Dirs.Down };
        }

        public static Deltas Left()
        {
            return new Deltas() { dR = 0, dC = -1, Direction = Dirs.Left };
        }

        public static Deltas Right()
        {
            return new Deltas() { dR = 0, dC = 1, Direction = Dirs.Right };
        }
    }

    public class Map
    {
        public char[][] Positions { get; set; }
        public List<Deltas>[][] PreviousDirections { get; set; }
        public List<Coord> ListOfLoopObstructions { get; set; } = new();
        public Coord CurrentPosition { get; set; }
        public Deltas CurrentDirection { get; set; }

        public Map(string[] inputs)
        {
            this.Positions = new char[inputs.Length][];
            CurrentPosition = Coord.NotInitialized();
            this.CurrentDirection = Deltas.Up();
            this.PreviousDirections = new List<Deltas>[inputs.Length][];

            for (int r = 0; r < Positions.Length; r++)
            {
                this.Positions[r] = new char[inputs[r].Length];
                this.PreviousDirections[r] = new List<Deltas>[inputs[r].Length];

                for (int c = 0; c < Positions[r].Length; c++)
                {
                    Positions[r][c] = inputs[r][c];
                    this.PreviousDirections[r][c] = new List<Deltas>();

                    if (Positions[r][c] == '^')
                    {
                        CurrentPosition = new Coord(r, c);
                        CurrentDirection = Deltas.Up();
                    }
                    else if (Positions[r][c] == '>')
                    {
                        CurrentPosition = new Coord(r, c);
                        CurrentDirection = Deltas.Right();
                    }
                    else if (Positions[r][c] == 'v')
                    {
                        CurrentPosition = new Coord(r, c);
                        CurrentDirection = Deltas.Down();
                    }
                    else if (Positions[r][c] == '<')
                    {
                        CurrentPosition = new Coord(r, c);
                        CurrentDirection = Deltas.Left();
                    }
                }
            }
        }

        public bool IsOnMap(Coord c)
        {
            return c.R >= 0 && c.R < Positions.Length
                    && c.C >= 0 && c.C < Positions[c.R].Length;
        }

        public char GetCurrentPosition()
        {
            return this.GetPosition(this.CurrentPosition);
        }

        public char GetPosition(Coord c)
        {
            return Positions[c.R][c.C];
        }

        public void SetCurrentPosition(char c)
        {
            this.Positions[this.CurrentPosition.R][this.CurrentPosition.C] = c;
        }

        public void RecordThisPreviousDirection()
        {
            // lol
            this.PreviousDirections[this.CurrentPosition.R][this.CurrentPosition.C].Add(this.CurrentDirection);
        }

        public bool IsWallInFront(Coord c, Deltas d)
        {
            bool bIsWall = false;

            Coord inFront = c.PeekForward(d);
            if (this.IsOnMap(inFront))
            {
                char ch = this.GetPosition(inFront);
                if (ch == '#')
                {
                    bIsWall = true;
                }
            }

            return bIsWall;
        }

        public bool TryTakeStep(bool testLoops, out bool resultsInLoop, bool debug)
        {
            resultsInLoop = false;

            // is the space in front of us a wall? '#'
            bool bWallInFront = this.IsWallInFront(this.CurrentPosition, this.CurrentDirection);
            if (bWallInFront)
            {
                // turn right 
                this.CurrentDirection = this.CurrentDirection.TurnRight();
            }
            // put a 1 under us. Take a step forward.
            this.SetCurrentPosition('1');
            this.RecordThisPreviousDirection();
            this.CurrentPosition.Move(this.CurrentDirection);

            // are we still on the map?
            bool stillOnMap = this.IsOnMap(this.CurrentPosition);

            // is this a loop?
            if (testLoops) resultsInLoop = DoesTurningRightResultInLoop(debug);

            return stillOnMap;
        }

        public bool DoesTurningRightResultInLoop(bool debug)
        {
            bool resultsInLoop = false;

            if (debug) Console.Out.WriteLine($" -- checking loop on right from {this.CurrentPosition} in dir {this.CurrentDirection}");

            Deltas loopingDir = this.CurrentDirection.TurnRight();
            Coord loopingCoord = this.CurrentPosition.Clone();
            bool stillOnMap = this.IsOnMap(loopingCoord);

            if (stillOnMap
                && this.PreviousDirections[loopingCoord.R][loopingCoord.C].Contains(loopingDir))
            {
                resultsInLoop = true;
            }

            List<Deltas> dirsOnHypotheticalLoop = new List<Deltas>();
            List<Coord> pointsOnHypotheticalLoop = new List<Coord>();
            pointsOnHypotheticalLoop.Add(loopingCoord.Clone());
            dirsOnHypotheticalLoop.Add(loopingDir.Clone());

            // if we turn right, and if we walk forward, 
            // do we ever hit a space that has previously gone in this same direction?
            // If we do, we know we will loop at some point, because we're going in a direction we previously went,
            // which is how we got to this spot in the first place. 
            // TIME PARADOX
            while (stillOnMap && !resultsInLoop) // if we see a loop, abort. if we're off the edge of the map, abort
            {
                // are we looking at a wall?
                bool bHitAWall = this.IsWallInFront(loopingCoord, loopingDir);
                // TURN right if wall... we might eventually get back to a previous path
                if (bHitAWall)
                {
                    loopingDir = loopingDir.TurnRight();
                    // return false;
                }

                // hypothetical step forward
                loopingCoord.Move(loopingDir);
                stillOnMap = this.IsOnMap(loopingCoord);

                dirsOnHypotheticalLoop.Add(loopingDir.Clone());
                pointsOnHypotheticalLoop.Add(loopingCoord.Clone());

                // have we ever been in this direction at this position?
                if (stillOnMap
                    && this.PreviousDirections[loopingCoord.R][loopingCoord.C].Contains(loopingDir))
                {
                    resultsInLoop = true;
                }

                // OR, have we ever been at this point, in this direction, on the hypothetical loop
                // but not the last one, because that's where we are now. ofc we are where we are NOW.
                for (int i = 0; i < pointsOnHypotheticalLoop.Count() - 1; i++)
                {
                    if (pointsOnHypotheticalLoop[i].Equals(loopingCoord)
                        && dirsOnHypotheticalLoop[i].Equals(loopingDir))
                    {
                        if (debug) Console.Out.WriteLine($" -- hypothetical match on i = {i}");
                        resultsInLoop = true;
                        break;
                    }
                }
            }

            if (resultsInLoop)
            {
                if (debug) Console.Out.WriteLine($"Loop found from coord {this.CurrentPosition} while heading {this.CurrentDirection}");
                Coord cInFront = this.CurrentPosition.PeekForward(this.CurrentDirection);
                if (!ListOfLoopObstructions.Contains(cInFront))
                {
                    ListOfLoopObstructions.Add(cInFront);
                }
            }

            return resultsInLoop;
        }

        public void PrintMap()
        {
            for (int r = 0; r < Positions.Length; r++)
            {
                for (int c = 0; c < Positions[r].Length; c++)
                {
                    Console.Out.Write(Positions[r][c]);
                }
                Console.Out.WriteLine();
            }
        }

        public int CountOccurrences(char ch)
        {
            int occ = 0;
            for (int r = 0; r < Positions.Length; r++)
            {
                for (int c = 0; c < Positions[r].Length; c++)
                {
                    if (this.Positions[r][c] == ch) ++occ;
                }
            }
            return occ;
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
                Console.Out.WriteLine($"Starting direction is {map.CurrentDirection.Direction}");
            }
        }

        // take steps until we're off the map
        bool loop = false;
        while (map.TryTakeStep(false, out loop, debug))
        {
            // going. yay.
        }
        // map.PrintMap();

        // how many 1's are there?
        countTouchedSpaces = map.CountOccurrences('1');

        return "number spaces = " + countTouchedSpaces;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
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
                Console.Out.WriteLine($"Starting direction is {map.CurrentDirection.Direction}");
            }
        }

        // take steps until we're off the map
        bool bLoop = false;
        int numLoops = 0;
        while (map.TryTakeStep(true, out bLoop, debug))
        {
            // going. yay.
            if (bLoop)
            {
                ++numLoops;
            }
        }
        if (debug) map.PrintMap();

        // how many 1's are there?
        // countTouchedSpaces = map.CountOccurrences('1');

        int numObstructions = map.ListOfLoopObstructions.Count();

        return "number loops = " + numLoops + ", unique obstructions = " + numObstructions;
    }
}