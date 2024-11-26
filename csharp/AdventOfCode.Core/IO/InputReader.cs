using System.Globalization;
using System.Text;

namespace AdventOfCode.Core.IO;

public class InputReader : IInputReader
{
    private const string BasePath = "Inputs";

    public async Task<string> ReadAllTextAsync(int challenge) => await File.ReadAllTextAsync(GetPath(challenge));

    public async Task<T> ReadNumberAsync<T>(int challenge)
        where T : IParsable<T>
        => T.Parse(await File.ReadAllTextAsync(GetPath(challenge)), CultureInfo.InvariantCulture);

    public async IAsyncEnumerable<char> ReadLineAsync(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));

        var buffer = new char[1024];
        while (!stream.EndOfStream)
        {
            var readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            for (var i = 0; i < readCount; i++)
                yield return buffer[i];
        }
    }

    public async IAsyncEnumerable<string> ReadLinesAsync(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));
        while (!stream.EndOfStream)
            yield return (await stream.ReadLineAsync())!;
    }

    public async IAsyncEnumerable<T> ReadLinesAsync<T>(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));
        while (!stream.EndOfStream)
            yield return (T)Convert.ChangeType((await stream.ReadLineAsync())!, typeof(T));
    }

    public async IAsyncEnumerable<string> ReadLineAsync(int challenge, char separator)
    {
        using var stream = File.OpenText(GetPath(challenge));

        var buffer = new char[1024];
        var sb = new StringBuilder();
        while (!stream.EndOfStream)
        {
            var readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            for (var i = 0; i < readCount; i++)
            {
                if (buffer[i] == separator)
                {
                    yield return sb.ToString();
                    sb.Clear();
                    continue;
                }

                sb.Append(buffer[i]);
            }
        }

        if (sb.Length > 0)
            yield return sb.ToString();
    }

    public async IAsyncEnumerable<T> ReadLineAsync<T>(int challenge, char separator)
    {
        using var stream = File.OpenText(GetPath(challenge));

        var buffer = new char[1024];
        var sb = new StringBuilder();
        while (!stream.EndOfStream)
        {
            var readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            for (var i = 0; i < readCount; i++)
            {
                if (buffer[i] == separator)
                {
                    yield return (T)Convert.ChangeType(sb.ToString(), typeof(T));
                    sb.Clear();
                    continue;
                }

                sb.Append(buffer[i]);
            }
        }

        if (sb.Length > 0)
            yield return (T)Convert.ChangeType(sb.ToString(), typeof(T));
    }

    public async Task<char[,]> ReadGridAsync(int challenge)
    {
        var lines = new List<string>();
        using var stream = File.OpenText(GetPath(challenge));

        while (!stream.EndOfStream)
            lines.Add((await stream.ReadLineAsync())!);

        var map = new char[lines.Count, lines[0].Length];
        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
                map[y, x] = lines[y][x];

        return map;
    }

    public async Task<T[,]> ReadGridAsync<T>(int challenge)
    {
        var lines = new List<string>();
        using var stream = File.OpenText(GetPath(challenge));

        while (!stream.EndOfStream)
            lines.Add((await stream.ReadLineAsync())!);

        var map = new T[lines.Count, lines[0].Length];
        for (var y = 0; y < map.GetLength(0); y++)
            for (var x = 0; x < map.GetLength(1); x++)
                map[y, x] = (T)Convert.ChangeType(lines[y][x].ToString(), typeof(T));

        return map;
    }

    public async IAsyncEnumerable<T> ParseLinesAsync<T>(int challenge, Func<string, T> parser)
    {
        using var reader = File.OpenText(GetPath(challenge));

        while (!reader.EndOfStream)
            yield return parser.Invoke((await reader.ReadLineAsync())!);
    }

    public async Task<T> ParseTextAsync<T>(int challenge, Func<string, T> parser)
        => parser.Invoke(await ReadAllTextAsync(challenge));

    private static string GetPath(int challenge) => Path.Combine(BasePath, $"{challenge:D2}.txt");
}