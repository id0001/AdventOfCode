// ReSharper disable All
namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
        {
            IEnumerator<T>? enumerator = null;

            try
            {
                enumerator = source.GetEnumerator();

                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        enumerator = source.GetEnumerator();
                        continue;
                    }

                    yield return enumerator.Current;
                }
            }
            finally
            {
                enumerator?.Dispose();
            }
        }

        public static async IAsyncEnumerable<T> Cycle<T>(this IAsyncEnumerable<T> source)
        {
            IAsyncEnumerator<T>? enumerator = null;

            try
            {
                enumerator = source.GetAsyncEnumerator();

                while (true)
                {
                    if (!(await enumerator.MoveNextAsync()))
                    {
                        await enumerator.DisposeAsync();
                        enumerator = source.GetAsyncEnumerator();
                        continue;
                    }

                    yield return enumerator.Current;
                }
            }
            finally
            {
                if (enumerator is not null)
                    await enumerator.DisposeAsync();
            }
        }
    }
}
