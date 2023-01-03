using System.Diagnostics;
using System.Reflection;
using SimpleInjector;

namespace AdventOfCode.Core;

public class ChallengeExecutor
{
    private readonly Container _container;
    private readonly Type _type;

    public ChallengeExecutor(Container container, Type type)
    {
        _container = container;
        _type = type;
    }

    public async Task<ChallengeExecutionResult> ExecutePart1Async()
    {
        var challenge = _container.GetInstance(_type);
        await SetupAsync(challenge);

        var sw = Stopwatch.StartNew();
        var result = await Part1Async(challenge);
        sw.Stop();
        return new ChallengeExecutionResult(sw.Elapsed, result);
    }

    public async Task<ChallengeExecutionResult> ExecutePart2Async()
    {
        var challenge = _container.GetInstance(_type);
        await SetupAsync(challenge);

        var sw = Stopwatch.StartNew();
        var result = await Part2Async(challenge);
        sw.Stop();
        return new ChallengeExecutionResult(sw.Elapsed, result);
    }

    private static async Task SetupAsync(object challenge)
    {
        var type = challenge.GetType();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod |
                                      BindingFlags.DeclaredOnly);
        var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<SetupAttribute>() != null);
        var result = runMethod?.Invoke(challenge, null);
        if (result is Task t)
            await t;
    }

    private static async Task<string?> Part1Async(object challenge)
    {
        var type = challenge.GetType();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod |
                                      BindingFlags.DeclaredOnly);
        var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part1Attribute>() != null);
        var result = runMethod?.Invoke(challenge, null);
        if (result is Task<string?> t)
            return await t;

        return (string?)result;
    }

    private static async Task<string?> Part2Async(object challenge)
    {
        var type = challenge.GetType();

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod |
                                      BindingFlags.DeclaredOnly);
        var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part2Attribute>() != null);
        var result = runMethod?.Invoke(challenge, null);
        if (result is Task<string?> t)
            return await t;

        return (string?)result;
    }
}

public record ChallengeExecutionResult(TimeSpan Duration, string? Result)
{
    public bool IsEmpty => string.IsNullOrEmpty(Result);
}