namespace AdventOfCode.Lib;

public static class ObjectExtensions
{
    public static T As<T>(this IConvertible source)
        where T : notnull, IConvertible
    {
        return (T)Convert.ChangeType(source, typeof(T));
    }

    public static TOut Into<TIn, TOut>(this TIn source, Func<TIn, TOut> converter)
        where TIn : notnull
        where TOut : notnull
        => converter(source);
}