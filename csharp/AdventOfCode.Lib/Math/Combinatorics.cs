using Microsoft;

namespace AdventOfCode.Lib.Math;

/// <summary>
///     Partly derived from MathNet: https://github.com/mathnet/mathnet-numerics/blob/master/src/Numerics/Combinatorics.cs
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

    public static IEnumerable<int[]> GenerateCombinations(int n, int k)
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

    public static double Permutations(int n)
    {
        Requires.Argument(n > 0, "Value must be greater than zero", nameof(n));

        return SpecialFunctions.Factorial(n);
    }

    public static IEnumerable<int[]> GeneratePermutations(int n)
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

    public static double Partitions(int n, int k, int minPartitionSize = 0) => SpecialFunctions.Binomial((n - (k * minPartitionSize)) + k - 1, k - 1);

    /// <summary>
    ///     Partition number n from 0 to n into an array of size k.
    /// </summary>
    /// <param name="n">Amount of items to distribute</param>
    /// <param name="k">Amount of containers to distribute over</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<int[]> GeneratePartitions(int n, int k, int minPartitionSize = 0)
    {
        //Requires.Argument(n >= k, "Value must be greater or equal to k", nameof(n));
        Requires.Argument(k >= 1, "Value must be greater or equal to 1", nameof(k));
        Requires.Argument(minPartitionSize >= 0, "Value must be greater or equal to 0", nameof(minPartitionSize));
        Requires.Argument(minPartitionSize <= n / k, "Value less or equal to n/k", nameof(minPartitionSize));

        if (k == 1)
        {
            yield return new[] { n };
            yield break;
        }

        var distribution = new int[k];
        if (minPartitionSize > 0)
            Array.Fill(distribution, minPartitionSize);

        while (true)
        {
            distribution[^1] = n - distribution.Take(k - 1).Sum();

            yield return (int[])distribution.Clone();

            distribution[^2]++;
            distribution[^1] = minPartitionSize;

            for (var i = k - 2; i >= 1; i--)
            {
                if (distribution.Sum() <= n)
                    continue;

                distribution[i - 1]++;
                distribution[i] = minPartitionSize;
            }

            if (distribution.Sum() > n)
                yield break;
        }
    }
}