

using System.Reflection;
using aoc2024;


string strCookieSession = "session=53616c7465645f5f4289f9104410d6e56436296ce0fa5b8bb1fa3378e97fb36639d5c94267ac61e6a0be6f3fdd88bcb033597f5a7a576b258b989f7dbe8f2837";

string strYear = "2024";
string strDay = "1";
WhichStar theStar = WhichStar.First;
// WhichStar theStar = WhichStar.Second;
ExecType execType = ExecType.Test;
// ExecType execType = ExecType.FullInput;


Console.Out.WriteLine($"Invoking AOC {strYear}, Day {strDay}.");

// do we have inputs?
String[] execInput = CheckAndFetchPuzzleInputs(strDay, strYear);

// System.Type? dayType = Type.GetType($"aoc{strYear}.Day{strDay}");
String strType = $"aoc2024.Day{strDay}";
System.Type? dayType = Type.GetType(strType);
if (dayType != null)
{
    IDay? iDay = (IDay?)Activator.CreateInstance(dayType);
    
    if (iDay != null)
    {
        if (theStar == WhichStar.First || theStar == WhichStar.Both)
        {
            if (execType == ExecType.Test)
            {
                try
                {
                    iDay.FirstStarTest();
                }
                catch (NotImplementedException)
                {
                    Console.Out.WriteLine("First Star test, not done");
                }
            }

            if (execType == ExecType.FullInput)
            {
                try
                {
                    iDay.FirstStarExec(execInput);
                }
                catch (NotImplementedException)
                {
                    Console.Out.WriteLine("First Star exec, not done");
                }
            }
        }

        if (theStar == WhichStar.Second || theStar == WhichStar.Both)
        {
            if (execType == ExecType.Test)
            {
                try
                {
                    iDay.SecondStarTest();
                }
                catch (NotImplementedException)
                {
                    Console.Out.WriteLine("Second Star test, not done");
                }
            }

            if (execType == ExecType.FullInput)
            {
                try
                {
                    iDay.SecondStarExec(execInput);
                }
                catch (NotImplementedException)
                {
                    Console.Out.WriteLine("Second Star exec, not done");
                }
            }
        }
    }
    else
    {
        Console.Out.WriteLine("Unable to instantiate instance of that day as IDay");
    }
}
else
{
    Console.Out.WriteLine($"No Type {strType} for that day located");
}

/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////


String[] CheckAndFetchPuzzleInputs(string day, string year)
{
    string inputFileName = $"puzzleInputs/day{day}.txt";

    if (File.Exists(inputFileName))
    {
        // perfect. good to go.
        Console.Out.WriteLine($"** Input file {inputFileName} exists");
    }
    else
    {
        // fetch it
        string strURI = $"https://adventofcode.com/{year}/day/{day}/input";

        Console.Out.WriteLine($"** Fetching input for {inputFileName} from {strURI}");

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Cookie", strCookieSession);

            using (Task<Stream> s = client.GetStreamAsync(strURI))
            {
                using (FileStream fs = new FileStream(inputFileName, FileMode.OpenOrCreate))
                {
                    s.Result.CopyTo(fs);
                }
            }
        }

        Console.Out.WriteLine("** Fetch complete");
    }

    return File.ReadAllLines(inputFileName);
}