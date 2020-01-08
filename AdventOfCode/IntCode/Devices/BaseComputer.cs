using AdventOfCode.IntCode.Core;
using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The BaseComputer class TODO: Describe class here
	/// </summary>
	internal abstract class BaseComputer
	{
		private readonly Queue<int> _inputQueue;
		private readonly Queue<int> _outputQueue;
		private readonly IntReader _inputReader;
		private readonly IntWriter _outputWriter;

		public BaseComputer()
		{
			Cpu = new Cpu();

			_inputQueue = new Queue<int>();
			_outputQueue = new Queue<int>();

			_inputReader = new IntReader(_inputQueue);
			_outputWriter = new IntWriter(_outputQueue);

			OutputReader = new IntReader(_outputQueue);
			InputWriter = new IntWriter(_inputQueue);

			Cpu.In = _inputReader;
			Cpu.Out = _outputWriter;
		}

		protected Cpu Cpu { get; }

		protected IntReader OutputReader { get; }

		protected IntWriter InputWriter { get; }

		public void LoadProgram(int[] program)
		{
			Cpu.LoadProgram(program);
		}

		public virtual int Execute()
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
