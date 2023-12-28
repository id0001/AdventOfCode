namespace AdventOfCode.Lib.Math;

public static class Polynomial
{
    /// <summary>
    ///     Calculates the (2nd degree) root of the numbers using the quadratic formula
    ///     y = ax^2 + bx + c
    ///     (-b +- sqrt(b^2 - 4ac)) / 2a
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static (double, double) FindRoots(double a, double b, double c)
    {
        return (
            (-b - System.Math.Sqrt(b * b - 4 * a * c)) / (2d * a),
            (-b + System.Math.Sqrt(b * b - 4 * a * c)) / (2d * a)
        );
    }

    /// <summary>
    ///     Interpolate the value of a point given the list of points
    /// </summary>
    /// <param name="points"></param>
    /// <param name="xi"></param>
    /// <returns></returns>
    public static long LagrangeInterpolate(IList<Point2> points, int xi)
    {
        var result = 0L;
        for (var i = 0; i < points.Count; i++)
        {
            var term = (long) points[i].Y;
            for (var j = 0; j < points.Count; j++)
                if (j != i)
                    term = term * (xi - points[j].X) / (points[i].X - points[j].X);

            result += term;
        }

        return result;
    }
}