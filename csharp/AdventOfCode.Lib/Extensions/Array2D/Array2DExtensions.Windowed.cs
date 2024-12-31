namespace AdventOfCode.Lib;

public static partial class Array2DExtensions
{
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