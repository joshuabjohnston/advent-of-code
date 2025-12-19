using System.Reflection.Metadata;
using Microsoft.Extensions.FileProviders;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day6 : AbstractDay
{
    public enum Operation
    {
        Add,
        Multiply
    };

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        Int64 sumAnswers = 0;

        string[][] probCols = new string[inputs.Length][];
        int numRows = 0;
        for (int i = 0; i < inputs.Length; i++)
        {
            String line = inputs[i];
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            probCols[i] = tokens;

            ++numRows;
        }

        // eval each problem
        for (int c = 0; c < probCols[0].Count(); c++)
        {
            // is this col add or mult?
            Operation thisOp = Operation.Add;
            if (probCols[numRows - 1][c].Equals("*"))
            {
                thisOp = Operation.Multiply;
            }

            Int64 opAnswer = (thisOp == Operation.Add ? 0 : 1);
            String debugOp = opAnswer.ToString();
            for (int r = 0; r < inputs.Length - 1; r++)
            {
                String strOperand = probCols[r][c];
                debugOp += " " + (thisOp == Operation.Add ? "+" : "*") + strOperand;

                Int64 operand = Int64.Parse(strOperand);

                if (thisOp == Operation.Add)
                {
                    opAnswer += operand;
                }
                else
                {
                    opAnswer *= operand;
                }
            }

            if (debug) Console.Out.WriteLine($"\t-> {debugOp} = {opAnswer}");

            sumAnswers += opAnswer;
        }


        return "sum of all answers == " + sumAnswers;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        Int64 sumAnswers = 0;

        // column at a time, starting at the right end and working backward toward the front, 0 column
        Operation thisOp = Operation.Add;
        List<Int64> operands = new List<Int64>();

        for (int c = inputs[0].Length - 1; c >= 0; c--)
        {
            bool hasOperand = false;

            // build operands top to bottom.
            // when we see an operator, evaluate. Then skip a col to the left b.c it's all a sigle col of whitespace
            String operand = "";
            for (int r = 0; r < inputs.Length; r++)
            {
                char ch = inputs[r][c];
                if (ch >= '0' && ch <= '9')
                {
                    operand += ch;
                }
                if (ch == '+')
                {
                    thisOp = Operation.Add;
                    hasOperand = true;
                }
                else if (ch == '*')
                {
                    thisOp = Operation.Multiply;
                    hasOperand = true;
                }
            }
            operands.Add(Int64.Parse(operand));


            if (hasOperand)
            {
                Int64 opResult = (thisOp == Operation.Add ? 0 : 1);
                String debugOp = $"\t-> {opResult}";
                foreach (Int64 op in operands)
                {
                    debugOp += " " + (thisOp == Operation.Add ? "+" : "*") + op.ToString();
                    if (thisOp == Operation.Add)
                    {
                        opResult += op;
                    }
                    else
                    {
                        opResult *= op;
                    }
                }

                if (debug) Console.Out.WriteLine($"\t-> {debugOp} = {opResult}");
                sumAnswers += opResult;
                operands.Clear();
                hasOperand = false;
                c--; // skip the empty column to our left
            }
        }

        return "sum of all answers == " + sumAnswers;
    }
}