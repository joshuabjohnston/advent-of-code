using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace org.jjohnston.aoc.days;

public abstract class AbstractDay : IDay
{

    public String Day {get; protected set;}

    public String Year {get; protected set;}

    public String[] PuzzleInputs {get; protected set;}


    public AbstractDay()
    {
        String? fullType = this.GetType().FullName; // org.jjohnston.aoc.yearXXXX.DayNN
        if (fullType != null)
        {
            // Console.Out.WriteLine($"AbstractDay() :: type name = {fullType}");
            this.Year = fullType.Substring(22, 4);
            this.Day = fullType.Substring(30);
            // Console.Out.WriteLine($"AbstractDay() :: year = {this.Year}");
            // Console.Out.WriteLine($"AbstractDay() :: day = {this.Day}");
        }
        else 
        {
            throw new NullReferenceException("type's full name is null");
        }

        Console.Out.WriteLine($"Invoking AOC {this.Year}, Day {this.Day}.");

        FileHelpers.EnsureExistsStarTestFiles(this.Day, this.Year);
        this.PuzzleInputs = FileHelpers.ReadWithFetchPuzzleInputs(this.Day, this.Year);
    }

    protected void PrintExecutionHeader(MethodBase? methodBase)
    {
        String methodName = "unknown";
        if (methodBase != null)
        {
            methodName = methodBase.Name;
        }
        Console.Out.WriteLine($"** Executing Day {this.Day}, {methodName} **");
    }

    public void Star_1()
    {
        PrintExecutionHeader(System.Reflection.MethodBase.GetCurrentMethod());

        SpecificStarExecution(FileHelpers.ReadStar1Tests, Star_1_Impl);
    }

    protected void SpecificStarExecution(Func<string, string, string[]> fileDelegate, Func<string[], bool, string> starDelegate)
    {
        Stopwatch sw = new Stopwatch();

        List<String> testResults = new List<string>();
        List<String> testTimings = new List<string>();
        // load all the tests and execute them.
        String[] allTestLines = fileDelegate(this.Day, this.Year);
        
        List<String> singleTestCase = new List<string>();
        foreach (string testLine in allTestLines)
        {
            if (testLine.Equals(FileHelpers.TestCasesSeparator))
            {
                sw.Restart();
                String thisTestResults = starDelegate(singleTestCase.ToArray(), GlobalConfig.DebugTests);
                sw.Stop();
                testTimings.Add(sw.Elapsed.ToString());
                testResults.Add(thisTestResults);

                singleTestCase.Clear();
            }
            else
            {
                singleTestCase.Add(testLine);
            }
        }
        // if we are here and there are testlines in the single test case, the input file did not end with the separator.
        // so we should execute this last test, which was at the end of the input file.
        if (singleTestCase.Count > 0)
        {
            sw.Restart();
            String thisTestResults = starDelegate(singleTestCase.ToArray(), GlobalConfig.DebugTests);
            sw.Stop();
            testTimings.Add(sw.Elapsed.ToString());
            testResults.Add(thisTestResults);

            singleTestCase.Clear();
        }

        // execute the tests
        sw.Restart();
        String inputResults = starDelegate(this.PuzzleInputs, GlobalConfig.DebugInputs);
        sw.Stop();

        String inputTiming = sw.Elapsed.ToString();


        // print out all the results again at the end
        Console.Out.WriteLine();
        int n = 1;
        for (int t = 0; t < testResults.Count; t++)
        {
            Console.Out.WriteLine($"== Test {n} result: {testResults[t]}");
            if (t < testTimings.Count)
            {
                Console.Out.WriteLine($"    --  in {testTimings[t]}");
            }
            ++n;
        }
        Console.Out.WriteLine($"== Input result: {inputResults}");
        Console.Out.WriteLine($"    --  in {inputTiming}");

    }

    public abstract string Star_1_Impl(String[] inputs, bool debug);

    public void Star_2()
    {
        PrintExecutionHeader(System.Reflection.MethodBase.GetCurrentMethod());

        SpecificStarExecution(FileHelpers.ReadStar2Tests, Star_2_Impl);
    }

    public abstract string Star_2_Impl(String[] inputs, bool debug);
}