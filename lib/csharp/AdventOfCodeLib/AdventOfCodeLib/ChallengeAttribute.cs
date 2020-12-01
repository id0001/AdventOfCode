using System;

namespace AdventOfCodeLib
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ChallengeAttribute : Attribute
	{
		public ChallengeAttribute(int day)
		{
			Day = day;
		}

		public int Day { get; set; }
	}
}
