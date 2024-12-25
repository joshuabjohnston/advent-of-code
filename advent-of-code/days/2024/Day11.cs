using System.Runtime.CompilerServices;
using jjohnston_extensions;
using org.jjohnston.aoc.days;
using org.jjohnston.extensions;

namespace org.jjohnston.aoc.year2024;

public class Day11 : AbstractDay
{
    public class Stone
    {
        public long Value { get; set; }
        public LinkedListNode<Stone>? Node { get; set; } = null;

        public Stone(long value)
        {
            this.Value = value;
        }
    }

    public class PlutonianStones
    {
        public LinkedList<Stone> Stones { get; set; } = new LinkedList<Stone>();

        public PlutonianStones(string line)
        {
            String[] tokens = line.Split(" ");
            foreach (String token in tokens)
            {
                Stone s = new Stone(int.Parse(token));
                s.Node = Stones.AddLast(s);
            }
        }

        public void Blink()
        {
            LinkedListNode<Stone>? curNode = Stones.First;
            while (curNode != null)
            {
                Stone curStone = curNode.Value;
                string curValStr = curStone.Value.ToString();

                // if 0, replace with 1
                if (curStone.Value == 0)
                {
                    curStone.Value = 1;
                }
                // if even number of digits, split digits in half across 2 stones. don't iterate on the new stone.
                else if (curValStr.Length % 2 == 0)
                {
                    String first = curValStr.Substring(0, curValStr.Length / 2);
                    String second = curValStr.Substring(curValStr.Length / 2);

                    curStone.Value = long.Parse(first);
                    Stone newStone = new Stone(long.Parse(second));
                    newStone.Node = Stones.AddAfter(curNode, newStone);

                    curNode = newStone.Node;
                }
                // else * 2024
                else
                {
                    curStone.Value *= 2024;
                }

                curNode = curNode.Next;
            }
        }

        public void Print()
        {
            bool first = true;
            Console.Out.Write("Stones :: [");
            foreach (Stone s in Stones)
            {
                if (!first) Console.Out.Write(", ");
                Console.Out.Write(s.Value);
                first = false;
            }
            Console.Out.WriteLine("]");
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        Dictionary<long, long> buckets = new Dictionary<long, long>();
        // init the buckets
        if (debug) Console.Out.WriteLine(inputs[0]);
        int[] initVals = inputs[0].Split(" ").ParseInts();
        foreach (int val in initVals)
        {
            if (!buckets.ContainsKey(val))
            {
                buckets.Add(val, 1);
            }
            else
            {
                long n = buckets[val];
                buckets[val] = n + 1;
            }
        }
        if (debug) PrintDict(buckets);

        int blinks = 25;
        for (int b = 0; b < blinks; b++)
        {
            Dictionary<long, long> newBuckets = new Dictionary<long, long>();

            foreach (long key in buckets.Keys)
            {
                string keyString = key.ToString();
                long count = buckets[key];

                // 0 --> 1
                if (key == 0)
                {
                    newBuckets.AddValueToKey(1, count);
                }
                // else if even number of digits, split in two
                else if (keyString.Length % 2 == 0)
                {
                    String first = keyString.Substring(0, keyString.Length / 2);
                    String second = keyString.Substring(keyString.Length / 2);

                    long lf = long.Parse(first);
                    long ls = long.Parse(second);

                    newBuckets.AddValueToKey(lf, count);
                    newBuckets.AddValueToKey(ls, count);
                }
                // else * 2024
                else
                {
                    long newKey = key * 2024;
                    newBuckets.AddValueToKey(newKey, count);
                }
            }

            buckets = newBuckets;

            if (debug)
            {
                Console.Out.WriteLine($"after blink {b}");
                PrintDict(buckets);
            }
        }

        // count up the number of stones in the buckes
        long stoneCount = 0;
        foreach (long key in buckets.Keys)
        {
            stoneCount += buckets[key];
        }

        return $"after {blinks} blinks there are {stoneCount} stones";
    }

    public void PrintDict(Dictionary<long, long> dict)
    {
        foreach (long key in dict.Keys)
        {
            Console.Out.WriteLine($"  -- buckets[{key}] = {dict[key]}");
        }
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        Dictionary<long, long> buckets = new Dictionary<long, long>();
        // init the buckets
        int[] initVals = inputs[0].Split(" ").ParseInts();
        foreach (int val in initVals)
        {
            if (!buckets.ContainsKey(val))
            {
                buckets.Add(val, 1);
            }
            else
            {
                long n = buckets[val];
                buckets[val] = n + 1;
            }
        }

        int blinks = 75;
        for (int b = 0; b < blinks; b++)
        {
            Dictionary<long, long> newBuckets = new Dictionary<long, long>();

            foreach (long key in buckets.Keys)
            {
                string keyString = key.ToString();
                long count = buckets[key];

                // 0 --> 1
                if (key == 0)
                {
                    newBuckets.AddValueToKey(1, count);
                }
                // else if even number of digits, split in two
                else if (keyString.Length % 2 == 0)
                {
                    String first = keyString.Substring(0, keyString.Length / 2);
                    String second = keyString.Substring(keyString.Length / 2);

                    long lf = long.Parse(first);
                    long ls = long.Parse(second);

                    newBuckets.AddValueToKey(lf, count);
                    newBuckets.AddValueToKey(ls, count);
                }
                // else * 2024
                else
                {
                    long newKey = key * 2024;
                    newBuckets.AddValueToKey(newKey, count);
                }
            }

            buckets = newBuckets;
        }

        // count up the number of stones in the buckes
        long stoneCount = 0;
        foreach (long key in buckets.Keys)
        {
            stoneCount += buckets[key];
        }

        return $"after {blinks} blinks there are {stoneCount} stones";
    }
}
