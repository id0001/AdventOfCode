using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2016.Challenges;

[Challenge(4)]
public class Challenge04(IInputReader inputReader)
{
    [Part1]
    public async Task<string> Part1Async()
    {
        return await inputReader.ParseLinesAsync(4, ParseLine).Where(msg => msg.IsValid).SumAsync(x => x.SectorId)
            .ToStringAsync();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        return await inputReader
            .ParseLinesAsync(4, ParseLine)
            .Where(msg => msg.IsValid && msg.Decrypted.StartsWith("northpole", StringComparison.OrdinalIgnoreCase))
            .Select(x => x.SectorId)
            .FirstOrDefaultAsync()
            .ToStringAsync();
    }

    private Message ParseLine(string line)
    {
        return line.SplitBy("-")
            .Into(x =>
            {
                var words = x[..^1];
                var (sectorId, checksum) = Regex.Match(x[^1], @"(\d+)\[([a-z]+)\]").Groups
                    .Into(groups => (groups[1].Value.As<int>(), groups[2].Value.ToHashSet()));
                return new Message(words, sectorId, checksum);
            });
    }

    private record Message(string[] Words, int SectorId, ISet<char> Checksum)
    {
        public bool IsValid
        {
            get
            {
                var mostCommon = Words
                    .SelectMany(w => w)
                    .GroupBy(c => c)
                    .Select(g => (g.Key, Count: g.Count()))
                    .GroupBy(g => g.Count, g => g.Key)
                    .OrderByDescending(x => x.Key)
                    .SelectMany(x => x.Order())
                    .ToArray();

                return mostCommon.Take(5).All(c => Checksum.Contains(c));
            }
        }

        public string Decrypted
        {
            get { return string.Join(' ', Words.Select(w => w.CaesarShift(SectorId))); }
        }
    }
}