//-------------------------------------------------------------------------------------------------
//
// Challenge12a.cs -- The Challenge12a class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Challenge12a class TODO: Describe class here
	/// </summary>
	internal class Challenge12b : IChallenge
	{
		public string Id => "12b";

		public async Task<string> RunAsync()
		{
			var lines = await File.ReadAllLinesAsync("Assets/Challenge12.txt");

			var v0 = ConvertLineToVector(lines[0]);
			var v1 = ConvertLineToVector(lines[1]);
			var v2 = ConvertLineToVector(lines[2]);
			var v3 = ConvertLineToVector(lines[3]);

			var moons = new[]
			{
				new Moon("io", v0),
				new Moon("europe", v1),
				new Moon("ganymede", v2),
				new Moon("callisto", v3)
			};

			var prevPositions = new HashSet<(Vector3, Vector3, Vector3, Vector3)>();

			int steps = 0;
			do
			{
				prevPositions.Add((moons[0].Position, moons[1].Position, moons[2].Position, moons[3].Position));

				for (int y = 0; y < 3; y++)
				{
					for (int x = y + 1; x < 4; x++)
					{
						Moon.ApplyGravity(moons[y], moons[x]);
					}
				}

				foreach (var moon in moons)
				{
					moon.ApplyVelocity();
				}

				steps++;
			} while (!prevPositions.Contains((moons[0].Position, moons[1].Position, moons[2].Position, moons[3].Position)));

			Console.WriteLine($"Total steps: {steps}");

			return string.Empty;
		}

		private Vector3 ConvertLineToVector(string line)
		{
			var pattern = new Regex(@"^<x=(-?\d+), y=(-?\d+), z=(-?\d+)>$");

			var match = pattern.Match(line);
			if (!match.Success)
				throw new InvalidOperationException("Unable to read line.");

			float x = float.Parse(match.Groups[1].Value);
			float y = float.Parse(match.Groups[2].Value);
			float z = float.Parse(match.Groups[3].Value);

			return new Vector3(x, y, z);
		}
	}
}
