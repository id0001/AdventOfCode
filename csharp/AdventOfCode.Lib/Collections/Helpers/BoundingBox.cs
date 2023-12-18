using System.Numerics;

namespace AdventOfCode.Lib.Collections.Helpers;

public class BoundingBox<TPoint, TNumber>
    where TPoint : IPoint<TNumber>, new()
    where TNumber : IBinaryInteger<TNumber>, IMinMaxValue<TNumber>
{
    private TNumber[] _min;
    private TNumber[] _max;

    internal BoundingBox()
    {
        Dimensions = new TPoint().Dimensions;
        _min = Enumerable.Repeat(TNumber.MaxValue, Dimensions).ToArray();
        _max = Enumerable.Repeat(TNumber.MinValue, Dimensions).ToArray();
    }

    public int Dimensions { get; }

    public void Inflate(TPoint added)
    {
        for (var d = 0; d < Dimensions; d++)
        {
            if (added[d] < _min[d])
                _min[d] = added[d];

            if (added[d] > _max[d])
                _max[d] = added[d];
        }
    }

    public void Deflate(IEnumerable<TPoint> points, TPoint removed)
    {
        var shouldUpdate = false;
        for (var d = 0; d < Dimensions; d++)
        {
            if (removed[d] != _min[d] && removed[d] != _max[d]) continue;

            shouldUpdate = true;
            break;
        }

        if (!shouldUpdate) return;

        _min = Enumerable.Repeat(TNumber.MaxValue, Dimensions).ToArray();
        _max = Enumerable.Repeat(TNumber.MinValue, Dimensions).ToArray();

        foreach (var point in points) Inflate(point);
    }

    public TNumber GetMin(int dimension)
    {
        if (dimension < 0 || dimension >= Dimensions)
            throw new ArgumentOutOfRangeException(nameof(dimension));

        return _min[dimension];
    }

    public TNumber GetMax(int dimension)
    {
        if (dimension < 0 || dimension >= Dimensions)
            throw new ArgumentOutOfRangeException(nameof(dimension));

        return _max[dimension];
    }

    public bool Contains(IPoint<TNumber> point) => Contains(point, TNumber.Zero);

    public bool Contains(IPoint<TNumber> point, TNumber inflateBy)
    {
        for (var d = 0; d < Dimensions; d++)
            if (point[d] < _min[d] - inflateBy || point[d] > _max[d] + inflateBy)
                return false;

        return true;
    }
}