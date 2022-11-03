using System.Collections.Immutable;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2021.Challenges;

[Challenge(12)]
public class Challenge12
{
    private readonly IInputReader _inputReader;
    private readonly Dictionary<string, HashSet<string>> _edges = new();

    public Challenge12(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in _inputReader.ReadLinesAsync(12))
        {
            var splitPath = line.Split('-');

            if (!_edges.TryGetValue(splitPath[0], out var set))
                _edges.Add(splitPath[0], set = new HashSet<string>());
            set.Add(splitPath[1]);

            if (!_edges.TryGetValue(splitPath[1], out set))
                _edges.Add(splitPath[1], set = new HashSet<string>());
            set.Add(splitPath[0]);
        }
    }

    [Part1]
    public string Part1()
    {
        return CountPaths("start", ImmutableHashSet<string>.Empty, false).ToString();
    }

    [Part2]
    public string Part2()
    {
        return CountPaths("start", ImmutableHashSet<string>.Empty, true).ToString();
    }

    private int CountPaths(string currentNode, IImmutableSet<string> visited, bool canVisitTwice)
    {
        if (currentNode == "end")
            return 1;

        if (currentNode == "start" && visited.Contains(currentNode)) // Can never visit start twice
            return 0;

        if (visited.Contains(currentNode) && !canVisitTwice)
            return 0;

        var newVisited = visited;
        if (currentNode.All(char.IsLower))
            newVisited = visited.Add(currentNode);

        return _edges[currentNode].Sum(neighbor =>
            CountPaths(neighbor, newVisited, canVisitTwice && !visited.Contains(currentNode)));
    }
}