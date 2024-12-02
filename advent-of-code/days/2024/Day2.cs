
using System.Diagnostics.Tracing;
using jjohnston_extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024
{
    public class Day2 : AbstractDay
    {
        public enum Direction
        {
            Unknown, 
            Ascending,
            Descending
        }

        public override string Star_1_Impl(string[] inputs, bool debug)
        {
            int nCountSafe = 0;

            foreach (String input in inputs)
            {
                string[] strLevels = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int[] levels = strLevels.ParseInts();
                bool bSafeReport = IsReportSafe(levels, debug);                

                if (bSafeReport)
                {
                    ++nCountSafe;
                }
            }

            return "Number safe reports == " + nCountSafe;
        }

        public bool IsReportSafe(int[] levels, bool debug)
        {                
            int thisLevel = -999;
            int prevLevel = -999;
            Direction thisDir = Direction.Unknown;
            Direction prevDir = Direction.Unknown;

            bool bSafeReport = true;

            for (int i = 1; i < levels.Length; i++)
            {
                if (prevLevel == -999)
                {
                    prevLevel = levels[0];
                }
                else
                {
                    prevLevel = thisLevel;
                    prevDir = thisDir;
                }
                thisLevel = levels[i];

                thisDir = (thisLevel > prevLevel) ? Direction.Ascending : Direction.Descending;
                if (prevDir != Direction.Unknown)
                {
                    if (thisDir != prevDir)
                    {
                        bSafeReport = false;
                        if (debug) Console.Out.WriteLine($"{String.Join(' ', levels)} -- UNSAFE -- change in direction");
                        break;
                    }
                }
                int diff = Math.Abs(thisLevel - prevLevel);
                if (diff < 1 || diff > 3)
                {
                    bSafeReport = false;
                    if (debug) Console.Out.WriteLine($"{String.Join(' ', levels)} -- UNSAFE -- diff of {diff}");
                    break;
                }
            }

            return bSafeReport;
        }

        public override string Star_2_Impl(string[] inputs, bool debug)
        {
            int nCountSafe = 0;

            foreach (String input in inputs)
            {
                string[] strLevels = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int[] levels = strLevels.ParseInts();
                bool bSafeReport = IsReportSafe(levels, debug);                

                for (int i = 0; i < strLevels.Length && !bSafeReport; i++)
                {
                    // throw out level i
                    // List<String> newLevels = new List<string>(strLevels);
                    // newLevels.RemoveAt(i);
                    // bSafeReport = IsReportSafe(newLevels.ToArray(), debug);

                    // string[] newLevels = new string[strLevels.Length - 1];
                    // int newIdx = 0;
                    // for (int j = 0; j < strLevels.Length; j++)
                    // {
                    //     if (j == i)
                    //     {
                    //         continue;
                    //     }
                    //     newLevels[newIdx] = strLevels[i];
                    //     ++newIdx;
                    // }

                    int[] newLevels = levels.RemoveAt(i);
                    bSafeReport = IsReportSafe(newLevels, debug);
                }

                if (bSafeReport)
                {
                    ++nCountSafe;
                }
            }

            return "Number safe reports == " + nCountSafe;
        }
    }
}

