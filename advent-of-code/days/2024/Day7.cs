using System.Text;
using jjohnston_extensions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day7 : AbstractDay
{
    public class Operation
    {
        public long Answer { get; set; }
        public List<int> Operands { get; set; }

        public Operation()
        {
            this.Answer = 0;
            this.Operands = new List<int>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Operands.Count(); i++)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" +|* ");
                }
                sb.Append(Operands[i]);
            }
            sb.Append(" =?= ").Append(Answer);

            return sb.ToString();
        }

        public static Operation TryParse(out bool bSuccess, String s)
        {
            bSuccess = false;
            Operation op = Operation.NoOp;

            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    int colIdx = s.IndexOf(':');
                    string sAns = s.Substring(0, colIdx);
                    int[] ops = s.Substring(colIdx + 1).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ParseInts();

                    if (colIdx > 0 && ops.Length > 0)
                    {
                        op = new Operation();
                        op.Answer = long.Parse(sAns);
                        op.Operands.AddRange(ops);
                    }

                    bSuccess = true;
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Can't parse :: " + s);
                }
            }

            return op;
        }

        public static Operation NoOp
        {
            get
            {
                Operation noop = new Operation();
                noop.Answer = -1;
                noop.Operands.Add(-1);

                return noop;
            }
        }
    }

    public bool CanCalculate(Operation op, bool debug, bool bWithConcat)
    {
        List<long> potentialAnswers = new List<long>();
        potentialAnswers.Add(op.Operands[0]);

        // if (debug) Console.Out.WriteLine($"  -- op.Answer == {op.Answer}");

        for (int o = 1; o < op.Operands.Count(); o++)
        {
            List<long> newValuesToPutOnTheEnd = new List<long>();

            if (debug) Console.Out.WriteLine("  [[ " + potentialAnswers.ToArray().GetString() + " ]]");

            for (int a = potentialAnswers.Count() - 1; a >= 0; a--)
            {
                long prevAnswer = potentialAnswers[a];
                potentialAnswers.RemoveAt(a);

                long newOp = op.Operands[o];

                // plus is a new answer
                long newPlus = prevAnswer + newOp;
                if (newPlus <= op.Answer)
                {
                    newValuesToPutOnTheEnd.Add(newPlus);
                    if (debug) Console.Out.WriteLine($" -- {prevAnswer} + {newOp} = {newPlus}");
                }

                // times is a new answer
                long newTimes = prevAnswer * newOp;
                if (newTimes <= op.Answer)
                {
                    newValuesToPutOnTheEnd.Add(newTimes);
                    if (debug) Console.Out.WriteLine($" -- {prevAnswer} * {newOp} = {newTimes}");
                }

                // concatenation is a new operation
                if (bWithConcat)
                {
                    long newCat = long.Parse("" + prevAnswer + newOp);
                    if (newCat <= op.Answer)
                    {
                        newValuesToPutOnTheEnd.Add(newCat);
                        if (debug) Console.Out.WriteLine($" -- {prevAnswer} || {newOp} = {newCat}");
                    }
                }
            }

            potentialAnswers.AddRange(newValuesToPutOnTheEnd);
        }

        return potentialAnswers.Contains(op.Answer);
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        long sumOfCalcOps = 0;

        foreach (string s in inputs)
        {
            if (debug) Console.Out.WriteLine($"input = {s}");
            bool bParsed = false;
            Operation op = Operation.TryParse(out bParsed, s);

            // if (debug) Console.Out.WriteLine($" operand parsed (?{(bParsed ? 'T' : 'F')}) to {op.ToString()}");

            if (CanCalculate(op, debug, false))
            {
                if (debug) Console.Out.WriteLine(" -- CALC!");
                sumOfCalcOps += op.Answer;
            }
        }

        return $"sum of calculable operations = {sumOfCalcOps}";
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        long sumOfCalcOps = 0;

        foreach (string s in inputs)
        {
            if (debug) Console.Out.WriteLine($"input = {s}");
            bool bParsed = false;
            Operation op = Operation.TryParse(out bParsed, s);

            // if (debug) Console.Out.WriteLine($" operand parsed (?{(bParsed ? 'T' : 'F')}) to {op.ToString()}");

            if (CanCalculate(op, debug, true))
            {
                if (debug) Console.Out.WriteLine(" -- CALC!");
                sumOfCalcOps += op.Answer;
            }
        }

        return $"sum of calculable operations = {sumOfCalcOps}";
    }
}