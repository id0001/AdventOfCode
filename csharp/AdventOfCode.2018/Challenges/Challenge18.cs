using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2018.Challenges;

[Challenge(18)]
public class Challenge18(IInputReader inputReader)
{
    private static readonly Rectangle Bounds = new(0, 0, 50, 50);

    [Part1]
    public async Task<string> Part1Async()
    {
        var initialGrid = await inputReader.ReadGridAsync(18);

        for (var i = 0; i < 10; i++)
        {
            var nextGrid = new char[initialGrid.GetLength(1), initialGrid.GetLength(0)];

            foreach (var (p, _) in initialGrid.AsEnumerable())
                nextGrid[p.Y, p.X] = ExecuteAutomata(initialGrid, p);

            initialGrid = nextGrid;
        }

        var (tree, lumberyard) = Count(initialGrid);
        return (tree * lumberyard).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var initialGrid = await inputReader.ReadGridAsync(18);

        var previousStates = new Dictionary<string, int>
        {
            {initialGrid.PrintToString((_, c) => c), 0}
        };

        var total = 1_000_000_000;
        for (var i = 0; i < total; i++)
        {
            var nextGrid = new char[initialGrid.GetLength(1), initialGrid.GetLength(0)];

            foreach (var (p, _) in initialGrid.AsEnumerable())
                nextGrid[p.Y, p.X] = ExecuteAutomata(initialGrid, p);

            initialGrid = nextGrid;

            var key = initialGrid.PrintToString((_, c) => c);
            if (previousStates.ContainsKey(key))
            {
                var cycleLength = i - previousStates[key];
                i += (total - i) / cycleLength * cycleLength;
                continue;
            }

            previousStates.Add(key, i);
        }

        var (tree, lumberyard) = Count(initialGrid);
        return (tree * lumberyard).ToString();
    }

    private static char ExecuteAutomata(char[,] state, Point2 p)
    {
        return state[p.Y, p.X] switch
        {
            '.' => OpenAcre(state, p),
            '#' => Lumberyard(state, p),
            '|' => Tree(state, p),
            _ => throw new NotImplementedException()
        };
    }

    private static char OpenAcre(char[,] state, Point2 p)
        => p.GetNeighbors(true).Where(IsInBounds).Count(n => state[n.Y, n.X] == '|') >= 3 ? '|' : '.';

    private static char Tree(char[,] state, Point2 p)
        => p.GetNeighbors(true).Where(IsInBounds).Count(n => state[n.Y, n.X] == '#') >= 3 ? '#' : '|';

    private static char Lumberyard(char[,] state, Point2 p)
    {
        var lumberyardCount = 0;
        var treeCount = 0;
        foreach (var n in p.GetNeighbors(true).Where(IsInBounds))
        {
            switch (state[n.Y, n.X])
            {
                case '#':
                    lumberyardCount++;
                    break;
                case '|':
                    treeCount++;
                    break;
            }

            if (lumberyardCount > 0 && treeCount > 0)
                return '#';
        }

        return '.';
    }

    private static (int Tree, int Lumberyard) Count(char[,] state)
    {
        int tree = 0, lumberyard = 0;
        foreach (var (_, c) in state.AsEnumerable())
            switch (c)
            {
                case '.':
                    break;
                case '|':
                    tree++;
                    break;
                case '#':
                    lumberyard++;
                    break;
            }

        return (tree, lumberyard);
    }

    private static bool IsInBounds(Point2 p) => Bounds.Contains(p);
}