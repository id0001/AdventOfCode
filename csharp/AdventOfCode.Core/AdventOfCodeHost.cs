using System.Reflection;
using DocoptNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

namespace AdventOfCode.Core;

public class AdventOfCodeHost : IHostedService
{
    private readonly Container _container;
    private readonly ILogger<AdventOfCodeHost> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly CommandLineParser _commandLineParser;
    private readonly Dictionary<int, Type> _challengeTypeMap;

    public AdventOfCodeHost(
        Container container,
        ILogger<AdventOfCodeHost> logger,
        IHostApplicationLifetime hostApplicationLifetime,
        CommandLineParser commandLineParser
        )
    {
        _container = container;
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _commandLineParser = commandLineParser;
        
        _challengeTypeMap = ScanAssemblyForChallengeTypes();
        _commandLineParser.RunLatest += OnRunLatestAsync;
        _commandLineParser.RunChallenge += OnRunChallengeAsync;
        _hostApplicationLifetime.ApplicationStarted.Register(OnApplicationStarted);
    }

    public static IHost Create(string[] args, Action<SimpleInjectorAddOptions> configureServices)
    {
        var container = new Container();
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddLogging();
                services.AddSimpleInjector(container, options =>
                {
                    options.AddHostedService<AdventOfCodeHost>();
                    options.AddLogging();
                    
                    options.Container.RegisterSingleton<CommandLineParser>();
                    
                    // Add challenges.
                    var challenges = ScanAssemblyForChallengeTypes();
                    foreach (var type in challenges.Values)
                        options.Container.Register(type);

                    configureServices(options);
                });
            })
            .ConfigureLogging((_, builder) => builder.AddConsole())
            .UseConsoleLifetime()
            .Build()
            .UseSimpleInjector(container);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application has started");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Application is stopping");
        return Task.CompletedTask;
    }
    
    private void OnApplicationStarted()
    {
        try
        {
            var args = Environment.GetCommandLineArgs().Skip(1).ToArray();

            _commandLineParser.Parse(args);
        }
        catch (DocoptInputErrorException ex)
        {
            Console.Error.WriteLine(ex.Message);
            _hostApplicationLifetime.StopApplication();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Input error");
            _hostApplicationLifetime.StopApplication();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");
            _hostApplicationLifetime.StopApplication();
        }
    }
    
    private async void OnRunLatestAsync(object? sender, EventArgs e)
    {
        if (_challengeTypeMap.Count == 0)
        {
            Console.WriteLine("finished.");
            _hostApplicationLifetime.StopApplication();
            return;
        }

        var (day, type) = _challengeTypeMap.MaxBy(x => x.Key);
        
        var executor = new ChallengeExecutor(_container, type);
        Console.WriteLine($"Day {day}:");

        try
        {
            // Run part 1
            await RunPart1(executor);

            // Run part 2
            await RunPart2(executor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "help");
        }

        Console.WriteLine("finished.");
        _hostApplicationLifetime.StopApplication();
    }
    
    private async void OnRunChallengeAsync(object? sender, int day)
    {
        if (_challengeTypeMap.Count == 0)
        {
            Console.WriteLine("finished.");
            _hostApplicationLifetime.StopApplication();
            return;
        }

        if (!_challengeTypeMap.TryGetValue(day, out var type))
            throw new InvalidOperationException($"Day not found: {day}");
        
        var executor = new ChallengeExecutor(_container, type);
        Console.WriteLine($"Day {day}:");

        try
        {
            // Run part 1
            await RunPart1(executor);

            // Run part 2
            await RunPart2(executor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "help");
        }

        Console.WriteLine("finished.");
        _hostApplicationLifetime.StopApplication();
    }
    
    private static async Task RunPart2(ChallengeExecutor executor)
    {
        var result = await executor.ExecutePart2Async();
        if (!result.IsEmpty)
            Console.WriteLine($"- Part 2 ({result.Duration.TotalMilliseconds:F6}ms): {result.Result}");
    }

    private static async Task RunPart1(ChallengeExecutor executor)
    {
        var result = await executor.ExecutePart1Async();
        if (!result.IsEmpty)
            Console.WriteLine($"- Part 1 ({result.Duration.TotalMilliseconds:F6}ms): {result.Result}");
    }
    
    private static Dictionary<int, Type> ScanAssemblyForChallengeTypes()
    {
        var challengeTypeMap = (from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let c = t.GetCustomAttribute<ChallengeAttribute>()
            where c is not null
            select (c, t)).ToDictionary(kv => kv.c!.Day, kv => kv.t);
        return challengeTypeMap;
    }
}