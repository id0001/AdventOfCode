namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> As<T>(this IEnumerable<T> source)
        where T : IConvertible
        {
            return source.Cast<IConvertible>().Select(x => x.As<T>());
        }
    }
}
