//-------------------------------------------------------------------------------------------------
//
// OpCode.cs -- The OpCode enumeration.
//
// Copyright (c) 2019 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

namespace AdventOfCode.IntCode
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The OpCode enumeration TODO: Describe enumeration here
	/// </summary>
	internal enum OpCode : int
	{
		Nop = 0,
		Add = 1,
		Multiply = 2,
		Input = 3,
		Print = 4,
		JumpIfTrue = 5,
		JumpIfFalse = 6,
		LessThan = 7,
		Equals = 8,
		Halt = 99
	}
}
