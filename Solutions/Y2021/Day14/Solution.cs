using System.Text;
using AdventOfCode.Utilities;

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
        var polymer = GetPolymer(input, 10);

        var groups = polymer.GroupBy(c => c)
            .OrderBy(g => g.LongCount())
            .ToArray();

        var result = groups[^1].LongCount() - groups[0].LongCount();
        return result;
    }

    private static List<char> GetPolymer(string input, int steps)
    {
        var lines = input.Lines().ToArray();
        var polymer = new List<char>(lines[0]);
        var templates = lines.Skip(1)
            .Matches(@"(\w\w) -> (\w)")
            .Select(g => new Template(g[0].ValueSpan[0], g[0].ValueSpan[1], g[1].ValueSpan[0]))
            .ToDictionary(t => t.Pair, t => t);

        var step = 0;
        while (step++ < steps)
        {
            var builder = new List<char>();
            for (var i = 0; i < polymer.Count - 1; i++)
            {
                builder.Add(polymer[i]);
                var pair = $"{polymer[i]}{polymer[i+1]}";
                var insertion = templates[pair].Insertion;
                builder.Add(insertion);
            }

            builder.Add(polymer[^1]);
            polymer = builder;
        }

        return polymer;
    }

    public class Template
    {
        public char First { get; }
        public char Second { get; }

        public char Insertion { get; }

        public string Pair => $"{First}{Second}";

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
        //var polymer = GetPolymer(input, 40);

        //var groups = polymer.GroupBy(c => c)
        //    .OrderBy(g => g.LongCount())
        //    .ToArray();

        //var result = groups[^1].LongCount() - groups[0].LongCount();
        //return result;

        return 0;
    }
}