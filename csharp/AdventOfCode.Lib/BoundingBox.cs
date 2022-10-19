namespace AdventOfCode.Lib;

public class BoundingBox<T> where T : IPoint, new()
{
    private int[] _min;
    private int[] _max;

    public BoundingBox()
    {
        Dimensions = new T().Dimensions;
        _min = Enumerable.Repeat(int.MaxValue, Dimensions).ToArray();
        _max = Enumerable.Repeat(int.MinValue, Dimensions).ToArray();
    }

    public int Dimensions { get; }

    public void Inflate(T added)
    {
        for (var d = 0; d < Dimensions; d++)
        {
            if (added[d] < _min[d])
                _min[d] = added[d];

            if (added[d] > _max[d])
                _max[d] = added[d];
        }
    }

    public void Deflate(IEnumerable<T> points, T removed)
    {
        var shouldUpdate = false;
        for (var d = 0; d < Dimensions; d++)
        {
            if (removed[d] != _min[d] && removed[d] != _max[d]) continue;

            shouldUpdate = true;
            break;
        }

        if (!shouldUpdate) return;

        _min = Enumerable.Repeat(int.MaxValue, Dimensions).ToArray();
        _max = Enumerable.Repeat(int.MinValue, Dimensions).ToArray();

        foreach (var point in points) Inflate(point);
    }

    public int GetMin(int dimension)
    {
        if (dimension < 0 || dimension >= Dimensions)
            throw new ArgumentOutOfRangeException(nameof(dimension));

        return _min[dimension];
    }

    public int GetMax(int dimension)
    {
        if (dimension < 0 || dimension >= Dimensions)
            throw new ArgumentOutOfRangeException(nameof(dimension));

        return _max[dimension] + 1;
    }

    public bool Contains(IPoint point)
    {
        for (var d = 0; d < Dimensions; d++)
            if (point[d] < _min[d] || point[d] > _max[d])
                return false;

        return true;
    }
}