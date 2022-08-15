﻿using System.Text;

namespace AdventOfCode.Core.IO;

public class InputReader : IInputReader
{
    private const string BasePath = "Inputs";

    public async Task<string> ReadAllTextAsync(int challenge) => await File.ReadAllTextAsync(GetPath(challenge));

    public async IAsyncEnumerable<char> ReadLineAsync(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));

        char[] buffer = new char[1024];
        while (!stream.EndOfStream)
        {
            int readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            for (int i = 0; i < readCount; i++)
            {
                yield return buffer[i];
            }
        }
    }

    public async IAsyncEnumerable<string> ReadLinesAsync(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));

        while (!stream.EndOfStream)
        {
            string line = await stream.ReadLineAsync();
            yield return line;
        }
    }

    public async IAsyncEnumerable<T> ReadLinesAsync<T>(int challenge)
    {
        using var stream = File.OpenText(GetPath(challenge));

        while (!stream.EndOfStream)
        {
            string line = await stream.ReadLineAsync();
            yield return (T)Convert.ChangeType(line, typeof(T));
        }
    }

    public async IAsyncEnumerable<string> ReadLineAsync(int challenge, char separator)
    {
        using var stream = File.OpenText(GetPath(challenge));

        char[] buffer = new char[1024];
        StringBuilder sb = new StringBuilder();
        while (!stream.EndOfStream)
        {
            int readCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            for (int i = 0; i < readCount; i++)
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

        char[] buffer = new char[1024];
        StringBuilder sb = new StringBuilder();
        while (!stream.EndOfStream)
        {
            int readCount = await stream.ReadAsync(buffer, 0, buffer.Length);

            for (int i = 0; i < readCount; i++)
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
        {
            string line = await stream.ReadLineAsync();
            lines.Add(line);
        }

        var map = new char[lines.Count, lines[0].Length];
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                map[y, x] = lines[y][x];
            }
        }

        return map;
    }

    public async Task<T[,]> ReadGridAsync<T>(int challenge)
    {
        var lines = new List<string>();
        using var stream = File.OpenText(GetPath(challenge));

        while (!stream.EndOfStream)
        {
            string line = await stream.ReadLineAsync();
            lines.Add(line);
        }

        var map = new T[lines.Count, lines[0].Length];
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                map[y, x] = (T)Convert.ChangeType(lines[y][x].ToString(), typeof(T));
            }
        }

        return map;
    }
    
    public async IAsyncEnumerable<T> ParseLinesAsync<T>(int challenge, Func<string, T> parser)
    {
        using var reader = File.OpenText(GetPath(challenge));

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if(line is not null)
                yield return parser.Invoke(line);
        }
    }

    private string GetPath(int challenge) => Path.Combine(BasePath, $"{challenge:D2}.txt");
}