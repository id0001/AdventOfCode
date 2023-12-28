using System.Numerics;

namespace AdventOfCode.Lib.Math
{
    public static class Polygon
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Pick's_theorem
        /// </summary>
        /// <param name="interiorPoints"></param>
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
        public static double ShoelaceArea(IEnumerable<Point2> vertices) => ShoelaceArea(vertices.Select(p => (p.X, p.Y)));

        /// <summary>
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea<TNumber>(IEnumerable<IPoint<TNumber>> vertices)
            where TNumber : IBinaryInteger<TNumber>
            => ShoelaceArea(vertices.Select(p => (p[0], p[1])));

        /// <summary>
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static double ShoelaceArea<TNumber>(IEnumerable<(TNumber X, TNumber Y)> vertices)
            where TNumber : IBinaryInteger<TNumber>
        {
            var sum1 = TNumber.Zero;
            var sum2 = TNumber.Zero;

            var first = ((TNumber X, TNumber Y)?)null;
            var last = ((TNumber X, TNumber Y)?)null;

            if (!vertices.Any())
                return 0d;

            foreach (var window in vertices.Windowed(2))
            {
                if (first is null)
                    first = window[0];

                sum1 += window[0].X * window[1].Y;
                sum2 += window[0].Y * window[1].X;

                last = window[1];
            }

            if (first is null || last is null)
                return 0d;

            sum1 += last.Value.X * first.Value.Y;
            sum2 += last.Value.Y * first.Value.X;

            return double.CreateChecked(TNumber.Abs(sum1 - sum2)) / 2d;
        }
    }
}
