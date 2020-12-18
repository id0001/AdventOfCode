using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(18)]
	public class Challenge18
	{
		private readonly IInputReader inputReader;

		public Challenge18(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{

		}

		//[Part1]
		public async Task<string> Part1Async()
		{
			long sum = 0;
			await foreach (var line in inputReader.ReadLinesAsync(18))
			{
				sum += EvaluateExpression(line);
			}

			return sum.ToString();
		}

		[Part2]
		public async Task<string> Part2Async()
		{
			long sum = 0;
			await foreach (var line in inputReader.ReadLinesAsync(18))
			{
				sum += EvaluateExpression(line, new HashSet<char> { '*' });
			}

			return sum.ToString();
		}

		private long EvaluateExpression(string expr, ISet<char> lowestPrecedence = null)
		{
			expr = expr.Trim();

			int bracketCounter = 0;
			int operatorIndex = -1;

			// Search the expr for an operator.
			for (int i = expr.Length - 1; i >= 0; i--)
			{
				char c = expr[i];

				switch (c)
				{
					case '(':
						bracketCounter--;
						break;
					case ')':
						bracketCounter++;
						break;
					case '+' when bracketCounter == 0:
						operatorIndex = i;
						break;
					case '*' when bracketCounter == 0:
						operatorIndex = i;
						break;
				}

				if (operatorIndex >= 0 && (lowestPrecedence == null || lowestPrecedence.Contains(expr[operatorIndex])))
					break;
			}

			// Parse expr as value if it does not contain an operator.
			if (operatorIndex < 0)
			{
				if (expr.StartsWith('(') && expr.EndsWith(')'))
					return EvaluateExpression(expr.Substring(1, expr.Length - 2), lowestPrecedence);

				return long.Parse(expr);
			}

			// Evaluate expressions arount operators and execute on the values.
			return expr[operatorIndex] switch
			{
				'+' => EvaluateExpression(expr.Substring(0, operatorIndex), lowestPrecedence) + EvaluateExpression(expr.Substring(operatorIndex + 1), lowestPrecedence),
				'*' => EvaluateExpression(expr.Substring(0, operatorIndex), lowestPrecedence) * EvaluateExpression(expr.Substring(operatorIndex + 1), lowestPrecedence),
				_ => throw new NotSupportedException()
			};
		}
	}
}
