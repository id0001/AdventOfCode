﻿namespace AdventOfCode.Lib;

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
}