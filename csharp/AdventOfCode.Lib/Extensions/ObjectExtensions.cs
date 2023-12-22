namespace AdventOfCode.Lib;

public static class ObjectExtensions
{
    public static T As<T>(this IConvertible source)
        where T : IConvertible
    {
        return (T)Convert.ChangeType(source, typeof(T));
    }
}