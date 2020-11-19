using AdventOfCode.IntCode.Core;
using System.Collections.Generic;


namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The BaseComputer class TODO: Describe class here
	/// </summary>
	internal abstract class BaseComputer
	{
		private readonly Queue<long> _inputQueue;
		private readonly Queue<long> _outputQueue;
		private readonly IntReader _inputReader;
		private readonly IntWriter _outputWriter;

		public BaseComputer()
		{
			Cpu = new Cpu();

			_inputQueue = new Queue<long>();
			_outputQueue = new Queue<long>();

			_inputReader = new IntReader(_inputQueue);
			_outputWriter = new IntWriter(_outputQueue);

			Out = new IntReader(_outputQueue);
			In = new IntWriter(_inputQueue);

			Cpu.In = _inputReader;
			Cpu.Out = _outputWriter;
		}

		protected Cpu Cpu { get; }

		public IntReader Out { get; }

		public IntWriter In { get; }

		public void LoadProgram(long[] program)
		{
			Cpu.LoadProgram(program);
		}
	}
}
