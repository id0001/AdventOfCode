//-------------------------------------------------------------------------------------------------
//
// Moon.cs -- The Moon class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Moon class TODO: Describe class here
	/// </summary>
	internal class Moon
	{
		private Vector3 initialPosition;

		public Moon(string name, Vector3 position) => (Name, Position, initialPosition) = (name, position, position);

		public string Name { get; }

		public Vector3 Position { get; set; }

		public Vector3 Velocity { get; set; }

		public float PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);

		public float KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);

		public float TotalEnergy => PotentialEnergy * KineticEnergy;

		public void ApplyVelocity()
		{
			Position += Velocity;
		}

		public bool IsInitialState => IsInitialStateX && IsInitialStateY && IsInitialStateZ;

		public bool IsInitialStateX => FloatEquals(Position.X, initialPosition.X) && FloatEquals(Velocity.X, 0);

		public bool IsInitialStateY => FloatEquals(Position.Y, initialPosition.Y) && FloatEquals(Velocity.Y, 0);

		public bool IsInitialStateZ => FloatEquals(Position.Z, initialPosition.Z) && FloatEquals(Velocity.Z, 0);

		public override string ToString() => $"{Name}:\t pos=<x={Position.X}, y={Position.Y}, z={Position.Z}> vel=<x={Velocity.X}, y={Velocity.Y}, z={Velocity.Z}>";

		public static void ApplyGravity(Moon a, Moon b)
		{
			Vector3 va = a.Velocity;
			Vector3 vb = b.Velocity;

			if (!FloatEquals(a.Position.X, b.Position.X))
			{
				(va.X, vb.X) = a.Position.X < b.Position.X
					? (va.X + 1, vb.X - 1)
					: (va.X - 1, vb.X + 1);
			}

			if (!FloatEquals(a.Position.Y, b.Position.Y))
			{
				(va.Y, vb.Y) = a.Position.Y < b.Position.Y
					? (va.Y + 1, vb.Y - 1)
					: (va.Y - 1, vb.Y + 1);
			}

			if (!FloatEquals(a.Position.Z, b.Position.Z))
			{
				(va.Z, vb.Z) = a.Position.Z < b.Position.Z
					? (va.Z + 1, vb.Z - 1)
					: (va.Z - 1, vb.Z + 1);
			}

			a.Velocity = va;
			b.Velocity = vb;
		}

		private static bool FloatEquals(float a, float b) => Math.Abs(a - b) < 0.000001f;
	}
}
