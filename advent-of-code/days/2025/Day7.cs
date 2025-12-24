using System.Data;
using jjohnston_extensions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day7 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int numSplits = 0;

        char[][] chin = inputs.ToCharMatrix();

        int sIdx = 0;
        for (; sIdx < chin[0].Length; sIdx++)
        {
            if (chin[0][sIdx] == 'S')
            {
                break;
            }
        }

        if (debug) Console.Out.WriteLine($"S in col {sIdx}");

        // under the S goes a beam
        chin[1][sIdx] = '|';

        // Then look for splits under beams and make new beams. Then under those beams continue the beams.
        // And loop those 2 steps.
        for (int r = 2; r < chin.Length - 1; r += 2)
        {
            for (int c = 0; c < chin[r].Length; c++)
            {
                // splitter?
                if (chin[r][c] == '^' && chin[r - 1][c] == '|')
                {
                    // don't have to check for out of bounds, because we know the puzzle inputs have space on the edges.
                    chin[r][c - 1] = '|';
                    chin[r][c + 1] = '|';
                    ++numSplits;

                    // continue the beams.
                    chin[r + 1][c - 1] = '|';
                    chin[r + 1][c + 1] = '|';
                }
                // continue an existing beam
                else if (chin[r - 1][c] == '|' && chin[r][c] == '.')
                {
                    chin[r][c] = '|';
                    chin[r + 1][c] = '|';
                }
            }
        }

        if (debug)
        {
            for (int r = 0; r < chin.Length; r++)
            {
                for (int c = 0; c < chin[r].Length; c++)
                {
                    Console.Out.Write(chin[r][c]);
                }
                Console.Out.WriteLine();
            }
        }

        return "number of beam splits == " + numSplits;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        throw new NotImplementedException();
    }
}