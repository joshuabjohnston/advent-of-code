using System.Runtime.CompilerServices;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day4 : AbstractDay
{
    public class Deltas
    {
        public int dR {get;set;} = 0;
        public int dC {get;set;} = 0;
        public string Name {get;set;} = "unk";

        public override string ToString()
        {
            return this.Name;
        }
    }
    public Deltas Up = new Deltas() {dR = -1, dC = 0, Name = "Up"};
    public Deltas Down = new Deltas() {dR = 1, dC = 0, Name = "Down"};
    public Deltas Left = new Deltas() {dR = 0, dC = -1, Name = "Left"};
    public Deltas Right = new Deltas() {dR = 0, dC = 1, Name = "Right"};
    public Deltas UpLeft = new Deltas() {dR = -1, dC = -1, Name = "UpLeft"};
    public Deltas UpRight = new Deltas() {dR = -1, dC = 1, Name = "UpRight"};
    public Deltas DownLeft = new Deltas() {dR = 1, dC = -1, Name = "DownLeft"};
    public Deltas DownRight = new Deltas() {dR = 1, dC = 1, Name = "DownRight"};

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int countXmas = 0;

        // starting at each point in the matrix
        for (int r = 0; r < inputs.Length; r++)
        {
            for (int c = 0; c < inputs[r].Length; c++)
            {
                // from here, do we see a full 'XMAS' in any direction?
                if (FindXmasInDirection(inputs, r, c, Up, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, Down, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, Left, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, Right, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, UpLeft, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, UpRight, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, DownLeft, debug)) ++countXmas;
                if (FindXmasInDirection(inputs, r, c, DownRight, debug)) ++countXmas;
            }
        }

        return "XMAS appears == " + countXmas;
    }

    public bool FindXmasInDirection(string[] inputs, int r, int c, Deltas dir, bool debug)
    {
        bool xmasFound = true;

        int curR = r;
        int curC = c;

        string match = "XMAS";

        for (int matchIdx = 0; matchIdx < match.Length; matchIdx++)
        {
            if (curR < 0 || curR >= inputs.Length
                || curC < 0 || curC >= inputs[curR].Length)
            {
                xmasFound = false;
                break;
            }

            if (inputs[curR][curC] == match[matchIdx])
            {
            }
            else
            {
                xmasFound = false;
                break;
            }

            curR += dir.dR;
            curC += dir.dC;
        }

        if (debug && xmasFound) Console.Out.WriteLine($"found from ({r},{c}) in dir {dir.Name}");

        return xmasFound;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int countXmas = 0;

        // starting at each point in the matrix
        for (int r = 0; r < inputs.Length; r++)
        {
            for (int c = 0; c < inputs[r].Length; c++)
            {
                // from here, do we see a full 'XMAS' in any direction?
                if (FindMasInX(inputs, r, c, debug)) ++ countXmas;
            }
        }

        return "X-MAS appears == " + countXmas;
    }

    public bool FindMasInX(string[] inputs, int r, int c, bool debug)
    {
        bool masXFound = true;

        if (r-1 < 0
            || c-1 < 0
            || r+1 >= inputs.Length
            || c+1 >= inputs.Length
            )
        {
            masXFound = false;
        }
        else
        {
            if (inputs[r][c] == 'A')
            {
                bool firstDiag = (inputs[r-1][c-1] == 'M' && inputs [r+1][c+1] == 'S')
                                || (inputs[r-1][c-1] == 'S' && inputs [r+1][c+1] == 'M');
                bool secondDiag = (inputs[r+1][c-1] == 'M' && inputs [r-1][c+1] == 'S')
                                || (inputs[r+1][c-1] == 'S' && inputs [r-1][c+1] == 'M');
                masXFound = firstDiag && secondDiag;
            }
            else
            {
                masXFound = false;
            }
        }

        return masXFound;
    }
}