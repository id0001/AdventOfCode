using System.Text;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2024.Challenges;

[Challenge(21)]
public class Challenge21(IInputReader inputReader)
{
    private static readonly (char Char, Point2 Direction)[] Directions = "<v^>"
        .Select(c => (c, c switch
        {
            '^' => Face.Up,
            '>' => Face.Right,
            '<' => Face.Left,
            'v' => Face.Down
        })).ToArray();
    
    private static readonly string[] Numpad =
    [
        "789",
        "456",
        "123",
        " 0A"
    ];

    private static readonly string[] DirectionPad =
    [
        " ^A",
        "<v>"
    ];

    //[Part1]
    public async Task<string> Part1Async()
    {
        var codes = await inputReader.ReadLinesAsync(21).ToListAsync();

        var numpad = new Dictionary<char, Point2>()
        {
            ['7'] = new Point2(0, 0),
            ['8'] = new Point2(1, 0),
            ['9'] = new Point2(2, 0),
            ['4'] = new Point2(0, 1),
            ['5'] = new Point2(1, 1),
            ['6'] = new Point2(2, 1),
            ['1'] = new Point2(0, 2),
            ['2'] = new Point2(1, 2),
            ['3'] = new Point2(2, 2),
            ['0'] = new Point2(1, 3),
            ['A'] = new Point2(2, 3)
        };

        var dirpad = new Dictionary<char, Point2>()
        {
            ['^'] = new Point2(1, 0),
            ['A'] = new Point2(2, 0),
            ['<'] = new Point2(0, 1),
            ['v'] = new Point2(1, 1),
            ['>'] = new Point2(2, 1),
        };

        var shortestNumpadPaths = GetShortestPaths(numpad);
        var shortestDirpadPaths = GetShortestPaths(dirpad);

        // Keypad robot -> Directional robot -> Directional robot -> Me

        var cache = new Dictionary<string, List<string>>();
        return codes.Select(c =>
                CalculateShortestPathLength(shortestNumpadPaths, shortestDirpadPaths, c, cache) *
                c.Extract(@"(\d+)")[0].As<int>())
            .Sum().ToString();

        // foreach (var code in codes)
        // {
        //     var t1 = CalculateNextSequence(shortestNumpadPaths, "A" + code);
        //     var t2 = t1.SelectMany(s => CalculateNextSequence(shortestDirpadPaths, "A" + s)).ToList();
        //     var t3 = t2.SelectMany(s => CalculateNextSequence(shortestDirpadPaths, "A" + s)).ToList();
        //     //var t4 = t3.SelectMany(s => CalculateNextSequence(shortestDirpadPaths, "A" + s)).ToList();
        //
        //     var shortest = t3.MinBy(s => s.Length);
        //
        //     // var t1 = CalculateNextSequence(shortestNumpadPaths, code);
        //     // var t2 = CalculateNextSequence(shortestDirpadPaths, t1);
        //     // var t3 = CalculateNextSequence(shortestDirpadPaths, t2);
        //     //
        //     // var numpart = code.Extract(@"(\d+)")[0].As<int>();
        //     // var complexity = numpart * t3.Length;
        // }

        return string.Empty;
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var codes = await inputReader.ReadLinesAsync(21).ToListAsync();

        var numpad = new Dictionary<char, Point2>()
        {
            ['7'] = new Point2(0, 0),
            ['8'] = new Point2(1, 0),
            ['9'] = new Point2(2, 0),
            ['4'] = new Point2(0, 1),
            ['5'] = new Point2(1, 1),
            ['6'] = new Point2(2, 1),
            ['1'] = new Point2(0, 2),
            ['2'] = new Point2(1, 2),
            ['3'] = new Point2(2, 2),
            ['0'] = new Point2(1, 3),
            ['A'] = new Point2(2, 3)
        };

        var dirpad = new Dictionary<char, Point2>()
        {
            ['^'] = new Point2(1, 0),
            ['A'] = new Point2(2, 0),
            ['<'] = new Point2(0, 1),
            ['v'] = new Point2(1, 1),
            ['>'] = new Point2(2, 1),
        };

        var shortestNumpadPaths = GetShortestPaths(numpad);
        var shortestDirpadPaths = GetShortestPaths(dirpad);
        shortestDirpadPaths[('>', '^')] = ["<^A"];
        shortestDirpadPaths[('^', '>')] = ["v>A"];
        shortestDirpadPaths[('A', 'v')] = ["<vA"];
        shortestDirpadPaths[('v', 'A')] = ["^>A"];

        var cache = new Dictionary<(char,char,int), long>();

        var solved = codes.Sum(code =>
        {
            var a = code.Extract(@"(\d+)")[0].As<int>();
            var numpadMoves = string.Join("", ("A" + code).Windowed(2).Select(w => Transition(w[0], w[1], numpad)));
            var sum = ("A" + numpadMoves).Windowed(2).Sum(w => GetCount(w[0], w[1], 25, dirpad, cache));
            return sum * a;
        });

        

        return solved.ToString();
        // return codes.Select(c =>
        //         CalculateShortestPathLength2(shortestNumpadPaths, shortestDirpadPaths, c, cache) *
        //         c.Extract(@"(\d+)")[0].As<int>())
        //     .Sum().ToString();
    }

