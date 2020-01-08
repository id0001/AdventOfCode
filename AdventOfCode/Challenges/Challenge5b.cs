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
	/// The Challenge5b class TODO: Describe class here
	/// </summary>
	internal class Challenge5b : IChallenge
	{
		public string Id => "5b";

		public async Task<string> RunAsync()
		{
			int[] program = (await File.ReadAllTextAsync("Assets/Challenge5.txt")).Split(',').Select(s => int.Parse(s)).ToArray();

			var computer = new SimpleRunner(program);
			computer.QueueInput(5);
			computer.Execute();

			return computer.ReadOutput().ToString();
		}
	}
}
