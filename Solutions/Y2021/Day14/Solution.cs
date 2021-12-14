using System.Text;
using AdventOfCode.Utilities;
using Newtonsoft.Json.Converters;
using Spectre.Console;

namespace AdventOfCode.Y2021.Day14;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 14;

    public string GetName() => "Extended Polymerization";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var result = Iterate(input, 10);
        return result;
    }

    private static long Iterate(string input, int steps)
    {
        var lines = input.Lines().ToArray();
        var polymer = new List<char>(lines[0]);
        var templates = lines.Skip(1)
            .Matches(@"(\w\w) -> (\w)")
            .Select(g => new Template(g[0].ValueSpan[0], g[0].ValueSpan[1], g[1].ValueSpan[0]))
            .ToDictionary(t => t.PairTuple, t => t);

        var initialDict = templates.Values.Select(t => new KeyValuePair<Template, long>(t, 0))
            .ToArray();

        var pairs = new Dictionary<Template, long>(initialDict);
        for (var i = 0; i < polymer.Count - 1; i++)
        {
            var pair = templates[(polymer[i], polymer[i + 1])];
            pairs[pair]++;
        }

        var step = 0;
        while (step < steps)
        {
            var newPairs = new Dictionary<Template, long>(initialDict);
            foreach (var (template, value) in pairs.Where(kvp => kvp.Value > 0))
            {
                var insertion = template.Insertion;
                var left = templates[(template.First, insertion)];
                newPairs[left] += value;
                var right = templates[(insertion, template.Second)];
                newPairs[right] += value;
            }

            pairs = newPairs;
            step++;
        }

        var counts =
            new Dictionary<char, long>();
        foreach (var template in pairs.Keys)
        {
            counts[template.First] = 0;
            counts[template.Second] = 0;
        }

        // Only take the first character of the pairs.
        foreach (var (template, value) in pairs)
        {
            counts[template.First] += value;
        }

        // Add the last character in the polymer.
        counts[polymer[^1]] += 1;

        var orderedCounts = counts.OrderBy(kvp => kvp.Value).ToArray();
        var mostCommon = orderedCounts[^1].Value;
        var leastCommon = orderedCounts[0].Value;
        var result = mostCommon - leastCommon;
        return result;
    }

    public class Template
    {
        public char First { get; }
        public char Second { get; }

        public char Insertion { get; }

        public string Pair => $"{First}{Second}";

        public (char, char) PairTuple => (First, Second);

        public Template(char first, char second, char insertion)
        {
            First = first;
            Second = second;
            Insertion = insertion;
        }

        public override string ToString()
        {
            return $"{First}{Second} -> {Insertion}";
        }
    }

    static object PartTwo(string input)
    {
        var result = Iterate(input, 40);
        return result;
        //return 0;
    }
}