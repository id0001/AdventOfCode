namespace AdventOfCode.IntCode.Core
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The ExecutionState enumeration TODO: Describe enumeration here
	/// </summary>
	internal enum ExecutionState : int
	{
		Halted = 0,
		Running = 1,
		ActionRequired = 2,
	}
}
