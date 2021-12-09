using AdventOfCode.Lib.Properties;
using DocoptNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Lib
{
    internal static class ChallengeHelper
	{
		public static async Task SetupAsync(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<SetupAttribute>() != null);
			if (runMethod != null)
			{
				var result = runMethod.Invoke(challenge, null);
				if (result != null && result is Task t)
				{
					await t;
				}
			}
		}

		public static async Task<string> Part1Async(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part1Attribute>() != null);
			if (runMethod != null)
			{
				var result = runMethod.Invoke(challenge, null);
				if (result != null && result is Task<string> t)
				{
					return await t;
				}

				return (string)result;
			}

			return null;
		}

		public static async Task<string> Part2Async(object challenge)
		{
			Type type = challenge.GetType();

			var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.DeclaredOnly);
			var runMethod = methods.FirstOrDefault(m => m.GetCustomAttribute<Part2Attribute>() != null);
			if (runMethod != null)
			{
				var result = runMethod.Invoke(challenge, null);
				if (result != null && result is Task<string> t)
				{
					return await t;
				}

				return (string)result;
			}

			return null;
		}
	}

	public class ChallengeHost : IHostedService
	{
		private readonly IServiceProvider serviceProvider;
		private readonly ILogger logger;
		private readonly IHostApplicationLifetime hostApplicationLifetime;
		private readonly IChallengeTypeProvider challengeTypeProvider;

		private ErrorCode exitCode = ErrorCode.Success;

		public ChallengeHost(IServiceProvider serviceProvider, ILogger<ChallengeHost> logger, IHostApplicationLifetime hostApplicationLifetime, IChallengeTypeProvider challengeTypeProvider)
		{
			this.serviceProvider = serviceProvider;
			this.logger = logger;
			this.hostApplicationLifetime = hostApplicationLifetime;
			this.challengeTypeProvider = challengeTypeProvider;

			hostApplicationLifetime.ApplicationStarted.Register(async () => await OnApplicationStarted());
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			Environment.ExitCode = (int)exitCode;
			return Task.CompletedTask;
		}

		private async Task OnApplicationStarted()
		{
			try
			{
				logger.LogDebug("Application started.");

				var args = Environment.GetCommandLineArgs().Skip(1).ToArray(); // Remove application path
				var usage = Resources.Docopt;
				var arguments = new Docopt().Apply(usage, args, true, "1.0", true, false);
				if (arguments == null)
				{
					return;
				}

				if (arguments["--challenge"] != null)
				{
					if (!arguments["--challenge"].IsInt)
					{
						logger.LogError("<day> argument must be an integer.");
						exitCode = ErrorCode.InvalidStartupArgument;
						return;
					}

					await RunChallenge(arguments["--challenge"].AsInt);
				}
				else if (arguments["--latest"].IsTrue)
				{
					await RunLatest();
				}
				else if (arguments["--all"].IsTrue)
				{
					await RunAll();
				}
			}
			catch (DocoptInputErrorException ex)
			{
				Console.WriteLine(ex.Message);
				await Task.Delay(3000);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Unhandled exception.");
			}
			finally
			{
				hostApplicationLifetime.StopApplication();
			}
		}

		private async Task RunChallenge(int day)
		{
			var sw = new Stopwatch();

			if (!challengeTypeProvider.TryGetValue(day, out Type challengeType))
			{
				logger.LogWarning($"Challenge for day {day} does not exist.");
				return;
			}

			Console.WriteLine($"Day {day}:");

			sw.Start();
			string part1 = await ExecutePart1(challengeType);
			sw.Stop();

			if (!string.IsNullOrEmpty(part1))
				Console.WriteLine($"- Part 1 ({sw.Elapsed.TotalMilliseconds:F3}ms): {part1}");

			sw.Restart();
			string part2 = await ExecutePart2(challengeType);
			sw.Stop();

			if (!string.IsNullOrEmpty(part2))
				Console.WriteLine($"- Part 2 ({sw.Elapsed.TotalMilliseconds:F3}ms): {part2}");

			Console.WriteLine();
		}

		private async Task RunLatest()
		{
			var sw = new Stopwatch();

			if (challengeTypeProvider.Count == 0)
				return;

			var pair= challengeTypeProvider.OrderByDescending(x => x.Key).First();

			Console.WriteLine($"Day {pair.Key}:");

			sw.Start();
			string part1 = await ExecutePart1(pair.Value);
			sw.Stop();

			if (!string.IsNullOrEmpty(part1))
				Console.WriteLine($"- Part 1 ({sw.Elapsed.TotalSeconds:F3}ms): {part1}");

			sw.Restart();
			string part2 = await ExecutePart2(pair.Value);
			sw.Stop();

			if (!string.IsNullOrEmpty(part2))
				Console.WriteLine($"- Part 2 ({sw.Elapsed.TotalSeconds:F3}ms): {part2}");

			Console.WriteLine();
		}

		private async Task RunAll()
		{
			Console.WriteLine("Running all challenges...");
			Console.WriteLine();
			foreach (var pair in challengeTypeProvider.OrderBy(x => x.Key))
			{
				var sw = new Stopwatch();
				Console.WriteLine($"Day {pair.Key}:");

				sw.Start();
				string part1 = await ExecutePart1(pair.Value);
				sw.Stop();

				if (!string.IsNullOrEmpty(part1))
					Console.WriteLine($"- Part 1 ({sw.Elapsed.TotalSeconds:F3}ms): {part1}");

				sw.Restart();
				string part2 = await ExecutePart2(pair.Value);
				sw.Stop();

				if (!string.IsNullOrEmpty(part2))
					Console.WriteLine($"- Part 2 ({sw.Elapsed.TotalSeconds:F3}ms): {part2}");

				Console.WriteLine();
			}
		}

		private async Task<string> ExecutePart1(Type type)
		{
			object challenge = serviceProvider.GetRequiredService(type);
			await ChallengeHelper.SetupAsync(challenge);
			return await ChallengeHelper.Part1Async(challenge);
		}

		private async Task<string> ExecutePart2(Type type)
		{
			object challenge = serviceProvider.GetRequiredService(type);
			await ChallengeHelper.SetupAsync(challenge);
			return await ChallengeHelper.Part2Async(challenge);
		}
	}
}
