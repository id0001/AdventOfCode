using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Challenges;

[Challenge(5)]
public class Challenge05
{
    private readonly IInputReader _inputReader;

    public Challenge05(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Part1]
    public async Task<string> Part1Async()
    {
        var almanac = MapInput(await _inputReader.ReadAllTextAsync(5));
        return almanac.Seeds.Select(almanac.FindBySeed).Min(x => x.Location).ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var almanac = MapInput(await _inputReader.ReadAllTextAsync(5));
        var seedRanges = almanac.Seeds.Chunk(2).Select(chunk => new LongRange(chunk[0], chunk[0] + chunk[1] - 1))
            .ToList();
        var largestLocation = almanac.HumidityToLocation.Max(x => x.Dest.End);

        for (long i = 0; i <= largestLocation; i++)
            if (seedRanges.Any(r => r.Contains(almanac.FindByLocation(i).Seed)))
                return i.ToString();

        throw new InvalidOperationException("Answer not found");
    }

    private Almanac MapInput(string input)
    {
        var nl = Environment.NewLine;
        return input
            .SplitBy($"{nl}{nl}")
            .Transform(parts =>
            {
                var seeds = parts[0].SplitBy(":").Second().SplitBy(" ").Select(long.Parse).ToArray();
                var seedsToSoil = ParsePart(parts[1]);
                var soilToFertilizer = ParsePart(parts[2]);
                var fertilizerToWater = ParsePart(parts[3]);
                var waterToLight = ParsePart(parts[4]);
                var lightToTemperature = ParsePart(parts[5]);
                var temperatureToHumidity = ParsePart(parts[6]);
                var humidityToLocation = ParsePart(parts[7]);

                return new Almanac
                {
                    Seeds = seeds,
                    SeedsToSoil = seedsToSoil,
                    SoilToFertilizer = soilToFertilizer,
                    FertilizerToWater = fertilizerToWater,
                    WaterToLight = waterToLight,
                    LightToTemperature = lightToTemperature,
                    TemperatureToHumidity = temperatureToHumidity,
                    HumidityToLocation = humidityToLocation
                };
            });
    }

    private List<Mapping> ParsePart(string part)
    {
        return part
            .SplitBy(":")
            .Second()
            .SplitBy("\r\n")
            .Select(x => x.SplitBy(" ").Transform(parts =>
                new
                {
                    DestStart = long.Parse(parts.First()),
                    SourceStart = long.Parse(parts.Second()),
                    Length = long.Parse(parts.Third())
                }))
            .Select(kv => new Mapping(new LongRange(kv.SourceStart, kv.SourceStart + kv.Length - 1),
                new LongRange(kv.DestStart, kv.DestStart + kv.Length - 1)))
            .OrderBy(x => x.Source.Start)
            .ToList();
    }

    private record Almanac
    {
        public required long[] Seeds { get; init; }
        public required List<Mapping> SeedsToSoil { get; init; }
        public required List<Mapping> SoilToFertilizer { get; init; }
        public required List<Mapping> FertilizerToWater { get; init; }
        public required List<Mapping> WaterToLight { get; init; }
        public required List<Mapping> LightToTemperature { get; init; }
        public required List<Mapping> TemperatureToHumidity { get; init; }
        public required List<Mapping> HumidityToLocation { get; init; }

        public AlmanacEntry FindBySeed(long seed)
        {
            var soil = MapValue(SeedsToSoil, seed);
            var fertilizer = MapValue(SoilToFertilizer, soil);
            var water = MapValue(FertilizerToWater, fertilizer);
            var light = MapValue(WaterToLight, water);
            var temperature = MapValue(LightToTemperature, light);
            var humidity = MapValue(TemperatureToHumidity, temperature);
            var location = MapValue(HumidityToLocation, humidity);
            return new AlmanacEntry(seed, location);
        }

        public AlmanacEntry FindByLocation(long location)
        {
            var humidity = ReverseMapValue(HumidityToLocation, location);
            var temperature = ReverseMapValue(TemperatureToHumidity, humidity);
            var light = ReverseMapValue(LightToTemperature, temperature);
            var water = ReverseMapValue(WaterToLight, light);
            var fertilizer = ReverseMapValue(FertilizerToWater, water);
            var soil = ReverseMapValue(SoilToFertilizer, fertilizer);
            var seed = ReverseMapValue(SeedsToSoil, soil);
            return new AlmanacEntry(seed, location);
        }

        private long MapValue(List<Mapping> map, long input)
        {
            var mapping = map.FirstOrDefault(x => x.Source.Contains(input));
            if (mapping == null)
                return input;

            return input - mapping.Source.Start + mapping.Dest.Start;
        }

        private long ReverseMapValue(List<Mapping> map, long input)
        {
            var mapping = map.FirstOrDefault(x => x.Dest.Contains(input));
            if (mapping == null)
                return input;

            return input - mapping.Dest.Start + mapping.Source.Start;
        }
    }

    private record AlmanacEntry(long Seed, long Location);

    private record Mapping(LongRange Source, LongRange Dest);

    private record LongRange(long Start, long End)
    {
        public bool Contains(long other) => other >= Start && other <= End;
    }
}