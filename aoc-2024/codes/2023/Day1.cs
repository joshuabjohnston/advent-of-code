
namespace org.jjohnston.aoc.year2023;


public class Day1 : IDay
{
    public void FirstStarTest()
    {
        String[] test1Lines = {
            "1abc2",
            "pqr3stu8vwx",
            "a1b2c3d4e5f",
            "treb7uchet"
        };
        this.FirstStarExec(test1Lines);    
    }

    public void FirstStarExec(string[] testInput)
    {
        Console.Out.WriteLine($"*** {this.GetType().Name} / Star 1 ***");
        int answer = 0;
        
        foreach (String line in testInput)
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
            Console.Out.WriteLine($"[[ {thisNum} ]] -- {line}");
            answer += thisNum;
        }

        Console.Out.WriteLine($"Answer == {answer}");    
    }

    public void SecondStarTest()
    {
        String[] testLines = {
            ""
        };
        this.SecondStarExec(testLines);
    }

    public void SecondStarExec(string[] testInput)
    {
        Console.Out.WriteLine($"*** {this.GetType().Name} / Star 2 ***");
        int answer = 0;
        

        Console.Out.WriteLine($"Answer == {answer}");    
    }
}