using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AdventOfCode.Core.IO;
using AdventOfCode.Core.Resources;
using DocoptNet;
using Spectre.Console;
using TextCopy;

namespace AdventOfCode.Core;

public class AdventOfCodeRunner
{
    private const string UsageResourceName = "AdventOfCode.Core.Resources.docopt.txt";
    private const string InputBasePath = "Inputs";
    private const string ConfigName = "aoc.json";

    private static readonly HttpClient SharedHttpClient = new()
    {
        BaseAddress = new Uri("https://adventofcode.com")
    };

    private readonly Table _container;
    private readonly string _usage;
    private readonly int _year;

    private AocConfig _config = new AocConfig();

    public AdventOfCodeRunner(int year)
    {
        _year = year;
        _usage = ResourceHelper.Read(UsageResourceName);

        _container = new Table().HideHeaders();
        _container.AddColumn(string.Empty);
        _container.Title = new TableTitle($"Advent of Code {year}");
    }

    public async Task RunAsync(string[] args)
    {
        try
        {
            _config = await ReadConfigAsync();
            EnsureSessionTokenExistsAsync();
            await ParseArgumentsAsync(args);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.ToString());
        }
    }

    private async void EnsureSessionTokenExistsAsync()
    {
        if (string.IsNullOrEmpty(_config.SessionToken))
        {
            Console.WriteLine("Please enter your session token:");
            var token = Console.ReadLine();
            if (!string.IsNullOrEmpty(token))
            {
                _config.SessionToken = token;
                await SaveConfigAsync(_config);
            }

            Console.Clear();
        }
    }

    private async Task<AocConfig> ReadConfigAsync()
    {
        if (!File.Exists(ConfigName))
            return new(); // Default config

        using var stream = File.OpenRead(ConfigName);
        return (await JsonSerializer.DeserializeAsync<AocConfig>(stream))!;
    }

    private async Task SaveConfigAsync(AocConfig config)
    {
        using var stream = File.OpenWrite(ConfigName);
        await JsonSerializer.SerializeAsync(stream, config);
    }

    private async Task ParseArgumentsAsync(string[] args)
    {
        var arguments = new Docopt().Apply(_usage, args, true, "1.0", true);
        if (arguments is null)
            return;

        if (!arguments["--day"].IsNullOrEmpty)
        {
            var day = arguments["--day"];
            if (!day.IsInt)
                throw new InvalidOperationException("<day> argument must be an integer");

            await RunDayAsync(day.AsInt);
            return;
        }

        if (arguments["--latest"].IsTrue)
        {
            await RunLatestAsync();
            return;
        }

        if (arguments["--all"].IsTrue) await RunAllAsync();
    }

    private async Task RunDayAsync(int day)
    {
        var type = FindDay(day);
        if (type is null)
            return;

        await RunChallenge(day, type);

        AnsiConsole.Write(_container);
    }

    private async Task RunLatestAsync()
    {
        var (day, type) = FindLatestDay();

        await RunChallenge(day, type);
        AnsiConsole.Write(_container);
    }

    private async Task RunAllAsync()
    {
        var answers = JsonNode.Parse(await File.ReadAllTextAsync("answers.json"));
        if (answers is null)
            throw new InvalidOperationException("Unable to parse answers file");

        foreach (var (day, type) in FindAllDays())
        {
            await RunChallenge(day, type);

            AnsiConsole.Clear();
            AnsiConsole.Write(_container);
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(_container);
    }

    private static Table CreateTable(bool benchmark, int day)
    {
        var table = new Table().Expand();
        table.Title = new TableTitle($"Day {day}");
        table.AddColumn(new TableColumn("Part").Centered());
        table.AddColumn(new TableColumn("Result"));
        table.AddColumn(new TableColumn("Time (first run)"));

        if (benchmark)
            table.AddColumn(new TableColumn("Benchmark (avg over 100 runs)"));

        table.AddColumn(new TableColumn("Is Correct").Centered());
        return table;
    }

    private async Task RunChallenge(int day, Type type)
    {
        var benchmark = ShouldBenchmark();

        if (!string.IsNullOrEmpty(_config.SessionToken))
            await DownloadInputIfNotExistsAsync(_year, day, _config.SessionToken);

        var setupMethod = GetSetup(type);
        var part1Method = GetPart1(type);
        var part2Method = GetPart2(type);

        var answers = JsonNode.Parse(await File.ReadAllTextAsync("answers.json"));
        if (answers is null)
            throw new InvalidOperationException("Unable to parse answers file");

        var table = CreateTable(benchmark, day);

        if (part1Method is not null)
            await RunPart(day, 1, benchmark, answers, type, setupMethod, part1Method, table);

        if (part2Method is not null)
            await RunPart(day, 2, benchmark, answers, type, setupMethod, part2Method, table);

        _container.AddRow(table);
    }

    private static async Task RunPart(int day, int part, bool benchmark, JsonNode? answers, Type type,
        MethodInfo? setupMethod, MethodInfo part1Method, Table table)
    {
        var instance = CreateInstance(type);

        if (instance is null)
            throw new InvalidOperationException("Unable to instantiate challenge");

        if (setupMethod is not null)
        {
            var setupResult = setupMethod.Invoke(instance, null);
            if (setupResult is Task t)
                await t;
        }

        var items = new List<Text>();
        var (result, time) = await Benchmark(() => GetPartResult(instance, part1Method), 1);
        if (result is not null)
        {
            await ClipboardService.SetTextAsync(result);
            items.Add(new Text($"{part}"));
            items.Add(new Text(result));
            items.Add(new Text(time.ToString()));

            if (benchmark)
                try
                {
                    (_, time) = await Benchmark(() => GetPartResult(instance, part1Method), 100,
                        TimeSpan.FromSeconds(60));
                    items.Add(new Text(time.ToString()));
                }
                catch (TimeoutException ex)
                {
                    items.Add(new Text(ex.Message, new Style(Color.Orange1)));
                }

            var answer = answers?[day.ToString()]?[part.ToString()]?.GetValue<string>();
            items.Add(string.IsNullOrEmpty(answer)
                ? new Text("-")
                : result == Regex.Replace(answer, @"(?:\r\n)|\n", Environment.NewLine)
                    ? new Text("Yes", new Style(Color.Green))
                    : new Text("No", new Style(Color.Red)));

            table.AddRow(new TableRow(items));
        }
    }

    private static async Task DownloadInputIfNotExistsAsync(int year, int day, string sessionToken)
    {
        var path = GetInputPath(day);
        if (File.Exists(path) && !string.IsNullOrEmpty(await File.ReadAllTextAsync(path)))
            return;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
        request.Headers.Add("Cookie", $"session={sessionToken}");
        var response = await SharedHttpClient.SendAsync(request);
        var responseMessage = await response.Content.ReadAsStringAsync();

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await File.WriteAllTextAsync(path, responseMessage);
    }

    private static async Task<(string?, TimeSpan)> Benchmark(Func<Task<string?>> func, int runAmount,
        TimeSpan? timeout = null)
    {
        string? result = null;
        var stopwatch = new Stopwatch();
        var avg = TimeSpan.Zero;
        for (var i = 0; i < runAmount; i++)
        {
            stopwatch.Restart();
            result = await func();
            stopwatch.Stop();
            avg += stopwatch.Elapsed;

            if (timeout.HasValue && stopwatch.Elapsed > timeout / runAmount)
                throw new TimeoutException("Too long for benchmark");
        }

        return (result, avg / runAmount);
    }

    private static Type? FindDay(int day) =>
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Select(t => new { Type = t, Challenge = t.GetCustomAttribute<ChallengeAttribute>() })
            .FirstOrDefault(x => x.Challenge is not null && x.Challenge.Day == day)?
            .Type;

    private static (int, Type) FindLatestDay()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Select(t => new { Type = t, Challenge = t.GetCustomAttribute<ChallengeAttribute>() })
            .Where(x => x.Challenge is not null)
            .OrderByDescending(x => x.Challenge!.Day)
            .Select(x => (x.Challenge!.Day, x.Type))
            .FirstOrDefault();
    }

    private static IEnumerable<(int, Type)> FindAllDays()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(GetLoadableTypes)
            .Select(t => new { Type = t, Challenge = t.GetCustomAttribute<ChallengeAttribute>() })
            .Where(x => x.Challenge is not null)
            .OrderBy(x => x.Challenge!.Day)
            .Select(x => (x.Challenge!.Day, x.Type));
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        if (assembly is null)
            throw new ArgumentNullException(nameof(assembly));

        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null).Select(t => t!);
        }
    }

    private static object? CreateInstance(Type type)
    {
        var ctor = type.GetConstructor(new[] { typeof(IInputReader) });
        return ctor is not null ? Activator.CreateInstance(type, new InputReader()) : Activator.CreateInstance(type);
    }

#pragma warning disable CS0612 // Type or member is obsolete
    private static MethodInfo? GetSetup(Type type) => GetMethod<SetupAttribute>(type);
#pragma warning restore CS0612 // Type or member is obsolete

    private static MethodInfo? GetPart1(Type type) => GetMethod<Part1Attribute>(type);

    private static MethodInfo? GetPart2(Type type) => GetMethod<Part2Attribute>(type);

    private static MethodInfo? GetMethod<T>(Type type) where T : Attribute
    {
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod |
                                      BindingFlags.DeclaredOnly);

        return methods.FirstOrDefault(m => m.GetCustomAttribute<T>() != null);
    }

    private static async Task<string?> GetPartResult(object instance, MethodInfo method)
    {
        var result = method.Invoke(instance, null);
        if (result is Task<string?> task)
            return await task;

        return (string?)result;
    }

    private static string GetInputPath(int day) => Path.Combine(InputBasePath, $"{day:D2}.txt");

    private static bool ShouldBenchmark()
    {
#if DEBUG
        return false;
#else
            return true;
#endif
    }
}