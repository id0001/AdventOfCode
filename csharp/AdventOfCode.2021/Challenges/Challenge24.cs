using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Immutable;

namespace AdventOfCode2021.Challenges
{
    [Challenge(24)]
    public class Challenge24
    {
        private readonly IInputReader inputReader;

        private Func<int, int, int> f1;
        private Func<int, int, int> f2;
        private Func<int, int, int> f3;
        private Func<int, int, int> f4;
        private Func<int, int, int> f5;
        private Func<int, int, int> f6;
        private Func<int, int, int> f7;
        private Func<int, int, int> f8;
        private Func<int, int, int> f9;
        private Func<int, int, int> f10;
        private Func<int, int, int> f11;
        private Func<int, int, int> f12;
        private Func<int, int, int> f13;
        private Func<int, int, int> f14;

        private Func<int, int, int>[] funcsInOrder;
        private int[] listOfBValues;
        private int[] listOfAValues;

        public Challenge24(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public void Setup()
        {
            f1 = new Func<int, int, int>((w, z) => Calculate(1, 10, 10, w, z));
            f2 = new Func<int, int, int>((w, z) => Calculate(1, 13, 5, w, z));
            f3 = new Func<int, int, int>((w, z) => Calculate(1, 15, 12, w, z));
            f4 = new Func<int, int, int>((w, z) => Calculate(26, -12, 12, w, z));
            f5 = new Func<int, int, int>((w, z) => Calculate(1, 14, 6, w, z));
            f6 = new Func<int, int, int>((w, z) => Calculate(26, -2, 4, w, z));
            f7 = new Func<int, int, int>((w, z) => Calculate(1, 13, 15, w, z));
            f8 = new Func<int, int, int>((w, z) => Calculate(26, -12, 3, w, z));
            f9 = new Func<int, int, int>((w, z) => Calculate(1, 15, 7, w, z));
            f10 = new Func<int, int, int>((w, z) => Calculate(1, 11, 11, w, z));
            f11 = new Func<int, int, int>((w, z) => Calculate(26, -3, 2, w, z));
            f12 = new Func<int, int, int>((w, z) => Calculate(26, -13, 12, w, z));
            f13 = new Func<int, int, int>((w, z) => Calculate(26, -12, 4, w, z));
            f14 = new Func<int, int, int>((w, z) => Calculate(26, -13, 11, w, z));

            funcsInOrder = new[] { f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14 };
            listOfBValues = new[] { 10, 13, 15, -12, 14, -2, 13, -12, 15, 11, -3, -13, -12, -13 };
            listOfAValues = new[] { 1, 1, 1, 26, 1, 26, 1, 26, 1, 1, 26, 26, 26, 26 };
        }

        private ImmutableArray<int> FindBiggestNumber(Func<int, int, int>[] funcs, int[] blist, int[] alist, int funci, int z, ImmutableArray<int> result)
        {
            if (result.Length == 14)
                return result;

            var recResult = ImmutableArray<int>.Empty;
            for (int w = 9; w > 0; w--)
            {
                if (alist[funci] == 1 || z % 26 == w - blist[funci])
                {
                    int nz = funcs[funci](w, z);
                    var newResult = result.Add(w);
                    recResult = FindBiggestNumber(funcs, blist, alist, funci + 1, nz, newResult);
                    if (recResult.Length == 14)
                        break;
                }
            }

            return recResult;
        }

        private ImmutableArray<int> FindSmallestNumber(Func<int, int, int>[] funcs, int[] blist, int[] alist, int funci, int z, ImmutableArray<int> result)
        {
            if (result.Length == 14)
                return result;

            var recResult = ImmutableArray<int>.Empty;
            for (int w = 1; w <= 9; w++)
            {
                if (alist[funci] == 1 || z % 26 == w - blist[funci])
                {
                    int nz = funcs[funci](w, z);
                    var newResult = result.Add(w);
                    recResult = FindSmallestNumber(funcs, blist, alist, funci + 1, nz, newResult);
                    if (recResult.Length == 14)
                        break;
                }
            }

            return recResult;
        }


        [Part1]
        public string Part1()
        {
            var numbers = FindBiggestNumber(funcsInOrder, listOfBValues, listOfAValues, 0, 0, ImmutableArray.Create<int>());
            return string.Join(string.Empty, numbers);
        }

        [Part2]
        public string Part2()
        {
            var numbers = FindSmallestNumber(funcsInOrder, listOfBValues, listOfAValues, 0, 0, ImmutableArray.Create<int>());
            return string.Join(string.Empty, numbers);
        }

        private int Calculate(int a, int b, int c, int w, int z)
        {
            bool x = (z % 26) + b != w;

            z /= a; // 1 or 26

            if (x)
            {
                z *= 26;
                z += w + c;
            }

            return z;
        }
    }
}
