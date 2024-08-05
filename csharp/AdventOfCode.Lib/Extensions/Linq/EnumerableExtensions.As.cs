using Microsoft;
using System.Collections;

namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IList<T> As<T>(this IList source)
        where T : IConvertible
        {
            Requires.NotNull(source, nameof(source));

            return source.Cast<IConvertible>().Select(x => x.As<T>()).ToList();
        }

        public static IEnumerable<T> As<T>(this IEnumerable<T> source)
        where T : IConvertible
        {
            Requires.NotNull(source, nameof(source));

            return source.Cast<IConvertible>().Select(x => x.As<T>());
        }
    }
}
