namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static int Product(this IEnumerable<int> source) => source.Aggregate(1, (a, b) => a * b);

        public static long Product(this IEnumerable<long> source) => source.Aggregate(1L, (a, b) => a * b);
    }
}
