namespace org.jjohnston.extensions;

public static class DictionaryExtensions
{
    public static void AddValueToKey(this Dictionary<long, long> dict, long key, long valToAdd)
    {
        long curVal = 0;
        dict.TryGetValue(key, out curVal);
        dict[key] = curVal + valToAdd;
    }
}