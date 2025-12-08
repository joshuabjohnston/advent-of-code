using System.ComponentModel;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2025;

public class Day1 : AbstractDay
{
    public Day1() : base()
    {
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        int dial = 50;

        int numZeroes = 0;

        foreach (String s in inputs)
        {
            char dir = s[0];
            int num = int.Parse(s.Substring(1));

            if (dir == 'R')
            {
                dial += num;
            }
            else if (dir == 'L')
            {
                dial -= num;
            }

            while (dial < 0 || dial >= 100)
            {
                if (dial < 0)
                {
                    dial += 100;
                }
                else if (dial >= 100)
                {
                    dial -= 100;
                }
            }

            if (dial == 0)
            {
                numZeroes++;
            }

            if (debug)
            {
                Console.WriteLine($"{s}, Direction: {dir}, Number: {num} = {dial}. Current zeroes: {numZeroes}");
            }
        }

        return "number of zeroes == " + numZeroes;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        int dial = 50;

        int numZeroes = 0;


        foreach (String s in inputs)
        {
            int startingDial = dial;

            char dir = s[0];
            int num = int.Parse(s.Substring(1));

            while (num >= 100)
            {
                numZeroes++;
                num -= 100;
            }

            if (dir == 'R')
            {
                dial += num;
                if (dial >= 100)
                {
                    if (startingDial != 0) ++numZeroes;
                    dial -= 100;
                }
            }
            else if (dir == 'L')
            {
                dial -= num;
                if (dial < 0)
                {
                    if (startingDial != 0) ++numZeroes;
                    dial += 100;
                }
                else if (dial == 0)
                {
                    ++numZeroes;
                }
            }

            if (debug)
            {
                Console.WriteLine($"{s}\t{startingDial} {(dir == 'L' ? "-" : "+")} {num} = {dial}\tNum zeroes == {numZeroes}");
            }
        }

        return "number of zeros == " + numZeroes;
    }
}