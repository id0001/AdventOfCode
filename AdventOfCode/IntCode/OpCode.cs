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
		Halt = 99
	}
}
