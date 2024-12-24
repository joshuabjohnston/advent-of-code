using System.Net;
using System.Security.Cryptography;
using org.jjohnston.aoc.days;
using org.jjohnston.extensions;

namespace org.jjohnston.aoc.year2024;

public class Day8 : AbstractDay
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
    }

    public class AntennaMap
    {
        public int MaxR { get; set; } = -1;
        public int MaxC { get; set; } = -1;

        public Dictionary<char, List<Coord>> AntennaLocations { get; set; } = new();

        public void AddAntenna(char ant, int r, int c)
        {
            if (!AntennaLocations.ContainsKey(ant))
            {
                List<Coord> coords = new List<Coord>();
                AntennaLocations.Add(ant, coords);
            }

            AntennaLocations[ant].Add(new Coord(r, c));
        }

        public bool IsInBounds(Coord c)
        {
            return c.R >= 0 && c.R < MaxR && c.C >= 0 && c.C < MaxC;
        }

        public List<Pair<Coord>> GetAllPairsAnntennae(char antKey)
        {
            List<Pair<Coord>> allPairs = new List<Pair<Coord>>();

            if (AntennaLocations.ContainsKey(antKey))
            {
                List<Coord> antennas = AntennaLocations[antKey];

                for (int i = 0; i < antennas.Count() - 1; i++)
                {
                    Coord thisAnt = antennas[i];
                    for (int j = i + 1; j < antennas.Count(); j++)
                    {
                        Coord otherAnt = antennas[j];
                        allPairs.Add(new Pair<Coord>(thisAnt, otherAnt));
                    }
                }
            }

            return allPairs;
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int numAntinodes = 0;

        AntennaMap antennaMap = new AntennaMap();
        antennaMap.MaxR = inputs.Length;
        antennaMap.MaxC = inputs[0].Length;

        for (int r = 0; r < inputs.Length; r++)
        {
            for (int c = 0; c < inputs[r].Length; c++)
            {
                if (inputs[r][c] != '.')
                {
                    char ant = inputs[r][c];
                    antennaMap.AddAntenna(ant, r, c);
                    if (debug) Console.Out.WriteLine($" -- {ant} at ({r}, {c})");
                }
            }
        }

        HashSet<Coord> antinodeLocations = new HashSet<Coord>();
        foreach (char ant in antennaMap.AntennaLocations.Keys)
        {
            List<Pair<Coord>> allAntPairs = antennaMap.GetAllPairsAnntennae(ant);
            foreach (Pair<Coord> pair in allAntPairs)
            {
                int dr = pair.B.R - pair.A.R;
                int dc = pair.B.C - pair.A.C;

                Coord ant1 = new Coord(pair.A.R - dr, pair.A.C - dc);
                Coord ant2 = new Coord(pair.B.R + dr, pair.B.C + dc);

                if (antennaMap.IsInBounds(ant1)) antinodeLocations.Add(ant1);
                if (antennaMap.IsInBounds(ant2)) antinodeLocations.Add(ant2);
            }
        }

        numAntinodes = antinodeLocations.Count();

        return "number of antinodes = " + numAntinodes;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int numAntinodes = 0;

        AntennaMap antennaMap = new AntennaMap();
        antennaMap.MaxR = inputs.Length;
        antennaMap.MaxC = inputs[0].Length;

        char[][] antinodesDebug = new char[inputs.Length][];

        for (int r = 0; r < inputs.Length; r++)
        {
            antinodesDebug[r] = new char[inputs[r].Length];
            for (int c = 0; c < inputs[r].Length; c++)
            {
                antinodesDebug[r][c] = inputs[r][c];
                if (inputs[r][c] != '.')
                {
                    char ant = inputs[r][c];
                    antennaMap.AddAntenna(ant, r, c);
                    if (debug) Console.Out.WriteLine($" -- {ant} at ({r}, {c})");
                }
            }
        }

        HashSet<Coord> antinodeLocations = new HashSet<Coord>();
        foreach (char ant in antennaMap.AntennaLocations.Keys)
        {
            List<Pair<Coord>> allAntPairs = antennaMap.GetAllPairsAnntennae(ant);
            foreach (Pair<Coord> pair in allAntPairs)
            {
                int dr = pair.B.R - pair.A.R;
                int dc = pair.B.C - pair.A.C;

                bool a1InBounds = true;
                bool a2InBounds = true;
                for (int m = 0; a1InBounds || a2InBounds; m++)
                {
                    Coord ant1 = new Coord(pair.A.R - dr * m, pair.A.C - dc * m);
                    Coord ant2 = new Coord(pair.B.R + dr * m, pair.B.C + dc * m);

                    a1InBounds = antennaMap.IsInBounds(ant1);
                    if (a1InBounds)
                    {
                        antinodeLocations.Add(ant1);
                        antinodesDebug[ant1.R][ant1.C] = '#';
                    }
                    a2InBounds = antennaMap.IsInBounds(ant2);
                    if (a2InBounds)
                    {
                        antinodeLocations.Add(ant2);
                        antinodesDebug[ant2.R][ant2.C] = '#';

                    }
                }
            }
        }

        if (debug)
        {
            Console.Out.WriteLine("Antinodes :: \n");
            for (int r = 0; r < antinodesDebug.Length; r++)
            {
                for (int c = 0; c < antinodesDebug[r].Length; c++)
                {
                    Console.Out.Write(antinodesDebug[r][c]);
                }
                Console.Out.WriteLine();
            }
        }

        numAntinodes = antinodeLocations.Count();

        return "number of antinodes = " + numAntinodes;
    }
}