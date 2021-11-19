using AdventOfCode.Lib.ChallengeTypeProviders;
using AdventOfCode.Lib.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

await CreateHostBuilder(args)
                .RunConsoleAsync();

if (Debugger.IsAttached)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
}

static IHostBuilder CreateHostBuilder(string[] args) => Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        var challengeTypeProvider = new ReflectionChallengeTypeProvider();
        challengeTypeProvider.ScanAssembly();

        services.AddChallengeHost(challengeTypeProvider);
        services.AddChallengeInput(config =>
        {
            config.InputFolder = "Inputs";
        });
    });