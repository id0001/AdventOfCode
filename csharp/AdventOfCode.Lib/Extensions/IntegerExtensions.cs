using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib
{
    public static class IntegerExtensions
    {
        public static int Mod(this int divident, int divisor) => Euclid.Modulus(divident, divisor);
    }
}
