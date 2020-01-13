namespace AdventOfCode.IntCode.Core
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
		Output = 4,
		JumpIfTrue = 5,
		JumpIfFalse = 6,
		LessThan = 7,
		Equals = 8,
		AjustRelativeBase = 9,
		Halt = 99
	}
}
