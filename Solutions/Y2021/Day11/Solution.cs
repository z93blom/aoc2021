using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day11;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 11;

    public string GetName() => "Dumbo Octopus";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var g = ParseGrid(input);

        var step = 0;
        var totalPointsFlashed = 0L;
        while (step < 100)
        {
            step++;
            var pointsFlashed = SimulateSingleStep(g);
            totalPointsFlashed += pointsFlashed.Count;
        }

        return totalPointsFlashed;
    }

    private static Grid<int> ParseGrid(string input)
    {
        var lines = input.Lines().ToArray();

        var g = new Grid<int>(lines[0].Length, lines.Length);
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                g[x++, y] = c - '0';
            }

            y++;
        }

        return g;
    }

    private static HashSet<Point2> SimulateSingleStep(Grid<int> g)
    {
        var pointsFlashed = new HashSet<Point2>();

        // First, the energy level of each octopus increases by 1.
        foreach (var point in g.Points)
        {
            var newValue = g[point] + 1;
            g[point] = newValue;
            if (newValue == 10)
            {
                pointsFlashed.Add(point);
            }
        }

        // Then, any octopus with an energy level greater than 9 flashes.
        // This increases the energy level of all adjacent octopuses by 1,
        // including octopuses that are diagonally adjacent. If this causes
        // an octopus to have an energy level greater than 9, it also flashes.
        // This process continues as long as new octopuses keep having their energy
        // level increased beyond 9. (An octopus can only flash at most once per step.)
        var adj = new Stack<Point2>(pointsFlashed
            .SelectMany(p => p.AdjacentPoints.Where(g.Contains)));
        while (adj.Count > 0)
        {
            var p = adj.Pop();
            if (!pointsFlashed.Contains(p))
            {
                var newValue = g[p] + 1;
                g[p] = newValue;
                if (newValue == 10)
                {
                    pointsFlashed.Add(p);
                    foreach (var adjacency in p.AdjacentPoints.Where(g.Contains))
                    {
                        adj.Push(adjacency);
                    }
                }
            }
        }

        // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
        foreach (var p in pointsFlashed)
        {
            g[p] = 0;
        }

        return pointsFlashed;
    }

    static object PartTwo(string input)
    {
        var g = ParseGrid(input);

        var step = 0;
        var pointsFlashed = new HashSet<Point2>();
        while (pointsFlashed.Count != g.Width * g.Height)
        {
            step++;
            pointsFlashed = SimulateSingleStep(g);
        }

        return step;
    }
}