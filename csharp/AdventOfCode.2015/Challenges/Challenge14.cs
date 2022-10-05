using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2015.Challenges;

[Challenge(14)]
public class Challenge14
{
    private static readonly Regex Pattern =
        new(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds.");
    
    private readonly IInputReader _inputReader;

    public Challenge14(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string?> Part1Async() => await _inputReader.ParseLinesAsync(14, ParseLine).Select(r => r.DistanceTraveledAfter(2503)).MaxAsync().ToStringAsync();

    [Part2]
    public async Task<string> Part2Async()
    {
        var reindeer = await _inputReader.ParseLinesAsync(14, ParseLine).ToListAsync();

        var score = reindeer.ToDictionary(kv => kv, _ => 0);
        var distanceTraveled = reindeer.ToDictionary(kv => kv, _ => 0);
        for (var s = 1; s <= 2503; s++)
        {
            foreach (var deer in reindeer)
            {
                distanceTraveled[deer] = deer.DistanceTraveledAfter(s);
            }

            var max = distanceTraveled.Values.Max();
            foreach (var deer in reindeer.Where(x => distanceTraveled[x] == max))
            {
                score[deer]++;
            }
        }

        return score.Values.Max().ToString();
    }

    private static Reindeer ParseLine(string line)
    {
        // Vixen can fly 8 km/s for 8 seconds, but then must rest for 53 seconds.
        var match = Pattern.Match(line);

        var sps = int.Parse(match.Groups[2].Value);
        var sbr = int.Parse(match.Groups[3].Value);
        var rest = int.Parse(match.Groups[4].Value);
        return new Reindeer(sps, sbr, rest);
    }

    private record Reindeer(int SpeedPerSecond, int SecondsBeforeRest, int Rest)
    {
        public int DistanceTraveledAfter(int seconds)
        {
            var fullCycles = seconds / (SecondsBeforeRest + Rest); // Calculate the amount of full cycles.
            var remainder = seconds - fullCycles * (SecondsBeforeRest + Rest); // Calculate the remaining seconds.
            var distance = fullCycles * SpeedPerSecond * SecondsBeforeRest; // Calculate the distance traveled so far.
            distance += Math.Min(remainder, SecondsBeforeRest) * SpeedPerSecond; // Add remaining possible distance.
            return distance;
        }
    }
}