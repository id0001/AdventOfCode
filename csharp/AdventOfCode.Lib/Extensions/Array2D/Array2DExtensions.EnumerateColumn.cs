namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static IEnumerable<T> EnumerateColumn<T>(this T[,] source, int column)
        {
            for (var i = 0; i < source.GetLength(0); i++)
                yield return source[i, column];
        }

        public static IEnumerable<IEnumerable<T>> EnumerateColumns<T>(this T[,] source)
        {
            for (var col = 0; col < source.GetLength(0); col++)
                yield return source.EnumerateColumn(col);
        }
    }
}