    private long GetCount(char lastPos, char newPos, int level, Dictionary<char,Point2> keymap, Dictionary<(char, char, int level), long> cache)
    {
        if (cache.TryGetValue((lastPos, newPos, level), out var cachedValue))
            return cachedValue;

        var todo = Transition(lastPos, newPos, keymap);
        if (level == 1)
            return todo.Length;

        var l = ("A" + todo).Windowed(2).Sum(w => GetCount(w[0], w[1], level - 1, keymap, cache));
        cache.Add((lastPos, newPos, level), l);
        return l;
    }

    public string Transition(char from, char to, Dictionary<char, Point2> keyMap)
    {
        var sb = new StringBuilder();
        var target = keyMap[to];
        var curPos = keyMap[from];
        var delta = target - curPos;
        var d = 0;

        while (delta != Point2.Zero)
        {
            var (dirChar, dir) = Directions[(d++ % Directions.Length)];
            var amount = dir.X == 0 ? delta.Y / dir.Y : delta.X / dir.X;
            if (amount <= 0)
                continue;
            var dest = curPos + dir * amount;
            if (!keyMap.ContainsValue(dest))
                continue;
            curPos = dest;
            delta -= dir * amount;
            sb.Append(new string(dirChar, amount));
        }
        sb.Append('A');
        return sb.ToString();
    }
    
    // private string ShortestPresses(string target, Point2 current, string moves)
    // {
    //     if (target.Length == 0)
    //         return moves;
    //
    //     if (Numpad[current.Y][current.X] == target[0])
    //         return ShortestPresses(target[1..], current, moves + 'A');
    //     
    //     // foreach(var dir in [Face.Left, Face.Right, Face])
    // }

    private int CalculateShortestPathLength(Dictionary<(char, char), List<string>> numpad,
        Dictionary<(char, char), List<string>> dirpad, string code, Dictionary<string, List<string>> cache)
    {
        var t1 = CalculateNextSequence(numpad, "A" + code, new Dictionary<string, List<string>>())
            .OrderBy(t1 => t1.Length)
            .ToList();
        var t2 = t1.SelectMany(s => CalculateNextSequence(dirpad, "A" + s, cache))
            .OrderBy(t2 => t2.Length)
            .ToList();
        
        var t3 = t2.SelectMany(s => CalculateNextSequence(dirpad, "A" + s, cache)).ToList();
        return t3.MinBy(s => s.Length)!.Length;
    }
    
    private int CalculateShortestPathLength2(Dictionary<(char, char), List<string>> numpad,
        Dictionary<(char, char), List<string>> dirpad, string code, Dictionary<string, List<string>> cache)
    {
        var t1 = CalculateNextSequence(numpad, "A" + code, new Dictionary<string, List<string>>());

        var t2 = t1.ToList();
        for (var i = 0; i < 25; i++)
        {
            t2 = t2.SelectMany(s => CalculateNextSequence(dirpad, "A" + s, cache)).ToList();
        }
        var t3 = t2.SelectMany(s => CalculateNextSequence(dirpad, "A" + s, cache)).ToList();
        return t3.MinBy(s => s.Length)!.Length;
    }
    
    private List<string> CalculateNextSequence(Dictionary<(char, char), List<string>> shortestPaths, string sequence, Dictionary<string, List<string>> cache)
    {
        if (cache.TryGetValue(sequence, out var cachedValue))
            return cachedValue;
        
        if (sequence.Length == 2)
            return shortestPaths[(sequence[0], sequence[1])].ToList();

        var list = new List<string>();
        foreach (var path in shortestPaths[(sequence[0], sequence[1])])
        {
            foreach(var p in CalculateNextSequence(shortestPaths, sequence[1..], cache))
                list.Add(p + path);
        }

        cache.Add(sequence, list);
        return list;
    }

