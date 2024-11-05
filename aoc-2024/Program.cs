

using aoc2024;

string strDay = "1";
if (args.Count() > 0)
{
    strDay = args[0];
}

Console.Out.WriteLine($"Invoking AOC 2024, Day {strDay}.");

System.Type? dayType = Type.GetType($"aoc2024.Day{strDay}");
if (dayType != null)
{
    IDay? iDay = (IDay?)Activator.CreateInstance(dayType);
    
    if (iDay != null)
    {
        TestDay(iDay);
    }
    else
    {
        Console.Out.WriteLine("Unable to instantiate instance of that day as IDay");
    }
}
else
{
    Console.Out.WriteLine("No Type for that day located");
}

/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////

void TestDay(IDay day)
{
    String[] lines = {"1", "2", "3"};
    day.Exec(lines);
}