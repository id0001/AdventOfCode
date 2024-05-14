namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source)
        {
            IEnumerator<T> enumerator = null!;

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
    }
}
