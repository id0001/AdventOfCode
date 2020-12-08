using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(8)]
	public class Challenge8
	{
		private readonly IInputReader inputReader;
		private IList<Instruction> input;

		public Challenge8(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			input = await inputReader.ReadLinesAsync(8)
				.Select(line => new Instruction(line.Substring(0, 3), int.Parse(line.Substring(4)))).ToListAsync();
		}

		[Part1]
		public string Part1()
		{
			int ip = 0;
			int acc = 0;

			ISet<int> history = new HashSet<int>();

			for (ip = 0; ip < input.Count; ip++)
			{
				if (history.Contains(ip))
					return acc.ToString();

				history.Add(ip);

				var instruction = input[ip];

				switch (instruction.Opcode)
				{
					case "acc":
						acc += instruction.Value;
						break;
					case "jmp":
						ip += instruction.Value - 1; // -1 because the for loop always increases the ip by 1
						break;
					case "nop":
						break;
				}
			}

			return null;
		}

		[Part2]
		public string Part2()
		{
			ISet<int> closedList = new HashSet<int>();

			bool hasError;
			int acc;
			do
			{
				// Reset the computer
				hasError = false;
				ISet<int> history = new HashSet<int>();
				int ip = 0;
				acc = 0;
				bool changedInstruction = false;

				for (ip = 0; ip < input.Count; ip++)
				{
					if (history.Contains(ip))
					{
						hasError = true;
						break;
					}

					history.Add(ip);

					var instruction = input[ip];

					if (!changedInstruction)
					{
						if (instruction.Opcode == "nop" && instruction.Value != 0 && !closedList.Contains(ip) && ip - instruction.Value > 0 && ip + instruction.Value <= input.Count)
						{
							// change to jmp if instruction falls within boundaries and not on trylist or causes infinite loop
							instruction = new Instruction("jmp", instruction.Value);
							closedList.Add(ip);
							changedInstruction = true;

						}
						else if (instruction.Opcode == "jmp" && !closedList.Contains(ip))
						{
							// change to nop if not in trylist
							instruction = new Instruction("nop", instruction.Value);
							closedList.Add(ip);
							changedInstruction = true;
						}
					}

					switch (instruction.Opcode)
					{
						case "acc":
							acc += instruction.Value;
							break;
						case "jmp":
							ip += instruction.Value - 1; // -1 because the for loop always increases the ip by 1
							break;
						case "nop":
							break;
					}
				}
			}
			while (hasError);

			return acc.ToString();
		}

		private record Instruction(string Opcode, int Value);
	}
}
