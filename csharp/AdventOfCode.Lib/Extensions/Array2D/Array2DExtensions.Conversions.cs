using Microsoft;

namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
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
    }
}
