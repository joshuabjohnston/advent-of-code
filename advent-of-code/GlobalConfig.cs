using System.Net.NetworkInformation;

namespace org.jjohnston.aoc;

public static class GlobalConfig
{
    public static String AOCAuthCookie { get; set; } = "unknown";

    public static string UserAgent { get; set; } = "unknown";

    public static bool DebugTests { get; set; } = true;

    public static bool DebugInputs { get; set; } = false;
}