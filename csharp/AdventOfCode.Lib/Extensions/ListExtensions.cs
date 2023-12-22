using System.Collections;

namespace AdventOfCode.Lib;

public static class ListExtensions
{
    public static TOut Transform<TIn, TOut>(this IList<TIn> source, Func<IList<TIn>, TOut> selector) =>
        selector(source);

    public static T First<T>(this IList<T> source) => source[0];
    
    public static T Second<T>(this IList<T> source) => source[1];
    
    public static T Third<T>(this IList<T> source) => source[2];
    
    public static IList<T> As<T>(this IList source)
        where T : IConvertible
    {
        return source.Cast<IConvertible>().Select(x => x.As<T>()).ToList();
    }
}