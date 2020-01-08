using AdventOfCode.IntCode;
using AdventOfCode.IntCode.Devices;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge5a class TODO: Describe class here
	/// </summary>
	internal class Challenge5a : IChallenge
	{
		public string Id => "5a";

		public async Task<string> RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge5.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var computer = new SimpleRunner(program);
			computer.QueueInput(1);
			computer.Execute();

			return computer.ReadToEnd().Last().ToString();
		}
	}
}
