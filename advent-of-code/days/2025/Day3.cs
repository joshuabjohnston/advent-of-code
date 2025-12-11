using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day3 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int sumJoltage = 0;

        foreach (String batteryBank in inputs)
        {
            int maxJoltage = 0;

            for (int d1x = 0; d1x < batteryBank.Length - 1; d1x++)
            {
                int d1 = (batteryBank[d1x] - '0') * 10;

                for (int d2x = d1x + 1; d2x < batteryBank.Length; d2x++)
                {
                    int d2 = batteryBank[d2x] - '0';

                    int joltage = d1 + d2;
                    if (joltage > maxJoltage)
                    {
                        maxJoltage = joltage;
                    }
                }
            }

            if (debug) Console.Out.WriteLine($"{batteryBank} max joltage == {maxJoltage}");

            sumJoltage += maxJoltage;
        }

        return "sum joltages = " + sumJoltage;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        Int64 sumJoltage = 0;

        foreach (String batteryBank in inputs)
        {
            if (debug) Console.Out.WriteLine($"Bank: {batteryBank}");

            Int64 maxJoltage = FindMaxJoltageOfLength12(String.Empty, batteryBank, debug);

            if (debug) Console.Out.WriteLine($"\t -> joltage: {maxJoltage}");

            sumJoltage += maxJoltage;
        }

        return "sum joltages = " + sumJoltage;
    }

    public Int64 FindMaxJoltageOfLength12(string joltageSoFar, string remainingBatteryBank, bool debug)
    {
        int targetLen = 12;

        int diffFromTargetLength = targetLen - joltageSoFar.Length;

        // if there's no room for enough joltages left in the battery bank, just abort. Don't try to shove
        // a 12-length joltage into a bank with 5 spots left
        if (joltageSoFar.Length < targetLen
            && (diffFromTargetLength) > remainingBatteryBank.Length)
        {
            return 0;
        }

        if (joltageSoFar.Length > targetLen)
        {
            return 0;
        }

        // is the joltage of the correct length? Stop.
        if (joltageSoFar.Length == targetLen)
        {
            if (debug) Console.Out.WriteLine($"\tJoltage: {joltageSoFar}");
            return Int64.Parse(joltageSoFar);
        }

        // only consider spots with the biggest digit available that might possibly work. Those are the 
        // only numbers that can be max.
        List<int> maxDigitIndices = new List<int>();
        int maxDigit = 0;
        for (int c = 0; c < (remainingBatteryBank.Length - (diffFromTargetLength - 1)); c++)
        {
            int dig = remainingBatteryBank[c] - '0';
            if (dig > maxDigit)
            {
                maxDigitIndices.Clear();
                maxDigit = dig;
            }

            if (dig == maxDigit)
            {
                maxDigitIndices.Add(c);
            }
        }

        Int64 localMaxJoltage = 0;
        // slide down the remaining battery bank, if we have room, and start adding joltages from that position.
        foreach (int c in maxDigitIndices)
        {
            string newJoltageSoFar = joltageSoFar + remainingBatteryBank[c];
            string newRemaining = remainingBatteryBank.Substring(c + 1);

            if (debug) Console.Out.WriteLine($"\t{newJoltageSoFar} | {newRemaining}");

            Int64 recursiveMax = FindMaxJoltageOfLength12(newJoltageSoFar, newRemaining, debug);
            if (recursiveMax > localMaxJoltage)
            {
                localMaxJoltage = recursiveMax;
            }
        }

        return localMaxJoltage;
    }
}