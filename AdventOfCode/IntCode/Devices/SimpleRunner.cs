using AdventOfCode.IntCode.Core;


namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The SimpleRunner class TODO: Describe class here
	/// </summary>
	internal class SimpleRunner : BaseComputer
	{
		public SimpleRunner(long[] program) : base()
		{
			LoadProgram(program);
		}

		public long Execute()
		{
			Cpu.Run();
			while (!Cpu.IsHalted)
			{
				Cpu.Next();
			}

			return Cpu.ExitCode;
		}
	}
}
