namespace AdventOfCode.Lib
{
    public static partial class Array2DExtensions
    {
        public static IEnumerable<KeyValuePair<Point2, T>> AsEnumerable<T>(this T[,] source)
        {
            for (var y = 0; y < source.GetLength(0); y++)
                for (var x = 0; x < source.GetLength(1); x++)
                {
                    var p = new Point2(x, y);
                    yield return new KeyValuePair<Point2, T>(p, source[y, x]);
                }
        }
    }
}
