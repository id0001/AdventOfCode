namespace AdventOfCode.Lib.Math
{
    public static class Polynomial
    {
        /// <summary>
        /// Calculates the (2nd degree) root of the numbers using the quadratic formula
        /// y = ax^2 + bx + c
        /// (-b +- sqrt(b^2 - 4ac)) / 2a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static (double, double) FindRoots(double a, double b, double c)
        {
            return (
                (-b - System.Math.Sqrt((b * b) - (4 * a * c))) / (2d * a),
                (-b + System.Math.Sqrt((b * b) - (4 * a * c))) / (2d * a)
                );
        }
    }
}
