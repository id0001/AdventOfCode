namespace AdventOfCode.Lib;

public static class EnumerableEx
{
    /// <summary>
    /// Enumerate all possible combinations of k items from a set of n
    /// </summary>
    /// <param name="n">Amount of items</param>
    /// <param name="k">Amount to choose</param>
    /// <returns></returns>
    public static IEnumerable<int[]> Combinations(int n, int k)
    {
        var result = new int[k];
        var stack = new Stack<int>(k);
        stack.Push(0);

        while (stack.Count > 0)
        {
            var index = stack.Count - 1;
            var value = stack.Pop();

            while (value < n)
            {
                result[index++] = value++;
                stack.Push(value);

                if (index != k) continue;
                
                yield return (int[])result.Clone();
                break;
            }
        }
    }

    /// <summary>
    /// Partition number n from 0 to n into an array of size k
    /// </summary>
    /// <param name="n">Amount of items to distribute</param>
    /// <param name="k">Amount of containers to distribute over</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<int[]> Partition(int n, int k)
    {
        if (n < k)
            throw new ArgumentException("Value must be greater or equal to k", nameof(n));
            
        if (k < 1)
            throw new ArgumentException("Value must be greater or equal to 1", nameof(k));

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

            for (var i = 1; i < k-1; i++)
            {
                if (distribution.Sum() > n)
                {
                    distribution[i + 1]++;
                    distribution[i] = 0;
                }
            }

            if (distribution.Sum() > n)
                yield break;
        }
    }
}