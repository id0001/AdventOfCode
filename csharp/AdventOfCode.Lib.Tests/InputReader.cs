using System.Text;

namespace AdventOfCode.Lib.Tests;

public static class InputReader
{
    public static async IAsyncEnumerable<string> ReadLinesAsync(string filename)
    {
        using var stream = File.OpenText(GetPath(filename));
        while (!stream.EndOfStream)
            yield return (await stream.ReadLineAsync())!;
    }

    public static async IAsyncEnumerable<T> ParseLinesAsync<T>(string filename, Func<string, T> parser)
    {
        using var reader = File.OpenText(GetPath(filename));

        while (!reader.EndOfStream)
            yield return parser.Invoke((await reader.ReadLineAsync())!);
    }

    public static async IAsyncEnumerable<T> ReadLineAsync<T>(string filename, char separator)
    {
        using var stream = File.OpenText(GetPath(filename));

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

    private static string GetPath(string filename) => Path.Combine("TestData", filename);
}