namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
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
    }
}
