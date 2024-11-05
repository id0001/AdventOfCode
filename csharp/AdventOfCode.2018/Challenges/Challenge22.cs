using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;
using AdventOfCode.Lib.PathFinding;

namespace AdventOfCode2018.Challenges;

[Challenge(22)]
public class Challenge22(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        var (depth, target) = ParseInput(await inputReader.ReadAllTextAsync(22));
        var grid = new int[target.Y + 1, target.X + 1];
        for (var y = 0; y <= target.Y; y++)
        {
            for (var x = 0; x <= target.X; x++)
            {
                var geologicalIndex = (x, y) switch
                {
                    (0, 0) => 0,
                    var (tx, ty) when tx == target.X && ty == target.Y => 0,
                    (0, _) => y * 48271,
                    (_, 0) => x * 16807,
                    _ => grid[y, x - 1] * grid[y - 1, x]
                };

                grid[y, x] = (geologicalIndex + depth) % 20183;
            }
        }

        return grid.AsEnumerable().Sum(kv => kv.Value % 3).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var (depth, target) = ParseInput(await inputReader.ReadAllTextAsync(22));

        var grid = CreateGrid(depth, target);
        
        var astar = new AStar<State>(n => GetAdjacent(grid, n), Weight);
        astar.TryPath(new State(Point2.Zero, EquipedTool.Torch), s => s.Location == target && s.Equiped == EquipedTool.Torch, out _, out var cost);
        
        return cost.ToString();
    }

    private static int Weight(State current, State next) => current.Equiped != next.Equiped ? 7 : 1;

    private static IEnumerable<State> GetAdjacent(TerrainType[,] grid, State state)
    {
        var type = grid[state.Location.Y, state.Location.X];
        var bounds = grid.Bounds();

        return type switch
        {
            TerrainType.Rocky => GetNextForRockyTerrain(grid, state, bounds),
            TerrainType.Wet => GetNextForWetTerrain(grid, state, bounds),
            TerrainType.Narrow => GetNExtForNarrowTerrain(grid, state, bounds),
            _ => throw new NotImplementedException(),
        };
    }

    private static IEnumerable<State> GetNExtForNarrowTerrain(TerrainType[,] grid, State state, Rectangle bounds)
    {
        if (state.Equiped == EquipedTool.ClimbingGear)
        {
            yield return state with { Equiped = EquipedTool.Torch };
            yield return state with { Equiped = EquipedTool.Neither };
            yield break;
        }

        yield return state.Equiped == EquipedTool.Torch
            ? state with { Equiped = EquipedTool.Neither }
            : state with { Equiped = EquipedTool.Torch };

        foreach (var neighbor in state.Location.GetNeighbors().Where(bounds.Contains))
        {
            switch (grid[neighbor.Y, neighbor.X])
            {
                case TerrainType.Rocky when state.Equiped == EquipedTool.Torch:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Wet when state.Equiped == EquipedTool.Neither:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Narrow:
                    yield return state with { Location = neighbor };
                    break;
            }
        }
    }

    private static IEnumerable<State> GetNextForWetTerrain(TerrainType[,] grid, State state, Rectangle bounds)
    {
        if (state.Equiped == EquipedTool.Torch)
        {
            yield return state with { Equiped = EquipedTool.ClimbingGear };
            yield return state with { Equiped = EquipedTool.Neither };
            yield break;
        }

        yield return state.Equiped == EquipedTool.ClimbingGear
            ? state with { Equiped = EquipedTool.Neither }
            : state with { Equiped = EquipedTool.ClimbingGear };

        foreach (var neighbor in state.Location.GetNeighbors().Where(bounds.Contains))
        {
            switch (grid[neighbor.Y, neighbor.X])
            {
                case TerrainType.Rocky when state.Equiped == EquipedTool.ClimbingGear:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Wet:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Narrow when state.Equiped == EquipedTool.Neither:
                    yield return state with { Location = neighbor };
                    break;
            }
        }
    }

    private static IEnumerable<State> GetNextForRockyTerrain(TerrainType[,] grid, State state, Rectangle bounds)
    {
        if (state.Equiped == EquipedTool.Neither)
        {
            yield return state with { Equiped = EquipedTool.ClimbingGear };
            yield return state with { Equiped = EquipedTool.Torch };
            yield break;
        }

        yield return state.Equiped == EquipedTool.Torch
            ? state with { Equiped = EquipedTool.ClimbingGear }
            : state with { Equiped = EquipedTool.Torch };

        foreach (var neighbor in state.Location.GetNeighbors().Where(bounds.Contains))
        {
            switch (grid[neighbor.Y, neighbor.X])
            {
                case TerrainType.Rocky:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Wet when state.Equiped == EquipedTool.ClimbingGear:
                    yield return state with { Location = neighbor };
                    break;
                case TerrainType.Narrow when state.Equiped == EquipedTool.Torch:
                    yield return state with { Location = neighbor };
                    break;
            }
        }
    }

    private static TerrainType[,] CreateGrid(int depth, Point2 target)
    {
        var grid = new int[target.Y * 5, target.X * 5]; // increase the size

        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                var geologicalIndex = (x, y) switch
                {
                    (0, 0) => 0,
                    var (tx, ty) when tx == target.X && ty == target.Y => 0,
                    (0, _) => y * 48271,
                    (_, 0) => x * 16807,
                    _ => grid[y, x - 1] * grid[y - 1, x]
                };

                grid[y, x] = (geologicalIndex + depth) % 20183;
            }
        }

        var terrain = new TerrainType[grid.GetLength(0), grid.GetLength(1)];
        foreach (var kv in grid.AsEnumerable())
            terrain[kv.Key.Y, kv.Key.X] = (TerrainType)(kv.Value % 3);

        return terrain;
    }

    private static (int, Point2) ParseInput(string input)
    {
        var (depth, x, y) = input.Extract<int, int, int>(@$"depth: (\d+){Environment.NewLine}target: (\d+),(\d+)");
        return (depth, new(x, y));
    }

    private record State(Point2 Location, EquipedTool Equiped);

    private enum EquipedTool
    {
        Neither,
        Torch,
        ClimbingGear
    }

    private enum TerrainType
    {
        Rocky = 0,
        Wet = 1,
        Narrow = 2
    }
}