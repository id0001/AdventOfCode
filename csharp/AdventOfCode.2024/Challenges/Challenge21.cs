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
            'v' => Face.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        })).ToArray();
    
    private static readonly Dictionary<char, Point2> Numpad = new()
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
    
    private static readonly Dictionary<char, Point2> Dirpad = new()
    {
        ['^'] = new Point2(1, 0),
        ['A'] = new Point2(2, 0),
        ['<'] = new Point2(0, 1),
        ['v'] = new Point2(1, 1),
        ['>'] = new Point2(2, 1),
    };

    [Part1]
    public async Task<string> Part1Async()
    {
        var codes = await inputReader.ReadLinesAsync(21).ToListAsync();
        
        var cache = new Dictionary<(char,char,int), long>();
        var solved = codes.Sum(code =>
        {
            var a = code.Extract(@"(\d+)")[0].As<int>();
            var numpadMoves = string.Join("", ("A" + code).Windowed(2).Select(w => MoveBetween(w[0], w[1], Numpad)));
            var sum = ("A" + numpadMoves).Windowed(2).Sum(w => GetCount(w[0], w[1], 2, Dirpad, cache));
            return sum * a;
        });

        return solved.ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var codes = await inputReader.ReadLinesAsync(21).ToListAsync();

        var cache = new Dictionary<(char,char,int), long>();
        var solved = codes.Sum(code =>
        {
            var a = code.Extract(@"(\d+)")[0].As<int>();
            var numpadMoves = string.Join("", ("A" + code).Windowed(2).Select(w => MoveBetween(w[0], w[1], Numpad)));
            var sum = ("A" + numpadMoves).Windowed(2).Sum(w => GetCount(w[0], w[1], 25, Dirpad, cache));
            return sum * a;
        });

        return solved.ToString();
    }

    private static long GetCount(char lastPos, char newPos, int level, Dictionary<char,Point2> keymap, Dictionary<(char, char, int level), long> cache)
    {
        if (cache.TryGetValue((lastPos, newPos, level), out var cachedValue))
            return cachedValue;

        var todo = MoveBetween(lastPos, newPos, keymap);
        if (level == 1)
            return todo.Length;

        var l = ("A" + todo).Windowed(2).Sum(w => GetCount(w[0], w[1], level - 1, keymap, cache));
        cache.Add((lastPos, newPos, level), l);
        return l;
    }

    private static string MoveBetween(char from, char to, Dictionary<char, Point2> keyMap)
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
}