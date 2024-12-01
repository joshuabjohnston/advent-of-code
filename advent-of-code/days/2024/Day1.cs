using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day1 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        String res = "aoc 2024 day 1, star 1";

        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        foreach (String inp in inputs)
        {
            int num1End = 0;
            while (Char.IsDigit(inp[num1End])) ++num1End;
            list1.Add(int.Parse(inp.Substring(0, num1End)));

            int num2Beg = inp.Length - 1;
            while (Char.IsDigit(inp[num2Beg])) --num2Beg;
            list2.Add(int.Parse(inp.Substring(num2Beg)));

            if (debug) Console.WriteLine($"inp = {inp}     --> [{list1[list1.Count-1]},{list2[list2.Count-1]}]");
        }

        list1.Sort();
        list2.Sort();

        int sumDiffs = 0;

        for (int i = 0; i < list1.Count; i++)
        {
            int diff = Math.Abs(list1[i] - list2[i]);
            sumDiffs += diff;
        }

        return "sum of diffs == "+ sumDiffs;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        String res = "aoc 2024 day 1, star 2";

        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        foreach (String inp in inputs)
        {
            int num1End = 0;
            while (Char.IsDigit(inp[num1End])) ++num1End;
            list1.Add(int.Parse(inp.Substring(0, num1End)));

            int num2Beg = inp.Length - 1;
            while (Char.IsDigit(inp[num2Beg])) --num2Beg;
            list2.Add(int.Parse(inp.Substring(num2Beg)));

            if (debug) Console.WriteLine($"inp = {inp}     --> [{list1[list1.Count-1]},{list2[list2.Count-1]}]");
        }
        
        list1.Sort();
        list2.Sort();

        int sumSimScore = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            int n1 = list1[i];

            // how many times does this appear in list 2?
            int l2Count = list2.FindAll(x => x == n1).Count;

            int simScore = n1 * l2Count;
            sumSimScore += simScore;
        }

        return "Similary score == " + sumSimScore;
    }
}