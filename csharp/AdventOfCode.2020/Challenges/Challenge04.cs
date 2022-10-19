using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(4)]
public class Challenge04
{
    private static readonly string[] EyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

    private readonly IInputReader _inputReader;
    private readonly List<IDictionary<string, string>> _input = new();

    public Challenge04(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        var lines = await _inputReader.ReadLinesAsync(4).ToArrayAsync();

        var currentData = new List<KeyValuePair<string, string>>();
        foreach (var line in lines)
        {
            if (line.Length == 0)
            {
                _input.Add(new Dictionary<string, string>(currentData));
                currentData = new List<KeyValuePair<string, string>>();
                continue;
            }

            currentData.AddRange(line.Split(' ')
                .Select(l => l.Split(':'))
                .Select(kv => new KeyValuePair<string, string>(kv[0], kv[1])));
        }

        if (currentData.Count > 0)
            _input.Add(new Dictionary<string, string>(currentData));
    }

    [Part1]
    public string Part1()
    {
        var validCount = _input.Count(ContainsAllRequiredFields);
        return validCount.ToString();
    }

    [Part2]
    public string Part2()
    {
        var validCount = _input.Count(ValidatePassport);
        return validCount.ToString();
    }

    private static bool ContainsAllRequiredFields(IDictionary<string, string> passport) => passport.ContainsKey("byr")
        && passport.ContainsKey("iyr")
        && passport.ContainsKey("eyr")
        && passport.ContainsKey("hgt")
        && passport.ContainsKey("hcl")
        && passport.ContainsKey("ecl")
        && passport.ContainsKey("pid");

    private static bool ValidatePassport(IDictionary<string, string> passport)
    {
        if (!ContainsAllRequiredFields(passport))
            return false;

        var byr = int.Parse(passport["byr"]);
        if (byr is < 1920 or > 2002)
            return false;

        var iyr = int.Parse(passport["iyr"]);
        if (iyr is < 2010 or > 2020)
            return false;

        var eyr = int.Parse(passport["eyr"]);
        if (eyr is < 2020 or > 2030)
            return false;

        var match = Regex.Match(passport["hgt"], @"(\d+)(cm|in)");
        if (!match.Success)
            return false;

        var unit = match.Groups[2].Value;
        var hgt = int.Parse(match.Groups[1].Value);

        switch (unit)
        {
            case "cm" when hgt is < 150 or > 193:
            case "in" when hgt is < 59 or > 76:
                return false;
        }

        if (!Regex.IsMatch(passport["hcl"], @"\#[0-9a-f]{6}"))
            return false;

        if (!EyeColors.Contains(passport["ecl"]))
            return false;

        return passport["pid"].Length == 9 && int.TryParse(passport["pid"], out _);
    }
}