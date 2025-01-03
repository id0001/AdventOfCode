﻿namespace AdventOfCode.Core.IO;

public interface IInputReader
{
    Task<string> ReadAllTextAsync(int challenge);

    Task<T> ReadNumberAsync<T>(int challenge) where T : IParsable<T>;

    IAsyncEnumerable<char> ReadLineAsync(int challenge);

    Task<char[,]> ReadGridAsync(int challenge);
    Task<T[,]> ReadGridAsync<T>(int challenge);

    IAsyncEnumerable<string> ReadLinesAsync(int challenge);
    IAsyncEnumerable<T> ReadLinesAsync<T>(int challenge);

    IAsyncEnumerable<string> ReadLineAsync(int challenge, char separator);
    IAsyncEnumerable<T> ReadLineAsync<T>(int challenge, char separator);

    IAsyncEnumerable<T> ParseLinesAsync<T>(int challenge, Func<string, T> parser);
    Task<T> ParseTextAsync<T>(int challenge, Func<string, T> parser);
}