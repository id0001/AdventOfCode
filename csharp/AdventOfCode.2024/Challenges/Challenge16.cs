using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var grid = await inputReader.ReadGridAsync(16);

        var start = grid.Find((p, c) => c == 'S');
        var end = grid.Find((p, c) => c == 'E');

        return grid.Path(new Pose2(start, Face.Right), GetAdjacent)
            .WithWeight(GetWeight)
            .FindShortest(p => p.Position == end)
            .Cost
            .ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var grid = await inputReader.ReadGridAsync(16);
        var start = grid.Find((p, c) => c == 'S');
        var end = grid.Find((p, c) => c == 'E');

        return grid.Path(new Pose2(start, Face.Right), GetAdjacent)
            .WithWeight(GetWeight)
            .FindAll(n => n.Position == end)
            .GroupBy(p => p.Cost)
            .OrderBy(p => p.Key)
            .First()
            .SelectMany(p => p.Path.Select(n => n.Position))
            .Distinct()
            .Count()
            .ToString();
    }

    private static int GetWeight(char[,] grid, Pose2 current, Pose2 next)
    {
        if (current.Position != next.Position)
            return 1;

        return 1000;
    }

    private static IEnumerable<Pose2> GetAdjacent(char[,] grid, Pose2 current)
    {
        if (grid.At(current.Ahead) != '#')
            yield return current.Step();

        yield return current.TurnLeft();
        yield return current.TurnRight();
    }
}