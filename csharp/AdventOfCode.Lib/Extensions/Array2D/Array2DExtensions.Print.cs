using System.Text;

namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
    public static void PrintToConsole<T>(this T[,] source, Func<Point2, T, char> selector)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                var p = new Point2(x, y);
                var c = selector(p, source[y, x]);
                Console.Write(c);
                Console.ResetColor();
            }

            Console.WriteLine();
        }
    }

    public static async Task PrintToFileAsync<T>(this T[,] source, string path, Func<Point2, T, char> selector)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                var p = new Point2(x, y);
                var c = selector(p, source[y, x]);
                sb.Append(c);
            }

            sb.AppendLine();
        }

        await File.WriteAllTextAsync(path, sb.ToString());
    }

    public static string PrintToString<T>(this T[,] source, Func<Point2, T, char> selector)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < source.GetLength(0); y++)
        {
            for (var x = 0; x < source.GetLength(1); x++)
            {
                var p = new Point2(x, y);
                var c = selector(p, source[y, x]);
                sb.Append(c);
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}