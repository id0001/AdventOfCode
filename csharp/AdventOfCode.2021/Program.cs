using AdventOfCode.Lib.ChallengeTypeProviders;
using AdventOfCode.Lib.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode2021
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .RunConsoleAsync();

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                var challengeTypeProvider = new ReflectionChallengeTypeProvider();
                challengeTypeProvider.ScanAssembly();

                services.AddChallengeHost(challengeTypeProvider);
                services.AddChallengeInput(config =>
                {
                    config.InputFolder = "Inputs";
                    config.UseFixed2DigitFiles = true;
                });
            });
    }
}
