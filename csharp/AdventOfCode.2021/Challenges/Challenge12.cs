using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Lib.Extensions;

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
            var paths = new List<string[]>();
            var queue = new Queue<string[]>();

            queue.Enqueue(new string[] { "start" });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                string last = path[^1];
                if (last == "end")
                {
                    paths.Add(path.ToArray());
                    continue;
                }

                if (!edges.ContainsKey(last))
                    continue; // Dead end

                foreach (var neighbor in edges[last])
                {
                    if (neighbor.All(char.IsUpper) || !path.Contains(neighbor))
                    {
                        string[] newPath = path.Concat(new[] { neighbor }).ToArray();
                        queue.Enqueue(newPath);
                    }
                }
            }

            return paths.Count.ToString();
        }

        [Part2]
        public string Part2()
        {
            var paths = new List<string[]>();
            var queue = new Queue<string[]>();

            queue.Enqueue(new string[] { "start" });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                string last = path[^1];

                if (last.EndsWith("'"))
                    last = last.Substring(0, last.Length - 1);

                if (last == "end")
                {
                    paths.Add(path);
                    continue;
                }

                if (!edges.ContainsKey(last))
                    continue; // Dead end

                foreach (var neighbor in edges[last])
                {
                    if (neighbor.All(char.IsUpper) || !path.Contains(neighbor))
                    {
                        string[] newPath = path.Concat(new[] { neighbor }).ToArray();
                        queue.Enqueue(newPath);
                    }
                    else if (!path.Any(x => x.Contains("'")) && neighbor != "start")
                    {
                        // Can only visit 1 lowercase room twice
                        string[] newPath = path.Concat(new[] { neighbor + "'" }).ToArray();
                        queue.Enqueue(newPath);
                    }
                }
            }

            return paths.Count.ToString();
        }
    }
}
