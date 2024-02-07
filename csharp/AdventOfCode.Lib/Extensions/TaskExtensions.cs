namespace AdventOfCode.Lib;

public static class TaskExtensions
{
    public static async Task<string> ToStringAsync<T>(this Task<T> task)
    {
        return (await task)?.ToString() ?? string.Empty;
    }

    public static async ValueTask<string> ToStringAsync<T>(this ValueTask<T> task)
    {
        return (await task)?.ToString() ?? string.Empty;
    }
}