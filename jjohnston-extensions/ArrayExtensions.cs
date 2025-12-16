using System.Diagnostics.Tracing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace jjohnston_extensions;

public static class ArrayExtensions
{
    public static int[] ParseInts(this string[] strings)
    {
        int[] ints = new int[strings.Length];

        for (int i = 0; i < strings.Length; i++)
        {
            ints[i] = int.Parse(strings[i]);
        }

        return ints;
    }

    public static T[] RemoveAt<T>(this T[] array, int idx)
    {
        if (idx < 0) throw new ArgumentOutOfRangeException($"idx ({idx}) < 0");
        if (idx >= array.Length) throw new ArgumentOutOfRangeException($"idx ({idx} beyond length of array ({array.Length}))");

        T[] newArr = new T[array.Length - 1];
        int newIdx = 0;
        for (int j = 0; j < array.Length; j++)
        {
            if (j == idx)
            {
                continue;
            }
            newArr[newIdx] = array[j];
            ++newIdx;
        }

        return newArr;
    }

    public static void Swap<T>(this T[] array, int i, int j)
    {
        T tmp = array[i];
        array[i] = array[j];
        array[j] = tmp;
    }

    public static String GetString<T>(this T[] a)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < a.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }
            T t = a[i];
            if (t != null)
            {
                sb.Append(t.ToString());
            }
            else
            {
                throw new ArgumentNullException("null element in array");
            }
        }

        return sb.ToString();
    }

    public static char[][] ToCharMatrix(this String[] strings)
    {
        char[][] matrix = new char[strings.Length][];

        for (int r = 0; r < strings.Length; r++)
        {
            matrix[r] = new char[strings[r].Length];

            for (int c = 0; c < strings[r].Length; c++)
            {
                matrix[r][c] = strings[r][c];
            }
        }

        return matrix;
    }

}
