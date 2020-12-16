using System;
using System.Linq;
using System.Text;

namespace AdventOfCodeLib
{
	public static class ConsoleEx
	{
		public static void PrintMatrix(int[,] matrix)
		{
			int width = 0;
			for (int y = 0; y < matrix.GetLength(0); y++)
			{
				for (int x = 0; x < matrix.GetLength(1); x++)
				{
					string s = matrix[y, x].ToString();
					if (s.Length > width)
						width = s.Length;
				}
			}

			var sb = new StringBuilder();
			for (int y = 0; y < matrix.GetLength(0); y++)
			{
				sb.AppendLine(string.Join(", ", Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[y, x].ToString().PadLeft(width, ' ')).ToArray()));
			}

			Console.WriteLine(sb);
		}
	}
}
