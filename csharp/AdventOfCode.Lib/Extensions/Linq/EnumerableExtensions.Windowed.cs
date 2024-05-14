namespace AdventOfCode.Lib
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IList<T>> Windowed<T>(this IEnumerable<T> source, int windowSize)
        {
            var windows = Enumerable.Range(0, windowSize)
                .Select(_ => new List<T>())
                .ToList();

            var i = 0;
            using var iter = source.GetEnumerator();
            while (iter.MoveNext())
            {
                var c = System.Math.Min(i + 1, windowSize);
                for (var j = 0; j < c; j++)
                    windows[(i - j) % windowSize].Add(iter.Current);

                if (i >= windowSize - 1)
                {
                    var previous = (i + 1) % windowSize;
                    yield return windows[previous];
                    windows[previous] = new List<T>();
                }

                i++;
            }
        }

        public static async IAsyncEnumerable<IList<T>> Windowed<T>(this IAsyncEnumerable<T> source, int windowSize)
        {
            var windows = Enumerable.Range(0, windowSize)
                .Select(_ => new List<T>())
                .ToList();

            var i = 0;
            await using var iter = source.GetAsyncEnumerator();
            while (await iter.MoveNextAsync())
            {
                var c = System.Math.Min(i + 1, windowSize);
                for (var j = 0; j < c; j++)
                    windows[(i - j) % windowSize].Add(iter.Current);

                if (i >= windowSize - 1)
                {
                    var previous = (i + 1) % windowSize;
                    yield return windows[previous];
                    windows[previous] = new List<T>();
                }

                i++;
            }
        }
    }
}
