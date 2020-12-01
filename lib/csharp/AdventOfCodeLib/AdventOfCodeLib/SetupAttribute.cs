using System;

namespace AdventOfCodeLib
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class SetupAttribute : Attribute
	{
	}
}
