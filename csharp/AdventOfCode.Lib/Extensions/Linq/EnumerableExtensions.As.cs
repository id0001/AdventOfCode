﻿using Microsoft;
using System.Collections;
using System.Text;

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

        public static string AsString(this IEnumerable<char> source)
        {
            Requires.NotNull(source, nameof(source));

            var sb = new StringBuilder();
            foreach(var c in source)
                sb.Append(c);

            return sb.ToString();
        }
    }
}
