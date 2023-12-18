using System.Numerics;

namespace AdventOfCode.Lib.Math
{
    public static class Polygon
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Pick's_theorem
        /// </summary>
        /// <param name="area"></param>
        /// <param name="pointsOnBoundary"></param>
        /// <returns></returns>
        public static TNumber PicksArea<TNumber>(TNumber interiorPoints, TNumber pointsOnBoundary)
            where TNumber : IBinaryInteger<TNumber>
            => interiorPoints + (pointsOnBoundary / TNumber.CreateChecked(2)) - TNumber.One;

        /// <summary>
        /// Determine the integer points contained by the simple polygon.
        /// Using a modified Picks formula we can derive the interior points from the area and the amount of points from the border.
        /// https://en.wikipedia.org/wiki/Pick's_theorem
        /// </summary>
        /// <param name="area"></param>
        /// <param name="pointsOnBoundary"></param>
        /// <returns></returns>
        public static TNumber CountInteriorPoints<TNumber>(TNumber area, TNumber pointsOnBoundary)
            where TNumber : IBinaryInteger<TNumber>
            => area - (pointsOnBoundary / TNumber.CreateChecked(2)) + TNumber.One;

        /// <summary>
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea(IList<Point2> vertices) => ShoelaceArea<Point2, int>(vertices);

        /// <summary>
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea(IList<Point2L> vertices) => ShoelaceArea<Point2L, long>(vertices);

        private static double ShoelaceArea<TPoint, TNumber>(IList<TPoint> vertices)
            where TPoint : IPoint<TNumber>
            where TNumber : IBinaryInteger<TNumber>
        {
            var vcount = vertices.Count;
            var sum1 = TNumber.Zero;
            var sum2 = TNumber.Zero;

            for (var i = 0; i < vcount - 1; i++)
            {
                sum1 += vertices[i][0] * vertices[i + 1][1];
                sum2 += vertices[i][1] * vertices[i + 1][0];
            }

            sum1 += vertices[vcount - 1][0] * vertices[0][1];
            sum2 += vertices[vcount - 1][1] * vertices[0][0];

            return double.CreateChecked(TNumber.Abs(sum1 - sum2)) / 2d;
        }
    }
}
