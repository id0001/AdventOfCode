using System.Collections;

namespace AdventOfCode.Lib.Collections;

public class SparseSpatialMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IPoint, new()
{
    private readonly Dictionary<TKey, TValue?> _map = new();

    public BoundingBox<TKey> Bounds { get; } = new();

    public TValue? this[TKey key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    public int Count => _map.Count;

    public void Set(TKey key, TValue? value)
    {
        bool unset = EqualityComparer<TValue>.Default.Equals(value, default);

        if (!_map.ContainsKey(key) && !unset)
            Add(key, value);
        else if (_map.ContainsKey(key) && unset)
            Unset(key);
        else if (_map.ContainsKey(key))
            Update(key, value);
    }

    public void Unset(TKey key)
    {
        if (_map.Remove(key))
            Bounds.Deflate(_map.Keys, key);
    }

    public TValue? Get(TKey p) => !_map.ContainsKey(p) ? default : _map[p];

    public TValue? Get(TKey p, TValue defaultValue) => !_map.ContainsKey(p) ? defaultValue : _map[p];

    public IEnumerable<KeyValuePair<TKey, TValue?>> GetNeighbors(TKey p, bool includeDiagonal = false)
    {
        foreach (TKey neighbor in p.GetNeighbors(includeDiagonal))
            if (_map.TryGetValue(neighbor, out var value))
                yield return new KeyValuePair<TKey, TValue?>(neighbor, value);
    }

    public bool ContainsKey(TKey key) => _map.ContainsKey(key);

    private void Add(TKey key, TValue? value)
    {
        _map.Add(key, value);
        if (!Bounds.Contains(key))
            Bounds.Inflate(key);
    }

    private void Update(TKey key, TValue? value)
    {
        _map[key] = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();
}