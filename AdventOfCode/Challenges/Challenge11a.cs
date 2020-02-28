
using AdventOfCode.IntCode.Devices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public class Challenge11a : IChallenge
	{
		public string Id => "11a";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge11.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var robot = new PaintingRobot(program);
			robot.Run();

			return robot.LocationsPainted.Count.ToString();
		}
	}
}