    private static Dictionary<(char, char), List<string>> GetShortestPaths(Dictionary<char, Point2> pad)
    {
        var keys = pad.Keys.ToList();

        var shortestPaths = new Dictionary<(char, char), List<string>>();
        foreach (var from in keys)
        {
            foreach (var to in keys)
            {
                var astar = new BreadthFirstSearch<int, Point2>(0, pad[from], n => GetAdjacent(pad, n))
                    .WithWeight((_, _) => 1);

                var target = pad[to];
                var shortest = astar.FindAll(n => target == n)
                    .GroupBy(n => n.Cost)
                    .OrderBy(n => n.Key)
                    .First()
                    .Select(n => GetSequence(n.Path))
                    .ToList();
                
                shortestPaths.Add((from, to), shortest);
            }
        }

        return shortestPaths;
    }

    private static string GetSequence(IList<Point2> moves)
    {
        var sb = new StringBuilder();
        foreach (var window in moves.Windowed(2))
        {
            var diff = window[1] - window[0];
            sb.Append(diff switch
            {
                (1, 0) => '>',
                (-1, 0) => '<',
                (0, 1) => 'v',
                (0, -1) => '^',
                _ => throw new NotImplementedException()
            });
        }

        sb.Append('A');

        return sb.ToString();
    }

    private string FindShortestNumpadPath(string code)
    {
        var start = new NumpadState(new Point2(2, 3), string.Empty);
        var bfs = new BreadthFirstSearch<string, NumpadState>(code, start, GetNumpadAdjacent);
        var path = bfs.FindShortest(n => n.Pressed == code);

        var sb = new StringBuilder();
        foreach (var window in path.Path.Windowed(2))
        {
            var diff = window[1].Position - window[0].Position;
            sb.Append(diff switch
            {
                (1, 0) => '>',
                (-1, 0) => '<',
                (0, 1) => 'v',
                (0, -1) => '^',
                _ => 'A'
            });
        }

        return sb.ToString();
    }

    private string FindShortestDirectionPath(string sequence)
    {
        var start = new NumpadState(new Point2(2,0), string.Empty);
        var bfs = new BreadthFirstSearch<string, NumpadState>(sequence, start, GetDirectionAdjacent);
        var path = bfs.FindShortest(n => n.Pressed == sequence);

        var sb = new StringBuilder();
        foreach (var window in path.Path.Windowed(2))
        {
            var diff = window[1].Position - window[0].Position;
            sb.Append(diff switch
            {
                (1, 0) => '>',
                (-1, 0) => '<',
                (0, 1) => 'v',
                (0, -1) => '^',
                _ => 'A'
            });
        }

        return sb.ToString();
    }

    private static IEnumerable<Point2> GetAdjacent(Dictionary<char, Point2> dict, Point2 current)
    {
        foreach (var neighbor in current.GetNeighbors())
        {
            if(!dict.ContainsValue(neighbor))
                continue;

            yield return neighbor;
        }
    }

    private static IEnumerable<NumpadState> GetNumpadAdjacent(NumpadState current)
    {
        var bounds = new Rectangle(0, 0, 3, 4);
        foreach (var neighbor in current.Position.GetNeighbors())
        {
            if (neighbor == new Point2(0, 3))
                continue;

            if (!bounds.Contains(neighbor))
                continue;

            yield return current with {Position = neighbor};
        }

        var c = Numpad[current.Position.Y][current.Position.X];
        if (current.Pressed.Length > 0 && current.Pressed[^1] == c)
            yield break;

        yield return current with {Pressed = current.Pressed + Numpad[current.Position.Y][current.Position.X]};
    }

    private static IEnumerable<NumpadState> GetDirectionAdjacent(NumpadState current)
    {
        var bounds = new Rectangle(0, 0, 3, 2);
        foreach (var neighbor in current.Position.GetNeighbors())
        {
            if (neighbor == new Point2(0, 0))
                continue;

            if (!bounds.Contains(neighbor))
                continue;

            yield return current with {Position = neighbor};
        }

        var c = DirectionPad[current.Position.Y][current.Position.X];
        yield return current with {Pressed = current.Pressed + c};
    }

    private record NumpadState(Point2 Position, string Pressed);
}