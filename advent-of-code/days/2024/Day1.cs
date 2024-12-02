using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day1 : AbstractDay
{
    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        foreach (String inp in inputs)
        {
            list1.Add(int.Parse(inp.Substring(0, 5)));

            list2.Add(int.Parse(inp.Substring(8)));

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
        List<int> list1 = new List<int>();
        // List<int> list2 = new List<int>();
        Dictionary<int, int> dict2 = new Dictionary<int, int>();

        foreach (String inp in inputs)
        {
            list1.Add(int.Parse(inp.Substring(0, 5)));

            int n2 = int.Parse(inp.Substring(8));
            if (dict2.ContainsKey(n2))
            {
                dict2[n2] = dict2[n2] + 1;
            }
            else
            {
                dict2[n2] = 1;
            }

            if (debug) Console.WriteLine($"inp = {inp}     --> [{list1[list1.Count-1]},{n2}]");
        }
        
        // list1.Sort();
        // list2.Sort();

        int sumSimScore = 0;
        for (int i = 0; i < list1.Count; i++)
        {
            int n1 = list1[i];

            // how many times does this appear in list 2?
            // int l2Count = list2.FindAll(x => x == n1).Count;
            int l2Count = (dict2.ContainsKey(n1) ? dict2[n1] : 0);

            int simScore = n1 * l2Count;
            sumSimScore += simScore;
        }

        return "Similary score == " + sumSimScore;
    }
}