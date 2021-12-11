using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day09;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 9;

    public string GetName() => "Smoke Basin";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var grid = ParseGrid(input);

        var riskLevels = grid.Points
            .Where(p => IsLow(grid, p))
            .Select(p => grid[p] + 1)
            .Sum();
        
        return riskLevels;
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

    private static bool IsLow(Grid<int> grid, Point2 point)
    {
        var adjacentLow = point.OrthogonalPoints
            .Where(grid.Contains)
            .Select(p => grid[p])
            .Min();

        return grid[point] < adjacentLow;
    }

    static object PartTwo(string input)
    {
        var grid = ParseGrid(input);

        var lows = grid.Points
            .Where(p => IsLow(grid, p))
            .ToArray();

        var basins = new Dictionary<Point2, HashSet<Point2>>();

        foreach(var basinStart in lows)
        {
            basins.Add(basinStart, new HashSet<Point2>());
            var basin = basins[basinStart];
            var queue = new Queue<Point2>();
            queue.Enqueue(basinStart);
            while(queue.Count > 0)
            {
                var point = queue.Dequeue();
                basin.Add(point);
                foreach(var adj in point.OrthogonalPoints.Where(grid.Contains))
                {
                    if(grid[adj] < 9 && !basin.Contains(adj) && !queue.Contains(adj))
                    {
                        queue.Enqueue(adj);
                    }
                }
            }
        }

        var value = basins.OrderByDescending(b => b.Value.Count)
                    .Take(3)
            .Aggregate(1, (agg, b) => b.Value.Count * agg);

        return value;
    }
}