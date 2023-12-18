using AdventOfCode.Lib.Collections.Helpers;
using System.Collections;
using System.Numerics;

namespace AdventOfCode.Lib.Collections;

public class PointCloud<TPoint, TNumber> : IEnumerable<TPoint> 
    where TPoint : IPoint<TNumber>, new()
    where TNumber : IBinaryInteger<TNumber>, IMinMaxValue<TNumber>
{
    private readonly HashSet<TPoint> _points = new();

    public PointCloud()
    {
    }

    public PointCloud(IEnumerable<TPoint> points)
    {
        foreach (var point in points)
            Set(point);
    }

    public BoundingBox<TPoint,TNumber> Bounds { get; } = new();

    public int Count => _points.Count;

    public IEnumerator<TPoint> GetEnumerator() => _points.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _points.GetEnumerator();

    public void Set(TPoint point)
    {
        if (_points.Add(point))
            Bounds.Inflate(point);
    }

    public void Unset(TPoint point)
    {
        if (_points.Remove(point))
            Bounds.Deflate(_points, point);
    }

    public bool Contains(TPoint point) => _points.Contains(point);
}