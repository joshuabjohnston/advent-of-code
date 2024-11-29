using System.Reflection;
using Microsoft.Extensions.Configuration;
using org.jjohnston.aoc;
using org.jjohnston.aoc.year2024;


var builder = new ConfigurationBuilder().AddJsonFile("config.json", false);
var config = builder.Build();

string strCookieSession = config["AOCSessionCookie"] ?? "";

string strYear = "2024";
string strDay = "1";
WhichStar theStar = WhichStar.First;
// WhichStar theStar = WhichStar.Second;
ExecType execType = ExecType.Test;
// ExecType execType = ExecType.FullInput;


Console.Out.WriteLine($"Invoking AOC {strYear}, Day {strDay}.");

// do we have inputs?
String[] execInput = CheckAndFetchPuzzleInputs(strDay, strYear);

String strType = $"org.jjohnston.aoc.year{strYear}.Day{strDay}";
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
    string dir = Path.Combine("puzzleInputs", year);
    if (!Directory.Exists(dir))
    {
        Console.Out.WriteLine($"Creating directory {dir}");
        Directory.CreateDirectory(dir);
    }
    else
    {
        Console.Out.WriteLine($"{dir} exists.");
    }


    string inputFileName = Path.Combine(dir, $"day{day}.txt");

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