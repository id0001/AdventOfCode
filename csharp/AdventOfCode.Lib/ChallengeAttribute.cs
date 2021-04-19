using System;

namespace AdventOfCode.Lib
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ChallengeAttribute : Attribute
	{
		public ChallengeAttribute(int day)
		{
			Day = day;
		}

		public int Day { get; set; }
	}
}
