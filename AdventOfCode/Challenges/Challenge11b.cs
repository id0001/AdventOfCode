
using AdventOfCode.DataStructures;
using AdventOfCode.IntCode.Devices;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public class Challenge11b : IChallenge
	{
		public string Id => "11b";

		public async Task<string> RunAsync()
		{
			long[] program = (await File.ReadAllTextAsync("Assets/Challenge11.txt")).Split(',').Select(s => long.Parse(s)).ToArray();

			var robot = new PaintingRobot(program, 1);
			robot.Run();

			return DrawHull(robot.LocationsPainted);
		}

		private static string DrawHull(ReadOnlyDictionary<Point, long> locations)
		{
			var keys = locations.Keys;

			var leftMost = keys.Min(e => e.X);
			var rightMost = keys.Max(e => e.X);
			var topMost = keys.Min(e => e.Y);
			var bottomMost = keys.Max(e => e.Y);

			var rows = bottomMost - topMost + 1;
			var cols = rightMost - leftMost + 1;

			var sb = new StringBuilder();
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					if (locations.TryGetValue(new Point(x + leftMost, y + topMost), out long pvalue))
					{
						sb.Append(pvalue == 1 ? '#' : '.');
					}
					else
					{
						sb.Append('.');
					}
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
