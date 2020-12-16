using AdventOfCodeLib;
using AdventOfCodeLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(16)]
	public class Challenge16
	{
		private readonly IInputReader inputReader;

		private IList<ISet<int>> ticketRules;
		private int[] yourTicket;
		private List<int[]> tickets;

		public Challenge16(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			int state = 0;

			ticketRules = new List<ISet<int>>();
			tickets = new List<int[]>();

			await foreach (var line in inputReader.ReadLinesAsync(16))
			{
				if (string.IsNullOrEmpty(line))
				{
					state++;
					continue;
				}

				if (line is "your ticket:" or "nearby tickets:")
					continue;

				if (state == 0) // rules
				{
					var match = Regex.Match(line, @".+: (\d+)-(\d+) or (\d+)-(\d+)");
					int s1 = int.Parse(match.Groups[1].Value);
					int e1 = int.Parse(match.Groups[2].Value);
					int s2 = int.Parse(match.Groups[3].Value);
					int e2 = int.Parse(match.Groups[4].Value);
					var set = new HashSet<int>();
					for (int i = s1; i <= e1; i++)
						set.Add(i);

					for (int i = s2; i <= e2; i++)
						set.Add(i);

					ticketRules.Add(set);
				}
				else if (state == 1) // your ticket
				{
					yourTicket = line.Split(",").Select(e => int.Parse(e)).ToArray();
					tickets.Add(yourTicket);
				}
				else
				{
					int[] ticket = line.Split(",").Select(e => int.Parse(e)).ToArray();
					tickets.Add(ticket);
				}
			}
		}

		[Part1]
		public string Part1()
		{
			int invalid = 0;
			foreach (var ticket in tickets)
			{
				for (int i = 0; i < ticket.Length; i++)
				{
					if (!ticketRules.Any(rule => rule.Contains(ticket[i])))
					{
						invalid += ticket[i];
						break;
					}
				}
			}

			return invalid.ToString();
		}

		[Part2]
		public string Part2()
		{
			var validTickets = GetValidTickets();
			int[] matrix = new int[20];

			for (int y = 0; y < 20; y++) // y = rule index
			{
				for (int x = 0; x < 20; x++) // x = pos index
				{
					if (ticketRules[y].IsSupersetOf(validTickets.Select(ticket => ticket[x])))
						matrix[y] += 1 << x;
				}
			}

			int[] order = new int[20];
			int found = 0;
			int mask = (1 << 20) - 1;
			while (found < 20)
			{
				for (int y = 0; y < matrix.Length; y++)
				{
					if (IsPowerOf2(matrix[y] & mask))
					{
						int o = InversePow(matrix[y] & mask);
						order[y] = o;
						found++;
						mask -= 1 << o;
					}
				}
			}

			return MathEx.Product(Enumerable.Range(0, 6).Select(i => (long)yourTicket[order[i]]).ToArray()).ToString();
		}

		private int InversePow(int n)
		{
			return (int)(Math.Log(n) / Math.Log(2));
		}

		private bool IsPowerOf2(int n)
		{
			if (n == 0)
				return false;

			return (int)Math.Ceiling(Math.Log(n) / Math.Log(2)) == (int)Math.Floor(Math.Log(n) / Math.Log(2));
		}

		private List<int[]> GetValidTickets()
		{
			List<int[]> validTickets = new List<int[]>();
			foreach (var ticket in tickets)
			{
				bool valid = true;
				for (int i = 0; i < ticket.Length; i++)
				{
					if (!ticketRules.Any(rule => rule.Contains(ticket[i])))
					{
						valid = false;
						break;
					}
				}

				if (valid)
					validTickets.Add(ticket);
			}

			return validTickets;
		}

	}
}
