namespace AdventOfCode.Lib.Collections;

public class SparseSpatialMap<TKey, TValue> : Dictionary<TKey, TValue> where TKey : IPoint, new()
{
    public BoundingBox<TKey> Bounds { get; } = new();

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        if (!Bounds.Contains(key))
            Bounds.Inflate(key);
    }

    public void AddOrUpdate(TKey key, TValue value)
    {
        if (!ContainsKey(key))
            Add(key, value);
        else
            this[key] = value;
    }

    public new void Remove(TKey key)
    {
        if (base.Remove(key))
            Bounds.Deflate(Keys, key);
    }

    public TValue GetValue(TKey p, TValue defaultValue) => !ContainsKey(p) ? defaultValue : this[p];

    public IEnumerable<KeyValuePair<TKey, TValue?>> GetNeighbors(TKey p, bool includeDiagonal = false)
    {
        foreach (TKey neighbor in p.GetNeighbors(includeDiagonal))
            if (TryGetValue(neighbor, out var value))
                yield return new KeyValuePair<TKey, TValue?>(neighbor, value);
    }
}