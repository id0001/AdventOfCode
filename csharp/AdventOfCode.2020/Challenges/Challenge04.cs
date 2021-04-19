using AdventOfCode.Lib;
using AdventOfCode.Lib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
	[Challenge(4)]
	public class Challenge04
	{
		private static readonly string[] EyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

		private readonly IInputReader inputReader;
		private List<IDictionary<string, string>> input;

		public Challenge04(IInputReader inputReader)
		{
			this.inputReader = inputReader;
		}

		[Setup]
		public async Task SetupAsync()
		{
			var lines = await inputReader.ReadLinesAsync(4).ToArrayAsync();

			input = new List<IDictionary<string, string>>();
			List<KeyValuePair<string, string>> currentData = new List<KeyValuePair<string, string>>();
			foreach (var line in lines)
			{
				if (line.Length == 0)
				{
					input.Add(new Dictionary<string, string>(currentData));
					currentData = new List<KeyValuePair<string, string>>();
					continue;
				}

				currentData.AddRange(line.Split(' ')
					.Select(l => l.Split(':'))
					.Select(kv => new KeyValuePair<string, string>(kv[0], kv[1])));
			}

			if (currentData.Count > 0)
				input.Add(new Dictionary<string, string>(currentData));
		}

		[Part1]
		public string Part1()
		{
			int validCount = input.Count(p => ContainsAllRequiredFields(p));
			return validCount.ToString();
		}

		[Part2]
		public string Part2()
		{
			int validCount = input.Count(p => ValidatePassport(p));
			return validCount.ToString();
		}

		private void Print(IDictionary<string, string> item)
		{
			Console.WriteLine($"byr: {item["byr"]}, iyr: {item["iyr"]}, eyr: {item["eyr"]}, hgt: {item["hgt"]}, hcl: {item["hcl"]}, ecl: {item["ecl"]}, pid: {item["pid"]}");
		}

		private bool ContainsAllRequiredFields(IDictionary<string, string> passport) => passport.ContainsKey("byr")
																					&& passport.ContainsKey("iyr")
																					&& passport.ContainsKey("eyr")
																					&& passport.ContainsKey("hgt")
																					&& passport.ContainsKey("hcl")
																					&& passport.ContainsKey("ecl")
																					&& passport.ContainsKey("pid");

		private bool ValidatePassport(IDictionary<string, string> passport)
		{
			if (!ContainsAllRequiredFields(passport))
				return false;

			int byr = int.Parse(passport["byr"]);
			if (byr < 1920 || byr > 2002)
				return false;

			int iyr = int.Parse(passport["iyr"]);
			if (iyr < 2010 || iyr > 2020)
				return false;

			int eyr = int.Parse(passport["eyr"]);
			if (eyr < 2020 || eyr > 2030)
				return false;

			Match match = Regex.Match(passport["hgt"], @"(\d+)(cm|in)");
			if (!match.Success)
				return false;

			string unit = match.Groups[2].Value;
			int hgt = int.Parse(match.Groups[1].Value);

			if (unit == "cm" && (hgt < 150 || hgt > 193))
				return false;

			if (unit == "in" && (hgt < 59 || hgt > 76))
				return false;

			if (!Regex.IsMatch(passport["hcl"], @"\#[0-9a-f]{6}"))
				return false;

			if (!EyeColors.Contains(passport["ecl"]))
				return false;

			if (passport["pid"].Length != 9 || !int.TryParse(passport["pid"], out int _))
				return false;

			return true;
		}
	}
}
