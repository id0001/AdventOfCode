using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Lib.Collections;

public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue?>
    where TKey : notnull
{
    private readonly OrderedDictionary _dict = new();

    public KeyValuePair<TKey, TValue?> this[int index]
    {
        get
        {
            if (_dict.Count <= index)
                throw new IndexOutOfRangeException();

            var entry = (DictionaryEntry) _dict[index]!;
            return new KeyValuePair<TKey, TValue?>((TKey) entry.Key, (TValue?) entry.Value);
        }

        set
        {
            if (_dict.Count <= index)
                throw new IndexOutOfRangeException();

            _dict[index] = new DictionaryEntry(value.Key, value.Value);
        }
    }

    public TValue? this[TKey key]
    {
        get => (TValue?) _dict[key];
        set => _dict[key] = value;
    }

    public ICollection<TKey> Keys => _dict.Keys.Cast<TKey>().ToList();

    public ICollection<TValue?> Values => _dict.Values.Cast<TValue?>().ToList();

    public int Count => _dict.Count;

    public bool IsReadOnly => _dict.IsReadOnly;

    public void Add(TKey key, TValue? value) => _dict.Add(key, value);

    public void Add(KeyValuePair<TKey, TValue?> item) => _dict.Add(item.Key, item.Value);

    public void Clear() => _dict.Clear();

    public bool Contains(KeyValuePair<TKey, TValue?> item) => _dict.Contains(item.Key) &&
                                                              EqualityComparer<TValue?>.Default.Equals(
                                                                  (TValue?) _dict[item.Key], item.Value);

    public bool ContainsKey(TKey key) => _dict.Contains(key);

    public void CopyTo(KeyValuePair<TKey, TValue?>[] array, int arrayIndex) => _dict.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue?>> GetEnumerator() => new Enumerator(_dict.GetEnumerator());

    public bool Remove(TKey key)
    {
        if (ContainsKey(key))
            return false;

        _dict.Remove(key);
        return true;
    }

    public bool Remove(KeyValuePair<TKey, TValue?> item)
    {
        if (!Contains(item))
            return false;

        _dict.Remove(item.Key);
        return true;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue? value)
    {
        value = default;
        if (!ContainsKey(key))
            return false;

        value = this[key];
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

    private class Enumerator(IDictionaryEnumerator enumerator) : IEnumerator<KeyValuePair<TKey, TValue?>>
    {
        public KeyValuePair<TKey, TValue?> Current { get; private set; }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (!enumerator.MoveNext())
                return false;

            Current = new KeyValuePair<TKey, TValue?>((TKey) enumerator.Key, (TValue?) enumerator.Value);
            return true;
        }

        public void Reset()
        {
            enumerator.Reset();
            Current = default;
        }
    }
}