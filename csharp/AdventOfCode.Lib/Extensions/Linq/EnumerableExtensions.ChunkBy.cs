using System.ComponentModel;

namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, T separator)
        {
            var list = new List<T>();
            foreach (var item in source)
                if (!EqualityComparer<T>.Default.Equals(item, separator))
                {
                    list.Add(item);
                }
                else
                {
                    yield return list;
                    list = [];
                }
        }

        public static async IAsyncEnumerable<IEnumerable<T>> ChunkBy<T>(this IAsyncEnumerable<T> source, T separator)
        {
            var list = new List<T>();
            await foreach (var item in source)
                if (!EqualityComparer<T>.Default.Equals(item, separator))
                {
                    list.Add(item);
                }
                else
                {
                    yield return list;
                    list = [];
                }
        }

        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool matchIsLastItem = false)
        {
            var list = new List<T>();

            bool skipFirst = true;
            foreach (var item in source)
            {
                if(matchIsLastItem)
                {
                    list.Add(item);
                    var isMatch = predicate(item);
                    if (isMatch)
                    {
                        yield return list;
                        list = [];
                    }
                }
                else
                {
                    var isMatch = predicate(item);
                    if (isMatch)
                    {
                        if (!skipFirst)
                            yield return list;

                        list = [];
                        skipFirst = false;
                    }

                    list.Add(item);
                }
            }
        }

        public static async IAsyncEnumerable<IEnumerable<T>> ChunkBy<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate, bool matchIsLastItem = false)
        {
            var list = new List<T>();

            bool skipFirst = true;
            await foreach (var item in source)
            {
                if (matchIsLastItem)
                {
                    list.Add(item);
                    var isMatch = predicate(item);
                    if (isMatch)
                    {
                        yield return list;
                        list = [];
                    }
                }
                else
                {
                    var isMatch = predicate(item);
                    if (isMatch)
                    {
                        if (!skipFirst)
                            yield return list;

                        list = [];
                        skipFirst = false;
                    }

                    list.Add(item);
                }
            }
        }
    }
}
