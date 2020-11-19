
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.IntCode.Devices
{
	internal class ArcadeCabinet : BaseComputer
	{
		public ArcadeCabinet(long[] program)
		{
			Cpu.LoadProgram(program);
		}

		public int[,] ScreenBuffer { get; set; }

		public void Run()
		{
			Cpu.Start();
			ScreenBuffer = new int[64, 64];

			var output = new Queue<int>();

			while (!Cpu.IsHalted)
			{
				Cpu.Next();

				// Read output into local queue.
				foreach (var o in Out.ReadToEnd())
				{
					output.Enqueue((int)o);
				}

				// Handle draw instructions.
				while (output.Count >= 3)
				{
					int x = output.Dequeue();
					int y = output.Dequeue();
					int id = output.Dequeue();

					if (x == -1 && y == 0)
					{
						ShowScore(id);
					}
					else
					{
						DrawTile(x, y, id);
					}
				}

				if (Cpu.State == Core.ExecutionState.ActionRequired)
				{
					int bx = GetBallX();
					int px = GetPaddleX();

					if (bx == px)
					{
						In.Write(0);
					}
					else if (bx < px)
					{
						In.Write(-1);
					}
					else
					{
						In.Write(1);
					}
				}
			}
		}

		private void ShowScore(int score)
		{
			ScreenBuffer[0, 0] = score;

			//Console.CursorLeft = 0;
			//Console.CursorTop = 0;
			//Console.Write(string.Format("{0,-20}", score));
		}

		private int GetBallX()
		{
			for (int y = 0; y < ScreenBuffer.GetLength(0); y++)
			{
				for (int x = 0; x < ScreenBuffer.GetLength(1); x++)
				{
					if (ScreenBuffer[y, x] == 4)
						return x;
				}
			}

			return -1;
		}

		private int GetPaddleX()
		{
			for (int y = 0; y < ScreenBuffer.GetLength(0); y++)
			{
				for (int x = 0; x < ScreenBuffer.GetLength(1); x++)
				{
					if (ScreenBuffer[y, x] == 3)
						return x;
				}
			}

			return -1;
		}

		private void DrawTile(int x, int y, int id)
		{
			ScreenBuffer[y + 2, x] = id;

			//Console.CursorLeft = x;
			//Console.CursorTop = y + 2;

			//switch (id)
			//{
			//	case 0:
			//		Console.Write(" ");
			//		break;
			//	case 1:
			//		Console.Write("#");
			//		break;
			//	case 2:
			//		Console.Write("B");
			//		break;
			//	case 3:
			//		Console.Write("_");
			//		break;
			//	case 4:
			//		Console.Write("O");
			//		break;
			//}
		}
	}
}
