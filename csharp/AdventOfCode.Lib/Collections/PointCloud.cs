using AdventOfCode.Lib.Collections.Helpers;
using System.Collections;
using System.Numerics;

namespace AdventOfCode.Lib.Collections;

public class PointCloud<T, TNumber> : IEnumerable<T> 
    where T : IPoint<TNumber>, new()
    where TNumber : IBinaryInteger<TNumber>, IMinMaxValue<TNumber>
{
    private readonly HashSet<T> _points = new();

    public PointCloud()
    {
    }

    public PointCloud(IEnumerable<T> points)
    {
        foreach (var point in points)
            Set(point);
    }

    public BoundingBox<T,TNumber> Bounds { get; } = new();

    public int Count => _points.Count;

    public IEnumerator<T> GetEnumerator() => _points.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

    public void Set(T point)
    {
        if (_points.Add(point))
            Bounds.Inflate(point);
    }

    public void Unset(T point)
    {
        if (_points.Remove(point))
            Bounds.Deflate(_points, point);
    }

    public bool Contains(T point) => _points.Contains(point);
}