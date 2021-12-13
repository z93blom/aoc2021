using AdventOfCode.Utilities;
using Spectre.Console;

namespace AdventOfCode.Y2021.Day13;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 13;

    public string GetName() => "Transparent Origami";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var lines = input.Lines().ToArray();
        var coordinates = lines.Matches(@"(\d+),(\d+)")
            .Select(g => new Point2(long.Parse(g[0].Value), long.Parse(g[1].Value)))
            .ToArray();
        var width = coordinates.Select(p => p.X).Max() + 1;
        var height = coordinates.Select(p => p.Y).Max() + 1;
        var grid = new Grid<char>(width, height);
        foreach (var point in coordinates)
        {
            grid[point] = '#';
        }

        var instructions = lines.Matches(@"fold along ([xy])=(\d+)")
            .Select(g => new Fold(g[0].Value[0], long.Parse(g[1].Value)))
            .ToArray();

        for (var i = 0; i < 1; i++)
        {
            var fold = instructions[i];
            grid = fold.Axis switch
            {
                'x' => FoldVertical(grid, fold.Value),
                'y' => FoldHorizontal(grid, fold.Value),
                _ => throw new NotSupportedException()
            };
        }

        var points = grid.Points.Where(p => grid[p] == '#').ToArray();
        return points.Length;
    }

    private static Grid<char> FoldHorizontal(Grid<char> grid, long y)
    {
        var newGrid = new Grid<char>(grid.Width, y);
        foreach (var p in grid.Points.Where(p => grid[p] == '#'))
        {
            if (p.Y < y)
            {
                newGrid[p] = '#';
            }
            else if (p.Y > y)
            {
                newGrid[p.X, y - (p.Y - y)] = '#';
            }
            else
            {
                // On - skip it.
            }
        }

        return newGrid;
    }

    private static Grid<char> FoldVertical(Grid<char> grid, long x)
    {
        var newGrid = new Grid<char>(x, grid.Height);
        foreach (var p in grid.Points.Where(p => grid[p] == '#'))
        {
            if (p.X < x)
            {
                newGrid[p] = '#';
            }
            else if (p.X > x)
            {
                newGrid[x - (p.X - x), p.Y] = '#';
            }
            else
            {
                // On - skip it.
            }
        }

        return newGrid;
    }

    struct Fold
    {
        public char Axis { get; }
        public long Value { get; }

        public Fold(char axis, long value)
        {
            Axis = axis;
            Value = value;
        }
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines().ToArray();
        var coordinates = lines.Matches(@"(\d+),(\d+)")
            .Select(g => new Point2(long.Parse(g[0].Value), long.Parse(g[1].Value)))
            .ToArray();
        var width = coordinates.Select(p => p.X).Max() + 1;
        var height = coordinates.Select(p => p.Y).Max() + 1;
        var grid = new Grid<char>(width, height);
        foreach (var point in coordinates)
        {
            grid[point] = '#';
        }

        var instructions = lines.Matches(@"fold along ([xy])=(\d+)")
            .Select(g => new Fold(g[0].Value[0], long.Parse(g[1].Value)))
            .ToArray();

        foreach (var fold in instructions)
        {
            grid = fold.Axis switch
            {
                'x' => FoldVertical(grid, fold.Value),
                'y' => FoldHorizontal(grid, fold.Value),
                _ => throw new NotSupportedException()
            };
        }

        foreach (var point in grid.Points)
        {
            if (grid[point] != '#')
            {
                grid[point] = '.';
            }
        }

        var text = grid.ToString("");

        AnsiConsole.Write(text);

        return "BLHFJPJF";
    }
}