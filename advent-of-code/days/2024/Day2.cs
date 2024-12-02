
using System.Diagnostics.Tracing;
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
                bool bSafeReport = IsReportSafe(strLevels, debug);                

                if (bSafeReport)
                {
                    ++nCountSafe;
                }
            }

            return "Number safe reports == " + nCountSafe;
        }

        public bool IsReportSafe(string[] strLevels, bool debug)
        {                
            int thisLevel = -999;
            int prevLevel = -999;
            Direction thisDir = Direction.Unknown;
            Direction prevDir = Direction.Unknown;

            bool bSafeReport = true;

            for (int i = 1; i < strLevels.Length; i++)
            {
                if (prevLevel == -999)
                {
                    prevLevel = int.Parse(strLevels[0]);
                }
                else
                {
                    prevLevel = thisLevel;
                    prevDir = thisDir;
                }
                thisLevel = int.Parse(strLevels[i]);

                thisDir = (thisLevel > prevLevel) ? Direction.Ascending : Direction.Descending;
                if (prevDir != Direction.Unknown)
                {
                    if (thisDir != prevDir)
                    {
                        bSafeReport = false;
                        if (debug) Console.Out.WriteLine($"{String.Join(' ', strLevels)} -- UNSAFE -- change in direction");
                        break;
                    }
                }
                int diff = Math.Abs(thisLevel - prevLevel);
                if (diff < 1 || diff > 3)
                {
                    bSafeReport = false;
                    if (debug) Console.Out.WriteLine($"{String.Join(' ', strLevels)} -- UNSAFE -- diff of {diff}");
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
                bool bSafeReport = IsReportSafe(strLevels, debug);                

                for (int i = 0; i < strLevels.Length && !bSafeReport; i++)
                {
                    // throw out level i
                    List<String> newLevels = new List<string>(strLevels);
                    newLevels.RemoveAt(i);
                    bSafeReport = IsReportSafe(newLevels.ToArray(), debug);
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

