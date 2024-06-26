using AdventOfCode.Core;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(15)]
public class Challenge15
{
    private const int StartA = 783;
    private const int StartB = 325;
    private const long Divisor = 2147483647L;
    private const long FactorA = 16807L;
    private const long FactorB = 48271L;

    [Part1]
    public string Part1()
    {
        long valueA = StartA;
        long valueB = StartB;
        int matches = 0;
        for (var i = 0; i < 40_000_000; i++)
        {
            if (IsMatch(valueA, valueB))
                matches++;

            valueA = GetNextValueForA(valueA);
            valueB = GetNextValueForB(valueB);
        }

        return matches.ToString();
    }

    [Part2]
    public string Part2()
    {
        long valueA = StartA;
        long valueB = StartB;
        int matches = 0;
        for (var i = 0; i < 5_000_000; i++)
        {
            if (IsMatch(valueA, valueB))
                matches++;

            valueA = GetNextValueForA(valueA, true);
            valueB = GetNextValueForB(valueB, true);
        }

        return matches.ToString();
    }

    private static long GetNextValueForA(long current, bool part2 = false) => GetNextValue(current, FactorA, part2 ? 4 : 1);
    private static long GetNextValueForB(long current, bool part2 = false) => GetNextValue(current, FactorB, part2 ? 8 : 1);
    private static long GetNextValue(long current, long factor, long mustBeDivisibleBy)
    {
        current = (current * factor).Mod(Divisor);
        while (current.Mod(mustBeDivisibleBy) != 0)
            current = (current * factor).Mod(Divisor);

        return current;
    }

    private static bool IsMatch(long a, long b) => (a & 0xFFFF) == (b & 0xFFFF);
}