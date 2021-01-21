using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCodeLib.ChallengeTypeProviders
{
	public class ReflectionChallengeTypeProvider : Dictionary<int, Type>, IChallengeTypeProvider
	{
		public void ScanAssembly()
		{
			var challengeTypeMap = (from a in AppDomain.CurrentDomain.GetAssemblies()
									from t in a.GetTypes()
									let c = t.GetCustomAttribute<ChallengeAttribute>()
									where c != null
									select (c, t)).ToDictionary(kv => kv.c.Day, kv => kv.t);

			foreach (var kv in challengeTypeMap)
			{
				Add(kv.Key, kv.Value);
			}
		}
	}
}
