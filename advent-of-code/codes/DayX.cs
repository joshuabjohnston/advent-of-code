
namespace org.jjohnston.aoc;


public class DayX : IDay
{
    public void FirstStarTest()
    {
        String[] test1Lines = {
            ""
        };
        this.FirstStarExec(test1Lines);    
    }

    public void FirstStarExec(string[] testInput)
    {
        Console.Out.WriteLine($"*** {this.GetType().Name} / Star 1 ***");
        int answer = 0;
        

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