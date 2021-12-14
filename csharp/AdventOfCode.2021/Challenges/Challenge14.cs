using AdventOfCode.Lib;
using AdventOfCode.Lib.Extensions;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.Challenges
{
  [Challenge(14)]
  public class Challenge14
  {
    private readonly IInputReader inputReader;
    private IDictionary<string, char> insertionMap;
    private string template;
    private Dictionary<char, int> indexes;

    public Challenge14(IInputReader inputReader)
    {
      this.inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
      var lines = await inputReader.ReadLinesAsync(14).ToArrayAsync();

      template = lines[0];

      insertionMap = new Dictionary<string, char>();
      for (int i = 2; i < lines.Length; i++)
      {
        string[] split = lines[i].Split(" -> ");
        insertionMap.Add(split[0], Convert.ToChar(split[1]));
      }

      char[] uniquechars = insertionMap.Values.Distinct().ToArray();
      indexes = new Dictionary<char, int>();
      for (int i = 0; i < uniquechars.Length; i++)
      {
        indexes.Add(uniquechars[i], i);
      }
    }

    [Part1]
    public string Part1()
    {
      return CalculateResult(10).ToString();
    }

    [Part2]
    public string Part2()
    {
      return CalculateResult(40).ToString();
    }

    private long CalculateResult(int steps)
    {
      long[] counts = new long[indexes.Count];
      var cache = new Dictionary<StateKey, long[]>();
      for (int i = 0; i < template.Length - 1; i++)
      {
        counts[GetCharIndex(template[i])]++;
        counts = counts.Zip(CountInserted(template.Substring(i, 2), 0, steps - 1, cache), (a, b) => a + b).ToArray();
      }

      counts[GetCharIndex(template[^1])]++;
      return (counts.Max() - counts.Min());
    }

    private long[] CountInserted(string pair, int depth, int maxDepth, Dictionary<StateKey, long[]> cache)
    {
      if (cache.TryGetValue(new StateKey(pair, depth), out var counts))
      {
        return counts;
      }

      char inserted = insertionMap[pair];
      counts = new long[indexes.Count];

      if (depth == maxDepth)
      {
        counts[GetCharIndex(inserted)]++;
        return counts;
      }

      string pair1 = new string(new[] { pair[0], inserted });
      string pair2 = new string(new[] { inserted, pair[1] });
      long[] c0 = CountInserted(pair1, depth + 1, maxDepth, cache);
      long[] c1 = CountInserted(pair2, depth + 1, maxDepth, cache);

      counts = c0.Zip(c1, (a, b) => a + b).ToArray();
      counts[GetCharIndex(inserted)]++;

      cache.AddOrUpdate(new StateKey(pair, depth), counts);

      return counts;
    }

    private int GetCharIndex(char c) => indexes[c];

    private record StateKey(string Pair, int Depth);
  }
}
