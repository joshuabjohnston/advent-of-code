namespace org.jjohnston.aoc;

public static class FileHelpers
{
    public static readonly String BasePuzzleInputsDirectory = "puzzleInputs";
    public static readonly String TestCasesSeparator = "/** BREAK TEST CASES HERE **/";
    public static readonly String InputsFileExtension = ".input";

    public static readonly String Star1TestFile = "star_1.tests";
    public static readonly String Star2TestFile = "star_2.tests";

    public static string EnsureExistsInputsDirectories(string day, string year)
    {
        string dir = Path.Combine(BasePuzzleInputsDirectory, year, $"day{day}");

        if (!Directory.Exists(dir))
        {
            Console.Out.WriteLine($"Creating directory {dir}");
            Directory.CreateDirectory(dir);
        }

        return dir;
    }

    public static void EnsureExistsStarTestFiles(string day, string year)
    {
        EnsureExistsSpecificStarTestFile(day, year, FileHelpers.Star1TestFile);
        EnsureExistsSpecificStarTestFile(day, year, FileHelpers.Star2TestFile);
    }

    public static String[] ReadStar1Tests(string day, string year)
    {
        return ReadSpecificStarTests(day, year, FileHelpers.Star1TestFile);
    }

    public static String[] ReadStar2Tests(string day, string year)
    {
        return ReadSpecificStarTests(day, year, FileHelpers.Star2TestFile);
    }

    private static String[] ReadSpecificStarTests(string day, string year, string starTestFile)
    {
        String dir = EnsureExistsInputsDirectories(day, year);
        String starFile = Path.Combine(dir, starTestFile);
        return File.ReadAllLines(starFile);
    }

    private static void EnsureExistsSpecificStarTestFile(String day, String year, String specificTestFile)
    {
        String dir = EnsureExistsInputsDirectories(day, year);
        String testFile = Path.Combine(dir, specificTestFile);

        if (!File.Exists(testFile))
        {
            Console.Out.WriteLine($"! exists {testFile}, creating template");
            using (StreamWriter fileWriter = new StreamWriter(testFile))
            {
                fileWriter.WriteLine("--- first case here ---");
                fileWriter.WriteLine(FileHelpers.TestCasesSeparator);
            }
        }
    }

    public static String[] ReadWithFetchPuzzleInputs(string day, string year)
    {
        string dir = EnsureExistsInputsDirectories(day, year);

        string inputFileName = Path.Combine(dir, $"day{day}{FileHelpers.InputsFileExtension}");

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
                client.DefaultRequestHeaders.Add("Cookie", GlobalConfig.AOCAuthCookie);
                client.DefaultRequestHeaders.Add("User-Agent", GlobalConfig.UserAgent);

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
}