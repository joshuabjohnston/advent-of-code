using jjohnston_extensions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day4 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int numFreeRolls = 0;

        int maxAdjacentRolls = 4;

        for (int r = 0; r < inputs.Length; r++)
        {
            for (int c = 0; c < inputs[r].Length; c++)
            {
                // is this a roll of paper?
                if (inputs[r][c] == '@')
                {
                    int adj = CountRollsAdjacentTo(inputs, r, c);
                    if (adj < maxAdjacentRolls)
                    {
                        numFreeRolls++;
                    }
                }
            }
        }

        return "number of free rolls == " + numFreeRolls;
    }

    public int CountRollsAdjacentTo(string[] inputs, int r, int c)
    {
        int numAdjRolls = 0;

        for (int newR = r - 1; newR <= r + 1; newR++)
        {
            if (newR < 0) continue;
            if (newR >= inputs.Length) continue;

            for (int newC = c - 1; newC <= c + 1; newC++)
            {
                if (newC < 0) continue;
                if (newC >= inputs[r].Length) continue;
                if (newR == r && newC == c) continue;

                if (inputs[newR][newC] == '@') numAdjRolls++;
            }
        }

        return numAdjRolls;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int totalRollsRemoved = 0;

        int maxAdjacentRolls = 4;

        char[][] charInputs = inputs.ToCharMatrix();

        int numRemovedRoll = 0;

        do
        {
            numRemovedRoll = 0;

            for (int r = 0; r < charInputs.Length; r++)
            {
                for (int c = 0; c < charInputs[r].Length; c++)
                {
                    // is this a roll of paper?
                    if (charInputs[r][c] == '@')
                    {
                        int adj = CountRollsAdjacentToSecond(charInputs, r, c);
                        if (adj < maxAdjacentRolls)
                        {
                            charInputs[r][c] = 'x';
                            // if (debug) Console.Out.WriteLine($"Can remove [{r}][{c}]");
                        }
                    }
                }
            }

            // now remove the rolls that we can
            for (int r = 0; r < charInputs.Length; r++)
            {
                for (int c = 0; c < charInputs[r].Length; c++)
                {
                    // is this a roll of paper to remove?
                    if (charInputs[r][c] == 'x')
                    {
                        ++numRemovedRoll;
                        ++totalRollsRemoved;
                        charInputs[r][c] = '.';
                        // if (debug) Console.Out.WriteLine($"Remove [{r}][{c}]");
                    }
                }
            }

            if (debug) Console.Out.WriteLine($" -> Removed {numRemovedRoll} rolls");

        } while (numRemovedRoll > 0);

        return "number of rolls removed == " + totalRollsRemoved;
    }

    public int CountRollsAdjacentToSecond(char[][] inputs, int r, int c)
    {
        int numAdjRolls = 0;

        for (int newR = r - 1; newR <= r + 1; newR++)
        {
            if (newR < 0) continue;
            if (newR >= inputs.Length) continue;

            for (int newC = c - 1; newC <= c + 1; newC++)
            {
                if (newC < 0) continue;
                if (newC >= inputs[r].Length) continue;
                if (newR == r && newC == c) continue;

                if (inputs[newR][newC] == '@'
                    || inputs[newR][newC] == 'x')
                {
                    numAdjRolls++;
                }
            }
        }

        return numAdjRolls;
    }
}