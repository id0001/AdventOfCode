namespace AdventOfCode.Lib
{
    public readonly record struct LongPoint2(long X, long Y) : IPoint<long>
    {
        long IPoint<long>.this[int index] => index switch
        {
            0 => X,
            1 => Y,
            _ => throw new ArgumentOutOfRangeException(nameof(index))
        };

        int IPoint<long>.Dimensions => 2;

        bool IEquatable<IPoint<long>>.Equals(IPoint<long>? other)
        {
            if (other is null)
                return false;

            var instance = (IPoint<long>)this;

            if (other.Dimensions != instance.Dimensions)
                return false;

            for (var d = 0; d < instance.Dimensions; d++)
                if (instance[d] != other[d])
                    return false;

            return true;
        }

        IEnumerable<IPoint<long>> IPoint<long>.GetNeighbors(bool includeDiagonal) => GetNeighbors(includeDiagonal).Cast<IPoint<long>>();

        public IEnumerable<LongPoint2> GetNeighbors(bool includeDiagonal = false)
        {
            for (var y = -1; y <= 1; y++)
                for (var x = -1; x <= 1; x++)
                {
                    if (!includeDiagonal && !((x == 0) ^ (y == 0)))
                        continue;

                    if (x == 0 && y == 0)
                        continue;

                    yield return new LongPoint2(X + x, Y + y);
                }
        }
    }
}
