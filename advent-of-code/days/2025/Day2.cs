using System.Runtime.InteropServices;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day2 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        String[] ranges = inputs[0].Split(",");

        Int64 sumInvalidIds = 0;

        foreach (string range in ranges)
        {
            String[] ends = range.Split("-");
            Int64 low = Int64.Parse(ends[0]);
            Int64 high = Int64.Parse(ends[1]);

            Console.Out.WriteLine($"Range {low} - {high}");

            for (Int64 i = low; i <= high; i++)
            {
                bool equalHalves = isIdEqualHalves(i);
                if (equalHalves)
                {
                    sumInvalidIds += i;
                }

                if (debug)
                {
                    Console.Out.WriteLine($"\t{i} -> {(equalHalves ? "yes" : "no")}");
                }
            }
        }

        return "sum of invalid IDs = " + sumInvalidIds;
    }

    public bool isIdEqualHalves(Int64 id)
    {
        String strId = id.ToString();

        if (strId.Length % 2 == 0)
        {
            int halfIdx = strId.Length / 2;
            string firstHalf = strId.Substring(0, halfIdx);
            string secondHalf = strId.Substring(halfIdx);

            return firstHalf.Equals(secondHalf);
        }

        return false;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        String[] ranges = inputs[0].Split(",");

        Int64 sumInvalidIds = 0;

        foreach (string range in ranges)
        {
            String[] ends = range.Split("-");
            Int64 low = Int64.Parse(ends[0]);
            Int64 high = Int64.Parse(ends[1]);

            if (debug) Console.Out.WriteLine($"Range {low} - {high}");

            for (Int64 i = low; i <= high; i++)
            {
                bool repeatPattern = isIdPatternThatRepeats(i, debug);
                if (repeatPattern)
                {
                    sumInvalidIds += i;
                }

                if (debug)
                {
                    Console.Out.WriteLine($"\t{i} -> {(repeatPattern ? "yes" : "no")}");
                }
            }
        }

        return "sum of invalid IDs = " + sumInvalidIds;
    }

    public bool isIdPatternThatRepeats(Int64 id, bool debug)
    {
        String strId = id.ToString();

        // length of pattern can be as long as half the string
        for (int len = 1; len < strId.Length / 2; len++)
        {
            // does this pattern repeat?
            bool thisPatternWorks = true;
            string pat = strId.Substring(0, len);
            for (int i = len; i < strId.Length - len && thisPatternWorks; i++)
            {
                string sub = strId.Substring(i, len);
                if (pat.Equals(sub))
                {
                    // yay
                }
                else
                {
                    thisPatternWorks = false;
                }
            }

            if (thisPatternWorks)
            {
                return true;
            }
        }

        return true;
    }
}