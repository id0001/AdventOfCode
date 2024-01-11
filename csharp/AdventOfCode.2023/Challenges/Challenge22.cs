using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.Graphs;

namespace AdventOfCode2023.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader InputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var bricks = await InputReader.ParseLinesAsync(22, ParseLine).Select((v, i) => new Brick(i, v)).ToListAsync();
        var graph = CreateSupportGraph(bricks);

        return bricks.Count(b =>
            !graph.OutEdges(b.Id).Any() || graph.OutEdges(b.Id).Keys.All(s => graph.InEdges(s).Count > 1)).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var bricks = await InputReader.ParseLinesAsync(22, ParseLine).Select((v, i) => new Brick(i, v)).ToListAsync();
        var graph = CreateSupportGraph(bricks);

        var sum = 0;
        foreach (var brick in bricks)
        {
            var queue = new Queue<int>(new[] {brick.Id});
            var falls = new HashSet<int>(new[] {brick.Id});

            while (queue.Count > 0)
            {
                var id = queue.Dequeue();
                foreach (var supported in graph.OutEdges(id).Keys)
                {
                    if (!graph.InEdges(supported).Keys.All(falls.Contains))
                        continue;

                    queue.Enqueue(supported);
                    falls.Add(supported);
                }
            }

            sum += falls.Count - 1;
        }

        return sum.ToString();
    }

    private static DirectedGraph<int, int> CreateSupportGraph(IEnumerable<Brick> cubes)
    {
        var graph = new DirectedGraph<int, int>();
        var visited = new List<Brick>();

        foreach (var brick in cubes.OrderBy(b => b.Volume.Front))
        {
            var (id, volume) = brick;
            graph.AddVertex(id);

            var isStable = false;
            foreach (var v in visited.OrderByDescending(x => x.Volume.Back))
            {
                if (!v.Volume.IntersectsWith(volume with {Z = v.Volume.Back - 1}))
                    continue;

                volume = volume with {Z = v.Volume.Back};
                isStable = true;
                break;
            }

            if (!isStable)
                volume = volume with {Z = 1};

            foreach (var support in visited.Where(v =>
                         v.Volume.IntersectsWith(volume with {Z = volume.Position.Z - 1})))
                graph.AddEdge(support.Id, id, 1);

            visited.Add(new Brick(id, volume));
        }

        return graph;
    }

    private static Cube ParseLine(string line)
    {
        return line.SplitBy("~")
            .Into(parts =>
            {
                var start = parts
                    .First()
                    .SplitBy(",")
                    .As<int>()
                    .Into(p => new Point3(p[0], p[1], p[2]));

                var end = parts
                    .Second()
                    .SplitBy(",")
                    .As<int>()
                    .Into(p => new Point3(p[0], p[1], p[2]));

                return new Cube(start, end - start + Point3.One);
            });
    }

    private record Brick(int Id, Cube Volume);
}