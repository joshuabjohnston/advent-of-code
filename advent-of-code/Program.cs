using System.Collections.Specialized;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using org.jjohnston.aoc;
using org.jjohnston.aoc.days;




GlobalConfig.DebugInputs = false;
GlobalConfig.DebugInputs = true;

// GlobalConfig.DebugTests = false;
GlobalConfig.DebugTests = true;

string strYear = "2025";
string strDay = "7";
WhichStar theStar = WhichStar.First;
// WhichStar theStar = WhichStar.Second;





Console.Out.WriteLine("Advent of Code - Starting Program");


var builder = new ConfigurationBuilder().AddJsonFile("config.json", false);
var config = builder.Build();
GlobalConfig.AOCAuthCookie = config["AOCSessionCookie"] ?? "unknown";
GlobalConfig.UserAgent = config["User-Agent"] ?? "unknown";

String strType = $"org.jjohnston.aoc.year{strYear}.Day{strDay}";
System.Type? dayType = Type.GetType(strType);
if (dayType != null)
{
    IDay? iDay = (IDay?)Activator.CreateInstance(dayType);

    if (iDay != null)
    {
        if (theStar == WhichStar.First || theStar == WhichStar.Both)
        {
            try
            {
                iDay.Star_1();
            }
            catch (NotImplementedException)
            {
                Console.Out.WriteLine("First Star, not implemented");
            }
        }

        if (theStar == WhichStar.Second || theStar == WhichStar.Both)
        {
            try
            {
                iDay.Star_2();
            }
            catch (NotImplementedException)
            {
                Console.Out.WriteLine("Second Star, not implemented");
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