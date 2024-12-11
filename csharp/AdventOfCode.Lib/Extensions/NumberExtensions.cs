using System.Globalization;
using System.Numerics;
using AdventOfCode.Lib.Math;

namespace AdventOfCode.Lib;

public static class NumberExtensions
{
    public static TNumber Mod<TNumber>(this TNumber divident, TNumber divisor)
        where TNumber : IAdditionOperators<TNumber, TNumber, TNumber>, IModulusOperators<TNumber, TNumber, TNumber>
        => Euclid.Modulus(divident, divisor);

    public static string ToHexString<TNumber>(this TNumber source, int padding = 2, bool upperCase = false)
        where TNumber : IBinaryInteger<TNumber>
        => source.ToString($"{(upperCase ? "X" : "x")}{padding}", CultureInfo.InvariantCulture);

    public static Point2 ToPoint2(this int i, int width)
    {
        return new Point2(i % width, i / width);
    }

    public static TNumber ExtractDigit<TNumber>(this TNumber source, int offsetFromRight)
        where TNumber : IBinaryInteger<TNumber>
    {
        var value = double.CreateChecked(source);
        var divisor = System.Math.Pow(10d, offsetFromRight);
        var divisor2 = System.Math.Pow(10d, offsetFromRight + 1);
        return TNumber.CreateChecked(System.Math.Truncate(value / divisor) -
                                     10d * System.Math.Truncate(value / divisor2));
    }

    public static IEnumerable<TNumber> EnumerateDigits<TNumber>(this TNumber source)
        where TNumber :
        IBinaryInteger<TNumber>,
        IComparisonOperators<TNumber, TNumber, bool>,
        IDivisionOperators<TNumber, TNumber, TNumber>,
        IModulusOperators<TNumber, TNumber, TNumber>
    {
        var numberBase = TNumber.CreateChecked(10);
        return EnumerateDigits(source, numberBase);
    }

    public static IEnumerable<TNumber> EnumerateDigits<TNumber>(TNumber number, TNumber numberBase)
        where TNumber :
        IBinaryInteger<TNumber>,
        IComparisonOperators<TNumber, TNumber, bool>,
        IDivisionOperators<TNumber, TNumber, TNumber>,
        IModulusOperators<TNumber, TNumber, TNumber>
    {
        if (number < numberBase)
            yield return number;
        else
            foreach (var n in EnumerateDigits(number / numberBase, numberBase)
                         .Concat(EnumerateDigits(number % numberBase, numberBase)))
                yield return n;
    }

    public static TNumber Concat<TNumber>(this TNumber a, TNumber b)
        where TNumber :
        IBinaryInteger<TNumber>,
        IAdditionOperators<TNumber, TNumber, TNumber>,
        IMultiplyOperators<TNumber, TNumber, TNumber>
        => a * TNumber.CreateChecked(System.Math.Pow(10d, System.Math.Floor(System.Math.Log10(double.CreateChecked(b))) + 1)) + b;

    public static int CountDigits<TNumber>(this TNumber number)
        where TNumber :
        IBinaryInteger<TNumber>,
        IComparisonOperators<TNumber, TNumber, bool>,
        IDivisionOperators<TNumber, TNumber, TNumber>,
        IModulusOperators<TNumber, TNumber, TNumber> 
        => (int)System.Math.Floor(System.Math.Log10(double.CreateChecked(number))) + 1;
}