﻿using System.Globalization;
using System.Numerics;
using AdventOfCode.Lib.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        return TNumber.CreateChecked(System.Math.Truncate(value / divisor) - (10d * System.Math.Truncate(value / divisor2)));
    }
}