using AdventOfCode.DataStructures;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public class Challenge8a : IChallenge
	{
		public string Id => "8a";

		public async Task<string> RunAsync()
		{
			int[] rawData = (await File.ReadAllTextAsync(@"Assets/Challenge8.txt")).ToCharArray().Select(e => int.Parse(e.ToString())).ToArray();

			var image = new SigImage(25, 6, rawData);

			int layer = Enumerable.Range(0, image.Layers)
						.OrderBy(l => image.Count(l, i => i == 0))
						.First();

			return (image.Count(layer, i => i == 1) * image.Count(layer, i => i == 2)).ToString();
		}
	}
}
