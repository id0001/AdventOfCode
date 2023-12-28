using AdventOfCode.Core;
using AdventOfCode.Core.IO;
using AdventOfCode.Lib;

namespace AdventOfCode2021.Challenges;

[Challenge(20)]
public class Challenge20(IInputReader inputReader)
{
    private char[] _enhancement = Array.Empty<char>();
    private Dictionary<Point2, bool> _image = new();

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await inputReader.ReadLinesAsync(20).ToArrayAsync();
        _enhancement = lines[0].ToCharArray();

        lines = lines.Skip(2).ToArray();

        _image = lines.SelectMany((line, y) => line.Select((c, x) => (new Point2(x, y), c == '#')))
            .ToDictionary(kv => kv.Item1, kv => kv.Item2);
    }

    [Part1]
    public string Part1()
    {
        var outsideLit = false;
        for (var i = 0; i < 2; i++)
        {
            _image = Enhance(_image, _enhancement, outsideLit);
            outsideLit = !outsideLit;
        }

        return _image.Count(x => x.Value).ToString();
    }

    [Part2]
    public string Part2()
    {
        var outsideLit = false;
        for (var i = 0; i < 50; i++)
        {
            _image = Enhance(_image, _enhancement, outsideLit);
            outsideLit = !outsideLit;
        }

        return _image.Count(x => x.Value).ToString();
    }

    private static Dictionary<Point2, bool> Enhance(Dictionary<Point2, bool> image, char[] enhancementAlgorithm,
        bool outsideLit)
    {
        var newImage = new Dictionary<Point2, bool>();
        var left = image.Min(x => x.Key.X);
        var top = image.Min(x => x.Key.Y);
        var right = image.Max(x => x.Key.X);
        var bottom = image.Max(x => x.Key.Y);

        for (var y = top - 1; y <= bottom + 1; y++)
        for (var x = left - 1; x <= right + 1; x++)
        {
            var index = ToIndex(GetBits(x, y, image, outsideLit));
            newImage.Add(new Point2(x, y), enhancementAlgorithm[index] == '#');
        }

        return newImage;
    }

    private static IEnumerable<Point2> GetSquare3X3(Point2 center)
    {
        for (var y = -1; y <= 1; y++)
        for (var x = -1; x <= 1; x++)
            yield return new Point2(center.X + x, center.Y + y);
    }

    private static IEnumerable<char> GetBits(int x, int y, IReadOnlyDictionary<Point2, bool> image, bool outsideLit)
    {
        foreach (var point in GetSquare3X3(new Point2(x, y)))
        {
            if (!image.TryGetValue(point, out var value))
            {
                yield return outsideLit ? '1' : '0';
                continue;
            }

            yield return value ? '1' : '0';
        }
    }

    private static int ToIndex(IEnumerable<char> bits)
    {
        return Convert.ToInt32(new string(bits.ToArray()), 2);
    }
}