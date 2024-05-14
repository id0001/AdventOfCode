using Microsoft;

namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T> source)
        {
            var list = source.ToList();

            Requires.NotNullOrEmpty(list, nameof(source));

            var a = Enumerable.Range(0, list.Count).ToArray();

            while (true)
            {
                yield return a.Select(i => list[i]).ToArray();
                var j = list.Count - 1;

                while (j > 0 && a[j - 1] >= a[j])
                    j--;

                if (j == 0)
                    yield break;

                var l = list.Count;
                while (a[j - 1] >= a[l - 1])
                    l--;

                (a[j - 1], a[l - 1]) = (a[l - 1], a[j - 1]);
                Array.Reverse(a, j, list.Count - j);
            }
        }
    }
}
