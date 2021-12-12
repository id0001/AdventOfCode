using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Lib.Extensions;
using System.Collections.Immutable;

namespace AdventOfCode2021.Challenges
{
    [Challenge(12)]
    public class Challenge12
    {
        private readonly IInputReader inputReader;
        private Dictionary<string, ISet<string>> edges;

        public Challenge12(IInputReader inputReader)
        {
            this.inputReader = inputReader;
        }

        [Setup]
        public async Task SetupAsync()
        {
            edges = new Dictionary<string, ISet<string>>();
            await foreach (var line in inputReader.ReadLinesAsync(12))
            {
                string[] splitPath = line.Split('-');

                edges.AddOrUpdate(splitPath[0], set =>
                {
                    set ??= new HashSet<string>();
                    set.Add(splitPath[1]);
                    return set;
                });

                edges.AddOrUpdate(splitPath[1], set =>
                {
                    set ??= new HashSet<string>();
                    set.Add(splitPath[0]);
                    return set;
                });
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

            IImmutableSet<string> newVisited = null;
            if (currentNode.All(char.IsLower))
            {
                newVisited = visited.Add(currentNode);
            }
            else
            {
                newVisited = visited;
            }

            return edges[currentNode].Sum(neighbor => CountPaths(neighbor, newVisited, canVisitTwice && !visited.Contains(currentNode)));
        }
    }
}
