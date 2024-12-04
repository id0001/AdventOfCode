using System.Text;
using Microsoft;

namespace AdventOfCode.Lib;

public static class Array2dExtensions
{
    public static Point2 FindPosition<T>(this T[,] source, Func<T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
            if (predicate(source[y, x]))
                return new Point2(x, y);

        throw new InvalidOperationException("No element matching the condition was found");
    }

    public static Point2 FindPosition<T>(this T[,] source, Func<Point2, T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
        {
            var p = new Point2(x, y);
            if (predicate(p, source[y, x]))
                return p;
        }

        throw new InvalidOperationException("No element matching the condition was found");
    }

    public static IEnumerable<Point2> Where<T>(this T[,] source, Func<Point2, T, bool> predicate)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
        {
            var p = new Point2(x, y);
            if (predicate(p, source[y, x]))
                yield return p;
        }
    }

    public static IEnumerable<KeyValuePair<Point2, T>> AsEnumerable<T>(this T[,] source)
    {
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
        {
            var p = new Point2(x, y);
            yield return new KeyValuePair<Point2, T>(p, source[y, x]);
        }
    }

    public static Rectangle Bounds<T>(this T[,] source) => new(0, 0, source.GetLength(1), source.GetLength(0));

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

    public static int Count<T>(this T[,] source, Func<T, bool> predicate)
    {
        var c = 0;
        for (var y = 0; y < source.GetLength(0); y++)
        for (var x = 0; x < source.GetLength(1); x++)
            if (predicate(source[y, x]))
                c++;

        return c;
    }

    public static IEnumerable<T> GetRow<T>(this T[,] source, int row)
    {
        for (var i = 0; i < source.GetLength(1); i++)
            yield return source[row, i];
    }

    public static IEnumerable<T> GetColumn<T>(this T[,] source, int column)
    {
        for (var i = 0; i < source.GetLength(0); i++)
            yield return source[i, column];
    }

    public static int GetDiagonalLength<T>(this T[,] source) => source.GetLength(0) + source.GetLength(1);

    public static IEnumerable<T> GetDownRightDiagonal<T>(this T[,] source, int diagonalRow)
    {
        var width = source.GetLength(1);
        var height = source.GetLength(0);

        if (diagonalRow < height)
        {
            var y = height - diagonalRow - 1;
            var x = 0;

            while (x < width && y < height)
            {
                yield return source[y, x];
                x++;
                y++;
            }
        }
        else if (diagonalRow >= height)
        {
            var x = diagonalRow - (height - 1);
            var y = 0;

            while (x < width && y < height)
            {
                yield return source[y, x];
                x++;
                y++;
            }
        }
    }

    public static IEnumerable<T> GetDownLeftDiagonal<T>(this T[,] source, int diagonalRow)
    {
        var width = source.GetLength(1);
        var height = source.GetLength(0);

        if (diagonalRow < width)
        {
            var y = 0;
            var x = diagonalRow;

            while (x >= 0 && y < height)
            {
                yield return source[y, x];
                x--;
                y++;
            }
        }
        else if (diagonalRow >= width)
        {
            var x = width - 1;
            var y = diagonalRow - (width - 1);

            while (x >= 0 && y < height)
            {
                yield return source[y, x];
                x--;
                y++;
            }
        }
    }

    public static T[][] ToJaggedArray<T>(this T[,] source)
    {
        Requires.NotNull(source, nameof(source));
        Requires.NotNullOrEmpty(source, nameof(source));

        var result = new T[source.GetLength(0)][];
        for (var y = 0; y < source.GetLength(0); y++)
        {
            result[y] = new T[source.GetLength(1)];
            for (var x = 0; x < source.GetLength(1); x++)
                result[y][x] = source[y, x];
        }

        return result;
    }

    public static T[,] To2dArray<T>(this T[][] source)
    {
        Requires.NotNull(source, nameof(source));
        Requires.Argument(source.Length > 0 && source[0].Length > 0, nameof(source), "Array cannot be empty");
        Requires.Argument(source.DistinctBy(col => col.Length).Count() == 1, nameof(source),
            "All columns must be of equal length");

        var result = new T[source.Length, source[0].Length];
        for (var y = 0; y < source.Length; y++)
        for (var x = 0; x < source[y].Length; x++)
            result[y, x] = source[y][x];

        return result;
    }

    public static IEnumerable<T[,]> Windowed<T>(this T[,] source, int width, int height)
    {
        for (var y = 0; y < source.GetLength(0) - (height - 1); y++)
        for (var x = 0; x < source.GetLength(1) - (width - 1); x++)
        {
            var window = new T[height, width];
            for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
                window[j, i] = source[y + j, x + i];

            yield return window;
        }
    }
}