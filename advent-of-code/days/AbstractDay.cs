using System.Reflection;
using System.Security.Cryptography;

namespace org.jjohnston.aoc.days;

public abstract class AbstractDay : IDay
{

    public String Day {get; protected set;}

    public String Year {get; protected set;}

    public String[] PuzzleInputs {get; protected set;}


    public AbstractDay(string day, string year)
    {
        this.Day = day;
        this.Year = year;

        FileHelpers.EnsureExistsStarTestFiles(day, year);
        this.PuzzleInputs = FileHelpers.ReadWithFetchPuzzleInputs(day, year);
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
        List<String> testResults = new List<string>();
        // load all the tests and execute them.
        String[] allTestLines = fileDelegate(this.Day, this.Year);
        
        List<String> singleTestCase = new List<string>();
        foreach (string testLine in allTestLines)
        {
            if (testLine.Equals(FileHelpers.TestCasesSeparator))
            {
                String thisTestResults = starDelegate(singleTestCase.ToArray(), GlobalConfig.DebugTests);
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
            String thisTestResults = starDelegate(singleTestCase.ToArray(), GlobalConfig.DebugTests);
            testResults.Add(thisTestResults);

            singleTestCase.Clear();
        }

        // execute the tests
        String inputResults = starDelegate(this.PuzzleInputs, GlobalConfig.DebugInputs);


        // print out all the results again at the end
        Console.Out.WriteLine();
        int n = 1;
        foreach (String res in testResults)
        {
            Console.Out.WriteLine($"== Test {n} result: {res}");
            ++n;
        }
        Console.Out.WriteLine($"== Input result: {inputResults}");
    }

    public abstract string Star_1_Impl(String[] inputs, bool debug);

    public void Star_2()
    {
        PrintExecutionHeader(System.Reflection.MethodBase.GetCurrentMethod());

        SpecificStarExecution(FileHelpers.ReadStar2Tests, Star_2_Impl);
    }

    public abstract string Star_2_Impl(String[] inputs, bool debug);
}