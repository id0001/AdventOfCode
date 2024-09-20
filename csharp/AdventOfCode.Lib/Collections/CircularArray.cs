using System.Collections;

namespace AdventOfCode.Lib.Collections;

public class CircularArray<T>(int size) : IEnumerable<T>
{
    private readonly T[] _array = new T[size];

    public CircularArray(T[] source) : this(source.Length)
    {
        Array.Copy(source, _array, source.Length);
    }

    public T this[int index]
    {
        get => _array[index.Mod(size)];
        set => _array[index.Mod(size)] = value;
    }

    public int Length => size;

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _array)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyTo(T[] destination, int from, int length)
    {
        for (var i = 0; i < length; i++)
            destination[i] = this[from + i];
    }
}