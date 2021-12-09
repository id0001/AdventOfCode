using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib.Extensions
{
    public static class EnumerableExtensions
    {
        public static ulong Sum<T>(this IEnumerable<T> source, Func<T, ulong> selector)
        {
            ulong sum = 0;
            foreach (var item in source)
                sum += selector(item);

            return sum;
        }

        public static int Product<T>(this IEnumerable<T> source, Func<T, int> selector) => MathEx.Product(source.Select(selector).ToArray());

        public static int Product(this IEnumerable<int> source) => MathEx.Product(source.ToArray());

        public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source, int start, int end)
        {
            if (end < start)
                throw new ArgumentException("End must be equal or higher than start.");

            var list = new List<T[]>();
            GeneratePermutationsInternal(list, source.ToArray(), start, end);
            return list;
        }

        private static void GeneratePermutationsInternal<T>(List<T[]> perms, T[] source, int start, int end)
        {
            if (end == start)
            {
                T[] perm = new T[source.Length];
                Array.Copy(source, 0, perm, 0, source.Length);
                perms.Add(perm);
            }
            else
            {
                for (int i = start; i <= end; i++)
                {
                    (source[start], source[i]) = (source[i], source[start]);
                    GeneratePermutationsInternal(perms, source, start + 1, end);
                    (source[start], source[i]) = (source[i], source[start]);
                }
            }
        }
    }
}
