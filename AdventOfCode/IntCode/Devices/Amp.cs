using AdventOfCode.IntCode.Core;

namespace AdventOfCode.IntCode.Devices
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Amp class TODO: Describe class here
	/// </summary>
	internal class Amp : BaseComputer
	{
		private readonly int _phase;
		private Amp _pipeTo;

		public Amp(string id, long[] program, int phase) : base()
		{
			Id = id;
			Cpu.LoadProgram(program);
			_phase = phase;
		}

		public string Id { get; }

		public bool IsHalted => Cpu.IsHalted;

		public void PipeTo(Amp other)
		{
			_pipeTo = other;
		}

		public void Initialize()
		{
			Cpu.Run();
			In.Write(_phase);
		}

		public int Execute()
		{
			while (!Cpu.IsHalted)
			{
				Cpu.Next();
				if (!_pipeTo.IsHalted)
				{
					foreach (var output in Out.ReadToEnd())
					{

						_pipeTo?.In.Write(output);
					}
				}

				if (Cpu.State == ExecutionState.ActionRequired)
				{
					return -1;
				}
			}

			return 0;
		}
	}
}
