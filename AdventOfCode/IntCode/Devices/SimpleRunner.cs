using AdventOfCode.IntCode.Core;
using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The SimpleRunner class TODO: Describe class here
	/// </summary>
	internal class SimpleRunner : BaseComputer
	{
		public SimpleRunner(int[] program) : base()
		{
			LoadProgram(program);
		}

		public void QueueInput(int input)
		{
			InputWriter.Write(input);
		}

		public int ReadOutput()
		{
			return OutputReader.Read();
		}

		public bool TryReadOutput(out int output)
		{
			return OutputReader.TryRead(out output);
		}

		public IEnumerable<int> ReadToEnd()
		{
			while(OutputReader.TryRead(out int output))
			{
				yield return output;
			}
		}

		public override int Execute()
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
