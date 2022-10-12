using Microsoft;

namespace AdventOfCode.Lib.Math;

/// <summary>
/// Partly derived from MathNet: https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Combinatorics.cs
/// </summary>
public static class Combinatorics
{
    public static double Variations(int n, int k)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));
        Requires.Argument(n > k, "Value must be greater than k", nameof(n));

        return SpecialFunctions.Factorial(n) / SpecialFunctions.Factorial(n - k);
    }

    public static double VariationsWithRepetition(int n, int k)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        return System.Math.Pow(n, k);
    }

    public static double Combinations(int n, int k) => SpecialFunctions.Binomial(n, k);

    public static double CombinationsWithRepetition(int n, int k)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        return SpecialFunctions.Factorial(n + k - 1) /
               (SpecialFunctions.Factorial(k) * SpecialFunctions.Factorial(n - 1));
    }

    public static IEnumerable<int[]> GenerateAllCombinations(int n, int k)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        var a = new int[k];

        var c = Enumerable.Range(0, k + 2).ToArray();
        c[k] = n;
        c[k + 1] = 0;

        var j = k;
        if (k > n) yield break;

        if (k == n)
            yield return c.Take(k).ToArray();
        else
            while (true)
            {
                for (var i = 0; i < k; i++)
                    a[i] = c[i];

                yield return (int[])a.Clone();

                int x;
                if (j > 0)
                {
                    x = j;
                }
                else
                {
                    if (c[0] + 1 < c[1])
                    {
                        c[0] += 1;
                        continue;
                    }

                    j = 1;
                    do
                    {
                        j++;
                        c[j - 2] = j - 2;
                        x = c[j - 1] + 1;
                    } while (x == c[j]);

                    if (j > k)
                        break;
                }

                c[j - 1] = x;
                j--;
            }
    }

    public static IEnumerable<T[]> SelectAllCombinations<T>(IEnumerable<T> source, int k)
    {
        var list = source.ToList();
        Requires.NotNullOrEmpty(list, nameof(source));
        Requires.Argument(k > 0, "Value must be greater than zero", nameof(k));

        var a = new T[k];
        var c = Enumerable.Range(0, k + 2).ToArray();
        c[k] = list.Count;
        c[k + 1] = 0;

        var j = k;
        if (k > list.Count)
            yield break;

        if (k == list.Count)
            yield return c.Take(k).Select(i => list[i]).ToArray();
        else
            while (true)
            {
                for (var i = 0; i < k; i++)
                    a[i] = list[c[i]];

                yield return (T[])a.Clone();

                int x;
                if (j > 0)
                {
                    x = j;
                }
                else
                {
                    if (c[0] + 1 < c[1])
                    {
                        c[0] += 1;
                        continue;
                    }

                    j = 1;
                    do
                    {
                        j++;
                        c[j - 2] = j - 2;
                        x = c[j - 1] + 1;
                    } while (x == c[j]);

                    if (j > k)
                        break;
                }

                c[j - 1] = x;
                j--;
            }
    }

    public static double Permutations(int n)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));

        return SpecialFunctions.Factorial(n);
    }

    public static IEnumerable<int[]> GenerateAllPermutations(int n)
    {
        Requires.Argument(n >= 0, "Value must be greater or equal to zero", nameof(n));

        var a = Enumerable.Range(0, n).ToArray();

        while (true)
        {
            yield return a;
            var j = n - 1;

            while (j > 0 && a[j - 1] >= a[j])
                j--;

            if (j == 0)
                yield break;

            var l = n;
            while (a[j - 1] >= a[l - 1])
                l--;

            (a[j - 1], a[l - 1]) = (a[l - 1], a[j - 1]);
            Array.Reverse(a, j, n - j);
        }
    }

    public static IEnumerable<T[]> SelectAllPermutations<T>(IEnumerable<T> source)
    {
        var list = source.ToList();

        Requires.NotNullOrEmpty(list, nameof(source));

        var a = Enumerable.Range(0, list.Count).ToArray();

        while (true)
        {
            yield return a.Select(i => list[i]).ToArray();
            var j = list.Count - 1;

            while (j > 0 && a[j - 1] >= a[j])
                j--;

            if (j == 0)
                yield break;

            var l = list.Count;
            while (a[j - 1] >= a[l - 1])
                l--;

            (a[j - 1], a[l - 1]) = (a[l - 1], a[j - 1]);
            Array.Reverse(a, j, list.Count - j);
        }
    }

    public static double Partitions(int n, int k) => SpecialFunctions.Binomial(n + k - 1, k - 1);

    /// <summary>
    /// Partition number n from 0 to n into an array of size k
    /// </summary>
    /// <param name="n">Amount of items to distribute</param>
    /// <param name="k">Amount of containers to distribute over</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<int[]> GenerateAllPartitions(int n, int k)
    {
        Requires.Argument(n >= k, "Value must be greater or equal to k", nameof(n));
        Requires.Argument(k >= 1, "Value must be greater or equal to 1", nameof(k));

        if (k == 1)
        {
            yield return new[] { n };
            yield break;
        }

        var distribution = new int[k];

        while (true)
        {
            distribution[0] = n - distribution.Skip(1).Sum();

            yield return (int[])distribution.Clone();

            distribution[1]++;
            distribution[0] = 0;

            for (var i = 1; i < k - 1; i++)
            {
                if (distribution.Sum() <= n)
                    continue;

                distribution[i + 1]++;
                distribution[i] = 0;
            }

            if (distribution.Sum() > n)
                yield break;
        }
    }
}