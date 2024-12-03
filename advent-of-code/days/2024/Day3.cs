using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using jjohnston_extensions;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024
{
    public class Day3 : AbstractDay
    {
        public override string Star_1_Impl(string[] inputs, bool debug)
        {
            int sumMuls = 0;

            foreach (string input in inputs)
            {
                // sumMuls += SumMuls(input, debug, false);
                MatchCollection matches = Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)");
                foreach (Match m in matches)
                {
                    int x = int.Parse(m.Groups[1].Value);
                    int y = int.Parse(m.Groups[2].Value);

                    int prod = x * y;
                    sumMuls += prod;
                    if (debug) Console.Out.WriteLine($"mul( {x}, {y} ) == {prod}");
                }
            }

            return "sum of muls == " + sumMuls;
        }

        public int SumMuls(string input, bool debug, bool withDoDont)
        {
            if (debug) Console.Out.WriteLine("--- " + input);
            int sumMuls = 0;

            bool bDo = true;

            for (int i = 0; i < input.Length; i++)
            {
                // find a mul(
                if ((input.Length - i) > 4 && input.Substring(i, 4).Equals("mul("))
                {
                    int x = 0;
                    int y = 0;

                    i += 4;

                    // how many digits until a comma?
                    int j = i;
                    while (Char.IsDigit(input[j]))
                    {
                        j++;
                    }
                    if (input[j] == ',')
                    {
                        // 1-3 long? 
                        if ((j-i) > 0 && (j-i) <= 3)
                        {
                            x = int.Parse(input.Substring(i,(j-i)));

                            // how many more digits until a )?
                            ++j; // move past the ,
                            i = j;
                            while (Char.IsDigit(input[j]))
                            {
                                j++;
                            }
                            if (input[j] == ')')
                            {
                                // 1-3 long? 
                                if ((j-i) > 0 && (j-i) <= 3)
                                {
                                    y = int.Parse(input.Substring(i,(j-i)));
                                    // ++j; // move past the ) // don't do this. because you repeat the ++ at the top of the loop.
                                    i = j;

                                    int prod = x* y;
                                    if (withDoDont)
                                    {
                                        if (debug) Console.Out.WriteLine($"{(bDo ? "DO" : "DON'T")} mul( {x}, {y} ) == {prod}");
                                        if (bDo)
                                        {
                                            sumMuls += prod;
                                        }
                                    }
                                    else
                                    {
                                        sumMuls += prod;
                                        if (debug) Console.Out.WriteLine($"mul( {x}, {y} ) == {prod}");
                                    }
                                }
                                else
                                {
                                    i = j;
                                }
                            }
                            else  
                            {
                                i = j;
                            }
                        }
                        else 
                        {
                            i = j;
                        }
                    }
                    else
                    {
                        i = j;
                    }
                }
                else if ((input.Length - i) > 4 && input.Substring(i, 4).Equals("do()"))
                {
                    if (debug) Console.Out.WriteLine("do()");
                    bDo = true;
                    i += 4;
                }
                else if ((input.Length - i) > 7 && input.Substring(i, 7).Equals("don't()"))
                {
                    if (debug) Console.Out.WriteLine("don't()");
                    bDo = false;
                    i += 7;
                }
            }

            return sumMuls;
        }

        public override string Star_2_Impl(string[] inputs, bool debug)
        {
            int sumMuls = 0;

            bool bDo = true;


            foreach (string input in inputs)
            {
                // sumMuls += SumMuls(input, debug, false);
                MatchCollection matches = Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
                foreach (Match m in matches)
                {
                    if (m.Value.StartsWith("mul") && bDo)
                    {
                        int x = int.Parse(m.Groups[1].Value);
                        int y = int.Parse(m.Groups[2].Value);

                        int prod = x * y;
                        sumMuls += prod;
                        if (debug) Console.Out.WriteLine($"mul( {x}, {y} ) == {prod}");
                    }
                    else if (m.Value.StartsWith("don't"))
                    {
                        bDo = false;
                    }
                    else if (m.Value.StartsWith("do"))
                    {
                        bDo = true;
                    }
                }
            }

            return "sum of muls == " + sumMuls;
        }
    }
}