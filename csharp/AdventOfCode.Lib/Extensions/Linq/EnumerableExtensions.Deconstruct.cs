using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void Deconstruct<T>(this IEnumerable<T> source, out T? first, out IEnumerable<T> rest)
        {
            first = source.FirstOrDefault();
            rest = source.Skip(1);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void Deconstruct<T>(this IEnumerable<T> source, out T? first, out T? second, out IEnumerable<T> rest)
        {
            first = source.FirstOrDefault();
            (second, rest) = source.Skip(1);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static void Deconstruct<T>(this IEnumerable<T> source, out T? first, out T? second, out T? third,
            out IEnumerable<T> rest)
        {
            first = source.FirstOrDefault();
            (second, third, rest) = source.Skip(1);
        }
    }
}
