using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	public class Challenge8b : IChallenge
	{
		public string Id => "8b";

		public async Task<string> RunAsync()
		{
			int[] rawData = (await File.ReadAllTextAsync(@"Assets/Challenge8.txt")).ToCharArray().Select(e => int.Parse(e.ToString())).ToArray();

			var image = new SigImage(25, 6, rawData).Flatten();

			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					sb.Append(image[0, y, x] == 1 ? '#' : ' ');
				}

				sb.AppendLine();
			}

			// Result = ubufp
			return sb.ToString();
		}
	}
}
