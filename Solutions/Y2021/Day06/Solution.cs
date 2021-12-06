using AdventOfCode.Utilities;
using Spectre.Console;

namespace AdventOfCode.Y2021.Day06;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 6;

    public string GetName() => "Lanternfish";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    object PartOne(string input)
    {
        return Simulate(input, 80);
    }

    private static object Simulate(string input, int days)
    {
        var groups = input.Split(',')
            .Select(long.Parse)
            .GroupBy(v => v)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        for (var i = 0; i < 7; i++)
        {
            if (!groups.ContainsKey(i))
            {
                groups[i] = 0;
            }
        }

        var countdowns = new List<long>();
        for (var day = 1; day <= days; day++)
        {
            var spawningIndex = (day - 1) % 7;
            var newSpawns = groups[spawningIndex];
            if (countdowns.Count >= 2)
            {
                var readyFish = countdowns[0];
                groups[spawningIndex] += readyFish;
                countdowns.RemoveAt(0);
            }

            countdowns.Add(newSpawns);

            //AnsiConsole.MarkupLine($"[yellow]Day {day}: [/][blue]{groups.Select(g => g.Value).Sum()} + {countdowns.Select(v => v).Sum()}[/]");

        }

        return groups.Select(g => g.Value).Sum() + countdowns.Select(v => v).Sum();
    }

    object PartTwo(string input)
    {
        return Simulate(input, 256);

    }
}