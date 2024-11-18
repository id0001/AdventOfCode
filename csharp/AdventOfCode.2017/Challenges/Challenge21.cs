using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2017.Challenges;

[Challenge(21)]
public class Challenge21(IInputReader inputReader)
{
    private const string Start = @".#./..#/###";

    [Part1]
    public async Task<string> Part1Async()
    {
        var enhancements = await inputReader.ParseLinesAsync(21, ParseLine).SelectMany(x => x.ToAsyncEnumerable())
            .ToDictionaryAsync(kv => kv.Input, kv => kv.Output);
        var current = new Pattern(Start);

        for (var i = 0; i < 5; i++) current = new Pattern(current.Subdivide().Select(x => enhancements[x]));

        return current.ToString().Count(c => c == '#').ToString();
    }

    [Part2]
    public async Task<string> Part2Async()
    {
        var enhancements = await inputReader.ParseLinesAsync(21, ParseLine).SelectMany(x => x.ToAsyncEnumerable())
            .ToDictionaryAsync(kv => kv.Input, kv => kv.Output);
        var current = new Pattern(Start);

        for (var i = 0; i < 18; i++) current = new Pattern(current.Subdivide().Select(x => enhancements[x]));

        return current.ToString().Count(c => c == '#').ToString();
    }

    private static IEnumerable<Enhancement> ParseLine(string line)
    {
        var (input, output) = line.SplitBy("=>").Into(parts => (parts.First(), parts.Second()));

        return new[]
        {
            new Enhancement(new Pattern(input), new Pattern(output)),
            new Enhancement(new Pattern(input, 0, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 0, false, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 0, true, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 90), new Pattern(output)),
            new Enhancement(new Pattern(input, 90, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 90, false, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 90, true, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 180), new Pattern(output)),
            new Enhancement(new Pattern(input, 180, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 180, false, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 180, true, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 270), new Pattern(output)),
            new Enhancement(new Pattern(input, 270, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 270, false, true), new Pattern(output)),
            new Enhancement(new Pattern(input, 270, true, true), new Pattern(output))
        }.DistinctBy(x => x.Input);
    }

    private record Enhancement(Pattern Input, Pattern Output);

    private readonly record struct Pattern
    {
        private readonly string _pattern;

        public Pattern(string pattern, int angle = 0, bool flipHorizontally = false, bool flipVertically = false)
        {
            if (flipHorizontally && flipVertically)
                _pattern = Rotate(FlipHorizontally(FlipVertically(pattern)), angle);
            else if (flipHorizontally && !flipVertically)
                _pattern = Rotate(FlipHorizontally(pattern), angle);
            else if (!flipHorizontally && flipVertically)
                _pattern = Rotate(FlipVertically(pattern), angle);
            else
                _pattern = Rotate(pattern, angle);
        }

        public Pattern(IEnumerable<Pattern> parts)
        {
            var chunks = parts.Select(p => p.ToString().SplitBy("/")).ToArray();
            var largeSize = (int) Math.Sqrt(chunks.Length);
            var smallSize = chunks[0].Length;

            var fullPattern = new char[largeSize * smallSize, largeSize * smallSize];

            for (var yl = 0; yl < largeSize; yl++)
            for (var xl = 0; xl < largeSize; xl++)
            for (var ys = 0; ys < smallSize; ys++)
            for (var xs = 0; xs < smallSize; xs++)
            {
                var chunk = chunks[xl + yl * largeSize];
                fullPattern[yl * smallSize + ys, xl * smallSize + xs] = chunk[ys][xs];
            }

            _pattern = string.Join("/", fullPattern.ToJaggedArray().Select(x => new string(x)));
        }

        public bool Equals(Pattern other) => other._pattern == _pattern;

        public IEnumerable<Pattern> Subdivide()
        {
            var split = _pattern.SplitBy("/");
            if ((int) Math.Sqrt(_pattern.Length) % 2 == 0)
                for (var y = 0; y < split.Length; y += 2)
                for (var x = 0; x < split.Length; x += 2)
                    yield return new Pattern($"{split[y][x..(x + 2)]}/{split[y + 1][x..(x + 2)]}");
            else
                for (var y = 0; y < split.Length; y += 3)
                for (var x = 0; x < split.Length; x += 3)
                    yield return new Pattern(
                        $"{split[y][x..(x + 3)]}/{split[y + 1][x..(x + 3)]}/{split[y + 2][x..(x + 3)]}");
        }

        public override int GetHashCode() => HashCode.Combine(_pattern);

        public override string ToString() => _pattern;

        private static string FlipHorizontally(string pattern) =>
            string.Join("/", pattern.SplitBy("/").Select(s => string.Join("", s.Reverse())));

        private static string FlipVertically(string pattern) => string.Join("/", pattern.SplitBy("/").Reverse());

        private static string Rotate(string pattern, int angle)
        {
            var array = pattern.SplitBy("/").Select(x => x.ToCharArray()).ToArray();
            var h = array.Length;
            var w = array[0].Length;

            var result = new char[h][];
            for (var y = 0; y < h; y++)
            {
                result[y] = new char[w];
                for (var x = 0; x < w; x++)
                {
                    var (nx, ny) = RotatePoint(new Point2(x, y), w, h, angle);
                    result[y][x] = array[ny][nx];
                }
            }

            return string.Join("/", result.Select(s => new string(s)));
        }

        private static Point2 RotatePoint(Point2 p, int width, int height, int angle)
        {
            return angle switch
            {
                90 => new Point2(p.Y, width - p.X - 1),
                180 => new Point2(width - p.X - 1, height - p.Y - 1),
                270 => new Point2(height - p.Y - 1, p.X),
                _ => p
            };
        }
    }
}