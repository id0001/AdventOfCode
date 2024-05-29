using AdventOfCode.Lib.Math;
using System.Globalization;
using System.Numerics;

namespace AdventOfCode.Lib
{
    public static class NumberExtensions
    {
        public static TNumber Mod<TNumber>(this TNumber divident, TNumber divisor)
            where TNumber : IAdditionOperators<TNumber, TNumber, TNumber>, IModulusOperators<TNumber, TNumber, TNumber>
            => Euclid.Modulus(divident, divisor);

        public static string ToHexString<TNumber>(this TNumber source, int padding = 2, bool upperCase = false)
            where TNumber : IBinaryInteger<TNumber>
            => source.ToString($"{(upperCase ? "X" : "x")}{padding}", CultureInfo.InvariantCulture);
    }
}
