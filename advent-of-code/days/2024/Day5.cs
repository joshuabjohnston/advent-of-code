using System.ComponentModel;
using jjohnston_extensions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day5 : AbstractDay
{
    public class OrderingRule
    {
        private bool _bIsSorted = false;

        public int Page { get; set; } = 0;
        public List<int> PagesAfter { get; set; } = new List<int>();

        public void AddRule(int pgAftter)
        {
            this.PagesAfter.Add(pgAftter);
            this._bIsSorted = false;
        }

        public bool ContainsPageAfter(int pg)
        {
            if (!_bIsSorted)
            {
                PagesAfter.Sort();
            }

            return PagesAfter.Contains(pg);
        }

        public override int GetHashCode()
        {
            return Page;
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int sumMiddePageOfCorrectOrders = 0;

        Dictionary<int, OrderingRule> orderingRules = new Dictionary<int, OrderingRule>();

        foreach (string input in inputs)
        {
            // parsing a rule
            if (input.Contains('|'))
            {
                string[] tokens = input.Split('|');
                int pgBefore = int.Parse(tokens[0]);
                int pgAfter = int.Parse(tokens[1]);

                if (!orderingRules.ContainsKey(pgBefore))
                {
                    OrderingRule rule = new OrderingRule() { Page = pgBefore };
                    orderingRules.Add(pgBefore, rule);
                }
                orderingRules[pgBefore].AddRule(pgAfter);
            }
            // empty line before test cases
            else if (input.Trim().Equals(String.Empty))
            {
                // no-op
            }
            // else test case
            else
            {
                int[] pages = input.Split(',').ParseInts();

                bool bAllPagesOrderOk = true;

                for (int i = 0; i < pages.Length; i++)
                {
                    int thisPage = pages[i];


                    // are there even any rules for this page?
                    if (orderingRules.ContainsKey(thisPage))
                    {
                        // look backward. Are any "pages after" for THIS page before it in the list?
                        OrderingRule thisPageRules = orderingRules[thisPage];
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (thisPageRules.ContainsPageAfter(pages[j]))
                            {
                                bAllPagesOrderOk = false;
                                break;
                            }
                        }
                    }
                }

                if (bAllPagesOrderOk)
                {
                    int midPage = pages[pages.Length / 2];
                    if (debug) Console.Out.WriteLine($"Page {midPage} from valid ordering {input}");
                    sumMiddePageOfCorrectOrders += midPage;
                }
            }
        }

        return "ans = " + sumMiddePageOfCorrectOrders;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int sumMiddePageOfCorrectedOrders = 0;

        Dictionary<int, OrderingRule> orderingRules = new Dictionary<int, OrderingRule>();

        foreach (string input in inputs)
        {
            // parsing a rule
            if (input.Contains('|'))
            {
                string[] tokens = input.Split('|');
                int pgBefore = int.Parse(tokens[0]);
                int pgAfter = int.Parse(tokens[1]);

                if (!orderingRules.ContainsKey(pgBefore))
                {
                    OrderingRule rule = new OrderingRule() { Page = pgBefore };
                    orderingRules.Add(pgBefore, rule);
                }
                orderingRules[pgBefore].AddRule(pgAfter);
            }
            // empty line before test cases
            else if (input.Trim().Equals(String.Empty))
            {
                // no-op
            }
            // else test case
            else
            {
                int[] pages = input.Split(',').ParseInts();

                bool bAllPagesOrderOk = true;
                bool bCorrectedThisOrdering = false;

                for (int i = 0; i < pages.Length; i++)
                {
                    int thisPage = pages[i];


                    // are there even any rules for this page?
                    if (orderingRules.ContainsKey(thisPage))
                    {
                        // look backward. Are any "pages after" for THIS page before it in the list?
                        OrderingRule thisPageRules = orderingRules[thisPage];
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (thisPageRules.ContainsPageAfter(pages[j]))
                            {
                                bAllPagesOrderOk = false;
                                bCorrectedThisOrdering = true;
                                pages.Swap(i, j);
                                break;
                            }
                        }
                    }

                    if (!bAllPagesOrderOk)
                    {
                        // do over!
                        i = 0;
                        bAllPagesOrderOk = true;
                    }
                }

                if (bCorrectedThisOrdering)
                {
                    int midPage = pages[pages.Length / 2];
                    if (debug) Console.Out.WriteLine($"Invalid ordering {input} corrected to {pages.GetString()}");
                    sumMiddePageOfCorrectedOrders += midPage;
                }
            }
        }

        return "ans = " + sumMiddePageOfCorrectedOrders;
    }
}