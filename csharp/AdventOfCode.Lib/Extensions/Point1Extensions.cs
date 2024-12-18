namespace AdventOfCode.Lib
{
    public static class Point1Extensions
    {
        public static IEnumerable<Point1> GetNeighbors(this Point1 source)
        {
            yield return source.Left;
            yield return source.Right;
        }
    }
}
