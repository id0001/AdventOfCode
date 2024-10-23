using AdventOfCode.Lib.Collections.Helpers;
using Microsoft;

namespace AdventOfCode.Lib
{
    public static class BoundingBoxExtensions
    {
        public static IEnumerable<Point2> EnumeratePoints(this BoundingBox<Point2, int> box)
        {
            var minBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMin).ToArray();
            var maxBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMax).ToArray();

            foreach (var point in IterateOverPoints(minBounds, maxBounds))
                yield return new Point2(point[0], point[1]);
        }

        public static IEnumerable<Point3> EnumeratePoints(this BoundingBox<Point3, int> box)
        {
            var minBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMin).ToArray();
            var maxBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMax).ToArray();

            foreach (var point in IterateOverPoints(minBounds, maxBounds))
                yield return new Point3(point[0], point[1], point[2]);
        }

        public static Rectangle ToRectangle(this BoundingBox<Point2, int> box)
        {
            var minBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMin).ToArray();
            var maxBounds = Enumerable.Range(0, box.Dimensions).Select(box.GetMax).ToArray();

            return new Rectangle(minBounds[0], minBounds[1], maxBounds[0] - minBounds[0] + 1, maxBounds[1] - minBounds[1] + 1);
        }

        private static IEnumerable<int[]> IterateOverPoints(int[] minBounds, int[] maxBounds)
        {
            Requires.Argument(minBounds.Length == maxBounds.Length, null, "Min and max bounds must have the same length");

            int dimensions = minBounds.Length;
            int[] currentPoint = new int[dimensions];

            IEnumerable<int[]> Recurse(int dimension)
            {
                if (dimension == dimensions)
                {
                    yield return (int[])currentPoint.Clone();
                    yield break;
                }

                for (var i = minBounds[dimension]; i <= maxBounds[dimension]; i++)
                {
                    currentPoint[dimension] = i;
                    foreach (var point in Recurse(dimension + 1))
                        yield return point;
                }
            }

            foreach (var point in Recurse(0))
                yield return point;
        }
    }
}
