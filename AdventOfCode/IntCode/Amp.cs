//-------------------------------------------------------------------------------------------------
//
// Amp.cs -- The Amp class.
//
// Copyright (c) 2020 Marel. All rights reserved.
//
//-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace AdventOfCode.IntCode
{
	//---------------------------------------------------------------------------------------------
	/// <summary>
	/// The Amp class TODO: Describe class here
	/// </summary>
	internal class Amp : IntCodeComputer
	{
		private readonly int[] _program;
		private readonly Queue<int> _inputQueue = new Queue<int>();

		public Amp(char id, int phase, int[] program)
		{
			Id = id;
			Phase = phase;
			_program = program;
			OnInput = HandleInput;
			OnPrint = HandlePrint;
		}

		public char Id { get; }

		public int Phase { get; }

		public void WriteInput(int v)
		{
			// queue.add(v);
			// this.resume();
		}

		public Amp Out { get; }

		public AmpReader In { get; }

		private int HandleInput()
		{
			if (_inputQueue.Count == 0)
				//Suspend();
		}

		private void HandlePrint(int v)
		{
			Out.WriteInput(v);
		}

		public PipeBuilder PipeTo(Amp amp)
		{
			return new PipeBuilder(amp);
		}

		public class AmpWriter
		{
			private Queue<int> _writeQueue;

			private AmpWriter(Queue<int> writeQueue)
			{
				_writeQueue = writeQueue;
			}

			public void Write(int value)
			{
				_writeQueue.Enqueue(value);
			}
		}

		public class AmpReader
		{
			public int Read()
			{
				return 0;
			}
		}

	}

	internal class PipeBuilder
	{
		public Amp _from;

		public PipeBuilder(Amp from)
		{
			_from = from;
		}

		public PipeBuilder PipeTo(Amp amp)
		{
			return new PipeBuilder(amp);
		}
	}
}
