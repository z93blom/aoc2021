using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day07;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 7;

    public string GetName() => "The Treachery of Whales";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var positions = input.Integers().ToArray();
        var min = positions.Min();
        var max = positions.Max();
        var bestPosition = min;
        var bestValue = int.MaxValue;
        for(var i = min; i <= max; i++)
        {
            var value = positions.Sum(p => Math.Abs(p - i));
            if (value < bestValue)
            {
                bestPosition = i;
                bestValue = value;
            }
        }

        return bestValue;
    }

    static object PartTwo(string input)
    {
        var positions = input.Integers().ToArray();
        var min = positions.Min();
        var max = positions.Max();
        var bestPosition = min;
        var bestValue = int.MaxValue;
        var costs = GetCosts(min, max);
        for (var i = min; i <= max; i++)
        {
            var value = positions.Sum(p => costs[Math.Abs(p - i)]);
            if (value < bestValue)
            {
                bestPosition = i;
                bestValue = value;
            }
        }

        return bestValue;
    }

    private static List<int> GetCosts(int min, int max)
    {
        var costs = new List<int>(max - min)
        {
            0
        };
        for (var i = 1; i <= max - min; i++)
        {
            costs.Add(costs[i - 1] + i);
        }

        return costs;
    }
}