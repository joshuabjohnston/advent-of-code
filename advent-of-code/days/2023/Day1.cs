
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2023;


public class Day1 : AbstractDay
{
    public Day1() : base("1", "2023")
    {
    }

    public override string Star_1_Impl(string[] inputs)
    {
        int answer = 0;
        
        foreach (String line in inputs)
        {
            int firstNum = -1;
            int secondNum = -1;

            for (int c = 0; c < line.Length; c++)
            {
                if (Char.IsDigit(line[c]))
                {
                    int n = line[c] - '0';

                    if (firstNum < 0)
                    {
                        firstNum = n;
                    }
                    secondNum = n;
                }
            }

            int thisNum = firstNum * 10 + secondNum;
            // Console.Out.WriteLine($"[[ {thisNum} ]] -- {line}");
            answer += thisNum;
        }

        String ans = $"Answer == {answer}";
        return ans;
    }

    public override string Star_2_Impl(String[] inputs)
    {
        throw new NotImplementedException();
    }
}