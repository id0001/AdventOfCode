using System.Collections.Immutable;
using AdventOfCode.Core;

namespace AdventOfCode2021.Challenges;

[Challenge(24)]
public class Challenge24
{
    private readonly Func<int, int, int>[] _funcsInOrder;
    private readonly int[] _listOfBValues;
    private readonly int[] _listOfAValues;

    public Challenge24()
    {
        var f1 = new Func<int, int, int>((w, z) => Calculate(1, 10, 10, w, z));
        var f2 = new Func<int, int, int>((w, z) => Calculate(1, 13, 5, w, z));
        var f3 = new Func<int, int, int>((w, z) => Calculate(1, 15, 12, w, z));
        var f4 = new Func<int, int, int>((w, z) => Calculate(26, -12, 12, w, z));
        var f5 = new Func<int, int, int>((w, z) => Calculate(1, 14, 6, w, z));
        var f6 = new Func<int, int, int>((w, z) => Calculate(26, -2, 4, w, z));
        var f7 = new Func<int, int, int>((w, z) => Calculate(1, 13, 15, w, z));
        var f8 = new Func<int, int, int>((w, z) => Calculate(26, -12, 3, w, z));
        var f9 = new Func<int, int, int>((w, z) => Calculate(1, 15, 7, w, z));
        var f10 = new Func<int, int, int>((w, z) => Calculate(1, 11, 11, w, z));
        var f11 = new Func<int, int, int>((w, z) => Calculate(26, -3, 2, w, z));
        var f12 = new Func<int, int, int>((w, z) => Calculate(26, -13, 12, w, z));
        var f13 = new Func<int, int, int>((w, z) => Calculate(26, -12, 4, w, z));
        var f14 = new Func<int, int, int>((w, z) => Calculate(26, -13, 11, w, z));

        _funcsInOrder = new[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14 };
        _listOfBValues = new[] { 10, 13, 15, -12, 14, -2, 13, -12, 15, 11, -3, -13, -12, -13 };
        _listOfAValues = new[] { 1, 1, 1, 26, 1, 26, 1, 26, 1, 1, 26, 26, 26, 26 };
    }

    private static ImmutableArray<int> FindBiggestNumber(IReadOnlyList<Func<int, int, int>> funcs,
        IReadOnlyList<int> blist, IReadOnlyList<int> alist, int funci,
        int z, ImmutableArray<int> result)
    {
        if (result.Length == 14)
            return result;

        var recResult = ImmutableArray<int>.Empty;
        for (var w = 9; w > 0; w--)
        {
            if (alist[funci] != 1 && z % 26 != w - blist[funci]) continue;

            var nz = funcs[funci](w, z);
            var newResult = result.Add(w);
            recResult = FindBiggestNumber(funcs, blist, alist, funci + 1, nz, newResult);
            if (recResult.Length == 14)
                break;
        }

        return recResult;
    }

    private static ImmutableArray<int> FindSmallestNumber(IReadOnlyList<Func<int, int, int>> funcs,
        IReadOnlyList<int> blist, IReadOnlyList<int> alist, int funci,
        int z, ImmutableArray<int> result)
    {
        if (result.Length == 14)
            return result;

        var recResult = ImmutableArray<int>.Empty;
        for (var w = 1; w <= 9; w++)
        {
            if (alist[funci] != 1 && z % 26 != w - blist[funci]) continue;
            
            var nz = funcs[funci](w, z);
            var newResult = result.Add(w);
            recResult = FindSmallestNumber(funcs, blist, alist, funci + 1, nz, newResult);
            if (recResult.Length == 14)
                break;
        }

        return recResult;
    }


    [Part1]
    public string Part1()
    {
        var numbers = FindBiggestNumber(_funcsInOrder, _listOfBValues, _listOfAValues, 0, 0,
            ImmutableArray.Create<int>());
        return string.Join(string.Empty, numbers);
    }

    [Part2]
    public string Part2()
    {
        var numbers =
            FindSmallestNumber(_funcsInOrder, _listOfBValues, _listOfAValues, 0, 0, ImmutableArray.Create<int>());
        return string.Join(string.Empty, numbers);
    }

    private static int Calculate(int a, int b, int c, int w, int z)
    {
        var x = z % 26 + b != w;

        z /= a; // 1 or 26

        if (!x) return z;
        
        z *= 26;
        z += w + c;

        return z;
    }
}