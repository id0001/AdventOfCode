//-------------------------------------------------------------------------------------------------
//
// PaintingRobot.cs -- The PaintingRobot class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using AdventOfCode.DataStructures;
using AdventOfCode.IntCode.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The PaintingRobot class TODO: Describe class here
	/// </summary>
	internal class PaintingRobot : BaseComputer
	{
		private readonly long _firstPanelColor;

		public PaintingRobot(long[] program, long firstPanelColor = 0)
		{
			Cpu.LoadProgram(program);

			_firstPanelColor = firstPanelColor;
		}

		public ReadOnlyDictionary<Point, long> LocationsPainted { get; private set; }

		public void Run()
		{
			Cpu.Start();

			var locationHistory = new Dictionary<Point, long>();
			var currentLocation = new Point(0, 0);
			locationHistory.Add(currentLocation, _firstPanelColor);
			int direction = 0;

			int action = 0;

			while (!Cpu.IsHalted)
			{
				Cpu.Next();

				foreach (var output in Out.ReadToEnd())
				{
					switch (action)
					{
						case 0: // Paint location
							locationHistory[currentLocation] = output;
							break;
						case 1: // Change direction
							direction = output == 0 ? SMod(direction - 1, 4) : SMod(direction + 1, 4);
							currentLocation = NextLocation(currentLocation, direction);
							break;
						default:
							throw new InvalidOperationException("Action not allowed");
					}

					// Change action
					action = SMod(action + 1, 2);
				}

				if (Cpu.State == ExecutionState.ActionRequired)
				{
					locationHistory.TryGetValue(currentLocation, out long color);
					In.Write(color);
				}
			}

			LocationsPainted = new ReadOnlyDictionary<Point, long>(locationHistory);
		}

		private Point NextLocation(Point currentLocation, int direction)
		{
			return direction switch
			{
				0 => new Point(currentLocation.X, currentLocation.Y - 1),
				1 => new Point(currentLocation.X + 1, currentLocation.Y),
				2 => new Point(currentLocation.X, currentLocation.Y + 1),
				3 => new Point(currentLocation.X - 1, currentLocation.Y),
				_ => throw new InvalidOperationException("Wrong direction")
			};
		}

		private int SMod(int a, int n) => (a % n + n) % n;
	}
}
