﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdventOfCodeLib
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class SetupAttribute : Attribute
	{
	}
}
