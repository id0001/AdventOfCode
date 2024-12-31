namespace AdventOfCode.Lib;

public static partial class EnumerableExtensions
{
    public static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1, T2)> source)
        => source.Unzip(x => x.Item1, x => x.Item2);

    public static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(this IEnumerable<(T1 First, T2 Second)> source,
        Func<(T1, T2), T1> selectFirst, Func<(T1, T2), T2> selectSecond)
    {
        List<T1> list1 = [];
        List<T2> list2 = [];
        foreach (var item in source)
        {
            list1.Add(selectFirst(item));
            list2.Add(selectSecond(item));
        }

        return (list1, list2);
    }
}