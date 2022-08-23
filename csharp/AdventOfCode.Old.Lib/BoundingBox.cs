using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Lib
{
    public class BoundingBox<T> where T : IPoint, new()
    {
        private int[] min;
        private int[] max;

        public BoundingBox()
        {
            Dimensions = new T().Dimensions;
            min = Enumerable.Repeat(int.MaxValue, Dimensions).ToArray();
            max = Enumerable.Repeat(int.MinValue, Dimensions).ToArray();
        }

        public int Dimensions { get; }

        public void Inflate(T added)
        {
            for (int d = 0; d < Dimensions; d++)
            {
                if (added.GetValue(d) < min[d])
                    min[d] = added.GetValue(d);

                if (added.GetValue(d) > max[d])
                    max[d] = added.GetValue(d);
            }
        }

        public void Deflate(IEnumerable<T> points, T removed)
        {
            bool shouldUpdate = false;
            for (int d = 0; d < Dimensions; d++)
            {
                if (removed.GetValue(d) == min[d] || removed.GetValue(d) == max[d])
                {
                    shouldUpdate = true;
                    break;
                }
            }

            if (shouldUpdate)
            {
                min = Enumerable.Repeat(int.MaxValue, Dimensions).ToArray();
                max = Enumerable.Repeat(int.MinValue, Dimensions).ToArray();

                foreach (var point in points)
                {
                    Inflate(point);
                }
            }
        }

        public int GetMin(int dimension)
        {
            if (dimension < 0 || dimension >= Dimensions)
                throw new ArgumentOutOfRangeException(nameof(dimension));

            return min[dimension];
        }

        public int GetMax(int dimension)
        {
            if (dimension < 0 || dimension >= Dimensions)
                throw new ArgumentOutOfRangeException(nameof(dimension));

            return max[dimension] + 1;
        }

        public bool Contains(IPoint point)
        {
            for (int d = 0; d < Dimensions; d++)
            {
                if (point.GetValue(d) < min[d] || point.GetValue(d) > max[d])
                    return false;
            }

            return true;
        }
    }
}
