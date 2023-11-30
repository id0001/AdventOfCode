using System.Text.RegularExpressions;
using AdventOfCode.Core;
using AdventOfCode.Core.IO;

namespace AdventOfCode2020.Challenges;

[Challenge(21)]
public class Challenge21
{
    private readonly IInputReader _inputReader;
    private readonly List<Food> _input = new();

    public Challenge21(IInputReader inputReader)
    {
        _inputReader = inputReader;
    }

    [Setup]
    public async Task SetupAsync()
    {
        await foreach (var line in _inputReader.ReadLinesAsync(21))
        {
            var match = Regex.Match(line, @"^(.+) \(contains (.+)\)$");
            var ingredients = match.Groups[1].Value.Split(" ");
            var allergens = match.Groups[2].Value.Split(", ");
            _input.Add(new Food(ingredients.ToList(), allergens.ToList()));
        }
    }

    [Part1]
    public string Part1()
    {
        var potentialAllergens = new Dictionary<string, ISet<string>>();
        var ingredientsWithCount = new Dictionary<string, int>();

        foreach (var food in _input)
        {
            foreach (var ingredient in food.Ingredients)
            {
                ingredientsWithCount.TryAdd(ingredient, 0);

                ingredientsWithCount[ingredient]++;
            }

            foreach (var allergen in food.Allergens)
                if (potentialAllergens.ContainsKey(allergen))
                    potentialAllergens[allergen].IntersectWith(food.Ingredients);
                else
                    potentialAllergens[allergen] = new HashSet<string>(food.Ingredients);
        }

        var dangerousIngredients = potentialAllergens.Values.SelectMany(e => e).ToHashSet();

        var num = ingredientsWithCount.Where(kv => !dangerousIngredients.Contains(kv.Key)).Sum(kv => kv.Value);

        return num.ToString();
    }

    [Part2]
    public string Part2()
    {
        var potentialAllergens = new Dictionary<string, ISet<string>>();

        foreach (var food in _input)
        foreach (var allergen in food.Allergens)
            if (potentialAllergens.ContainsKey(allergen))
                potentialAllergens[allergen].IntersectWith(food.Ingredients);
            else
                potentialAllergens[allergen] = new HashSet<string>(food.Ingredients);

        var result = new Dictionary<string, string>();

        while (result.Count < 8)
        {
            var potentials = potentialAllergens.Where(e => e.Value.Count == 1).ToList();
            foreach (var p in potentials)
            {
                result.Add(p.Key, p.Value.Single());
                potentialAllergens.Remove(p.Key);
            }

            var ingredientsToRemove = potentials.SelectMany(e => e.Value).ToList();
            foreach (var allergen in potentialAllergens) allergen.Value.ExceptWith(ingredientsToRemove);
        }


        return string.Join(",", result.OrderBy(kv => kv.Key).Select(kv => kv.Value));
    }

    private record Food(IList<string> Ingredients, IList<string> Allergens);
}