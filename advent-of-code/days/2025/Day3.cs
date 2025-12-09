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
        int sumJoltage = 0;

        foreach (String batteryBank in inputs)
        {
            int maxJoltage = 0;



            if (debug) Console.Out.WriteLine($"{batteryBank} max joltage == {maxJoltage}");

            sumJoltage += maxJoltage;
        }

        return "sum joltages = " + sumJoltage;
    }
}