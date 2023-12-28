using System.Collections;
using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib.Collections;

public class CircularBuffer<T>(int capacity) : IEnumerable<T?>
{
    private readonly T?[] _array = new T[capacity];
    private int _head;

    public T? this[int index] => _array[Euclid.Modulus(_head + index, Count)];

    public int Count { get; } = capacity;

    public IEnumerator<T?> GetEnumerator() => _array.Skip(_head).Union(_array.Take(_head)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Push(T item) => _array[GetCurrentPositionAndMoveNext()] = item;

    private int GetCurrentPositionAndMoveNext()
    {
        var p = _head;
        _head = (_head + 1) % Count;
        return p;
    }
}