namespace AdventOfCode.Lib;

public static class DictionaryExtensions
{
    public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value) where TKey : notnull
    {
        if (!source.TryAdd(key, value))
            source[key] = value;
    }
    
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue?> source, TKey key, Func<TValue?, TValue> update, TValue? defaultValue = default)
    {
        if (!source.TryGetValue(key, out TValue? oldValue))
        {
            source.Add(key, update(defaultValue));
        }
        else
        {
            source[key] = update(oldValue);
        }
    }
}