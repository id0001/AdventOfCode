using System.Collections;
using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib.Collections;

public class CircularBuffer<T> : IEnumerable<T?>
{
    private readonly T?[] _array;
    private int _head;

    public CircularBuffer(int capacity)
    {
        _array = new T[capacity];
        Count = capacity;
    }

    public T? this[int index] => _array[Euclid.Modulus(_head + index, Count)];

    public int Count { get; }

    public void Push(T item) => _array[GetCurrentPositionAndMoveNext()] = item;

    private int GetCurrentPositionAndMoveNext()
    {
        var p = _head;
        _head = Euclid.Remainder(_head + 1, Count);
        return p;
    }

    public IEnumerator<T?> GetEnumerator() => _array.Skip(_head).Union(_array.Take(_head)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}