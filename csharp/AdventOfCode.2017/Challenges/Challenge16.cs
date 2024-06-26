using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(16)]
public class Challenge16(IInputReader inputReader)
{
    private const string Dancers = "abcdefghijklmnop";

    [Part1]
    public async Task<string> Part1Async()
    {
        int[] programs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
        await foreach (var move in inputReader.ReadLineAsync(16, ','))
        {
            switch (move[0])
            {
                case 's':
                    Spin(programs, move[1..]);
                    break;
                case 'x':
                    Exchange(programs, move[1..]);
                    break;
                case 'p':
                    Partner(programs, move[1..]);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return Glue(programs);
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        int[] programs = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
        var moves = await inputReader.ReadLineAsync(16, ',').ToListAsync();
        var history = new List<string>();
        var cycleSet = new HashSet<(int, string)>();
        var i = 0;
        
        foreach (var move in moves.Cycle())
        {
            if (cycleSet.Contains((i % moves.Count, Glue(programs))))
                break;

            cycleSet.Add((i % moves.Count, Glue(programs)));
            history.Add(Glue(programs));
            i++;

            switch (move[0])
            {
                case 's':
                    Spin(programs, move[1..]);
                    break;
                case 'x':
                    Exchange(programs, move[1..]);
                    break;
                case 'p':
                    Partner(programs, move[1..]);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        var remaining = 1_000_000_000 % cycleSet.Count;
        return history[remaining];
    }

    private static void Spin(int[] programs, string move)
    {
        int x = int.Parse(move);
        for (int i = 0; i < programs.Length; i++)
            programs[i] = (programs[i] + x).Mod(programs.Length);
    }

    private static void Exchange(int[] programs, string move)
    {
        var pos = move.SplitBy("/").As<int>().Select(i => Array.IndexOf(programs, i)).ToArray();
        (programs[pos[0]], programs[pos[1]]) = (programs[pos[1]], programs[pos[0]]);
    }

    private static void Partner(int[] programs, string move)
    {
        var pos = move.SplitBy("/").As<char>().Select(c => Dancers.IndexOf(c)).ToArray();
        (programs[pos[0]], programs[pos[1]]) = (programs[pos[1]], programs[pos[0]]);
    }

    private static string Glue(int[] programs) => string.Join("", programs.Zip(Dancers.ToCharArray()).OrderBy(x => x.First).Select(x => x.Second));
}