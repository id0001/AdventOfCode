namespace AdventOfCode.Lib;

public static class AsyncEnumerableExtensions
{
    public static async IAsyncEnumerable<IEnumerable<T>> ChunkBy<T>(this IAsyncEnumerable<T> source, T separator)
    {
        var list = new List<T>();
        await foreach (var item in source)
        {
            if (!EqualityComparer<T>.Default.Equals(item, separator))
            {
                list.Add(item);
            }
            else
            {
                yield return list;
                list = new List<T>();
            }
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