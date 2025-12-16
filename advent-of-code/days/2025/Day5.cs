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

        public bool Overlaps(FreshIngredientRange other)
        {
            if (this.Min > other.Max) return false;
            if (this.Max < other.Min) return false;

            return true;
        }

        public void MergeWith(FreshIngredientRange other)
        {
            this.Min = Math.Min(this.Min, other.Min);
            this.Max = Math.Max(this.Max, other.Max);
        }

        public Int64 Count()
        {
            return this.Max - this.Min + 1; // +1 because it's inclusive
        }
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        Int64 numFreshIds = 0;

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

            // does this new range merge with any on our list already?
            // if it does, we need to take that new range out of the list and re-check it for merges
            bool didAMerge = false;
            do
            {
                didAMerge = false;

                for (int i = 0; i < freshRanges.Count && !didAMerge; i++)
                {
                    FreshIngredientRange range = freshRanges[i];
                    if (fir.Overlaps(range))
                    {
                        freshRanges.RemoveAt(i);
                        fir.MergeWith(range);
                        didAMerge = true;
                        break;
                    }
                }
            } while (didAMerge);

            freshRanges.Add(fir);
        }

        // Loop over unique ranges and count how many items are in them with subtraction
        foreach (FreshIngredientRange range in freshRanges)
        {
            numFreshIds += range.Count();
        }

        return "Number of fresh IDs == " + numFreshIds;
    }
}