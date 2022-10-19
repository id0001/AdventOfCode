using AdventOfCode.Lib;
using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib.Math;

namespace AdventOfCode2020.Challenges;

[Challenge(16)]
public class Challenge16
{
    private readonly IInputReader _inputReader;

    private readonly List<HashSet<int>> _ticketRules = new();
    private readonly List<int[]> _tickets = new();
    private int[] _yourTicket = Array.Empty<int>();

    public Challenge16(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var state = 0;

        await foreach (var line in _inputReader.ReadLinesAsync(16))
        {
            if (string.IsNullOrEmpty(line))
            {
                state++;
                continue;
            }

            if (line is "your ticket:" or "nearby tickets:")
                continue;

            if (state == 0) // rules
            {
                var match = Regex.Match(line, @".+: (\d+)-(\d+) or (\d+)-(\d+)");
                var s1 = int.Parse(match.Groups[1].Value);
                var e1 = int.Parse(match.Groups[2].Value);
                var s2 = int.Parse(match.Groups[3].Value);
                var e2 = int.Parse(match.Groups[4].Value);
                var set = new HashSet<int>();
                for (var i = s1; i <= e1; i++)
                    set.Add(i);

                for (var i = s2; i <= e2; i++)
                    set.Add(i);

                _ticketRules.Add(set);
            }
            else if (state == 1) // your ticket
            {
                _yourTicket = line.Split(",").Select(int.Parse).ToArray();
                _tickets.Add(_yourTicket);
            }
            else
            {
                var ticket = line.Split(",").Select(int.Parse).ToArray();
                _tickets.Add(ticket);
            }
        }
    }

    [Part1]
    public string Part1()
    {
        var invalid = 0;
        foreach (var ticket in _tickets)
        foreach (var t in ticket)
        {
            if (_ticketRules.Any(rule => rule.Contains(t))) continue;

            invalid += t;
            break;
        }

        return invalid.ToString();
    }

    [Part2]
    public string Part2()
    {
        var validTickets = GetValidTickets();
        var matrix = new int[20];

        for (var y = 0; y < 20; y++) // y = rule index
        for (var x = 0; x < 20; x++) // x = pos index
            if (_ticketRules[y].IsSupersetOf(validTickets.Select(ticket => ticket[x])))
                matrix[y] += 1 << x;

        var order = new int[20];
        var found = 0;
        var mask = (1 << 20) - 1;
        while (found < 20)
            for (var y = 0; y < matrix.Length; y++)
                if (Euclid.IsPowerOfTwo(matrix[y] & mask))
                {
                    var o = InversePow(matrix[y] & mask);
                    order[y] = o;
                    found++;
                    mask -= 1 << o;
                }

        return Enumerable.Range(0, 6).Select(i => (long)_yourTicket[order[i]]).Product().ToString();
    }

    private static int InversePow(int n) => (int)(Math.Log(n) / Math.Log(2));

    private List<int[]> GetValidTickets()
    {
        var validTickets = new List<int[]>();
        foreach (var ticket in _tickets)
        {
            var valid = ticket.All(t => _ticketRules.Any(rule => rule.Contains(t)));

            if (valid)
                validTickets.Add(ticket);
        }

        return validTickets;
    }
}