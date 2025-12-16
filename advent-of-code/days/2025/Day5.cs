using System.Runtime.ExceptionServices;
using System.Transactions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day5 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int numFreshIngredients = 0;

        List<FreshIngredientRange> freshRanges = new List<FreshIngredientRange>();

        int inputIdx = 0;

        for (; inputIdx < inputs.Length; inputIdx++)
        {
            if (String.IsNullOrWhiteSpace(inputs[inputIdx]))
            {
                ++inputIdx;
                break;
            }

            String s = inputs[inputIdx];
            FreshIngredientRange fir = new FreshIngredientRange(s);
            freshRanges.Add(fir);
        }

        Console.Out.WriteLine($"Compiled {freshRanges.Count} ranges");

        int numIdsChecked = 0;
        for (; inputIdx < inputs.Length; inputIdx++)
        {
            ++numIdsChecked;
            Int64 id = Int64.Parse(inputs[inputIdx]);

            bool isInAnyRange = false;
            foreach (FreshIngredientRange range in freshRanges)
            {
                if (range.IsInRange(id))
                {
                    if (debug) Console.Out.WriteLine($"id {id} is in range {range.ToString()}");
                    isInAnyRange = true;
                    break;
                }
            }
            if (isInAnyRange)
            {
                if (debug) Console.Out.WriteLine($"id {id} is in a range");
                ++numFreshIngredients;
            }

        }

        Console.Out.WriteLine($"Checked {numIdsChecked} ids");

        return "Num fresh ingredients == " + numFreshIngredients;
    }

    public class FreshIngredientRange
    {
        public Int64 Min { get; set; }
        public Int64 Max { get; set; }

        public FreshIngredientRange(string rangeString)
        {
            string[] tokens = rangeString.Split('-');
            Min = Int64.Parse(tokens[0]);
            Max = Int64.Parse(tokens[1]);

            if (Min > Max) throw new ArgumentException("min > max");
        }

        public bool IsInRange(Int64 n)
        {
            return n >= Min && n <= Max;
        }

        public override string ToString()
        {
            return $"{Min} - {Max}";
        }
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        throw new NotImplementedException();
    }
}