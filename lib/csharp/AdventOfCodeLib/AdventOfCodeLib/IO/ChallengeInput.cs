using System.Threading.Tasks;

namespace AdventOfCodeLib.IO
{
	public class ChallengeInput
	{
		private readonly string basePath;
		private readonly int challengeNumber;

		public ChallengeInput(string basePath, int challengeNumber)
		{
			this.basePath = basePath;
			this.challengeNumber = challengeNumber;
		}

		public async Task<string> ReadAllText()
		{
			return string.Empty;
		}
	}
}
