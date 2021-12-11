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
        var grid = new Grid(input);
        var step = 0;
        var totalPointsFlashed = 0L;
        while (step < 100)
        {
            step++;

            // First, the energy level of each octopus increases by 1.
            var pointsFlashed = new HashSet<Point2>();
            foreach (var point in grid.Points())
            {
                var newValue = point.Z + 1;
                grid.Set(point, newValue);
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
                .SelectMany(p => grid.Adjacencies(p, Adjacencies.OrthagonalAndDiagonals)
                    .Select(p3 => new Point2(p3.X, p3.Y)))
            );
            while (adj.Count > 0)
            {
                var p = adj.Pop();
                if (!pointsFlashed.Contains(p))
                {
                    var newValue = grid.Get(p) + 1;
                    grid.Set(p, newValue);
                    if (newValue == 10)
                    {
                        pointsFlashed.Add(p);
                        foreach (var adjacency in grid.Adjacencies(p, Adjacencies.OrthagonalAndDiagonals))
                        {
                            adj.Push(adjacency);
                        }
                    }
                }
            }

            // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
            foreach (var p in pointsFlashed)
            {
                grid.Set(p, 0);
            }

            totalPointsFlashed += pointsFlashed.Count;
        }

        return totalPointsFlashed;
    }

    static object PartTwo(string input)
    {
        var grid = new Grid(input);
        var step = 0;
        var allZero = false;
        while (!allZero)
        {
            step++;

            // First, the energy level of each octopus increases by 1.
            var pointsFlashed = new HashSet<Point2>();
            foreach (var point in grid.Points())
            {
                var newValue = point.Z + 1;
                grid.Set(point, newValue);
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
                .SelectMany(p => grid.Adjacencies(p, Adjacencies.OrthagonalAndDiagonals)
                    .Select(p3 => new Point2(p3.X, p3.Y)))
            );
            while (adj.Count > 0)
            {
                var p = adj.Pop();
                if (!pointsFlashed.Contains(p))
                {
                    var newValue = grid.Get(p) + 1;
                    grid.Set(p, newValue);
                    if (newValue == 10)
                    {
                        pointsFlashed.Add(p);
                        foreach (var adjacency in grid.Adjacencies(p, Adjacencies.OrthagonalAndDiagonals))
                        {
                            adj.Push(adjacency);
                        }
                    }
                }
            }

            // Finally, any octopus that flashed during this step has its energy level set to 0, as it used all of its energy to flash.
            foreach (var p in pointsFlashed)
            {
                grid.Set(p, 0);
            }

            allZero = pointsFlashed.Count == 100;
        }

        return step;
    }

    public enum Adjacencies
    {
        OrthagonalOnly,
        OrthagonalAndDiagonals
    }

    private class Grid
    {
        private readonly int[,] _grid;
        private readonly int _width;
        private readonly int _height;

        public Grid(string input)
        {
            var lines = input.Lines().ToArray();
            _width = lines[0].Length;
            _height = lines.Length;
            _grid = new int[_width, _height];
            var y = 0;
            foreach (var line in lines)
            {
                var x = 0;
                foreach (var c in line.Select(ToInt))
                {
                    _grid[x++, y] = c;
                }
                y++;
            }
        }

        public void Set(Point2 point, int value)
        {
            _grid[point.X, point.Y] = value;
        }

        public int Get(Point2 point)
        {
            return _grid[point.X, point.Y];
        }

        private static int ToInt(char c)
        {
            return c switch
            {
                '0' => 0,
                '1' => 1,
                '2' => 2,
                '3' => 3,
                '4' => 4,
                '5' => 5,
                '6' => 6,
                '7' => 7,
                '8' => 8,
                '9' => 9,
                _ => throw new InvalidOperationException("invalid character"),
            };
        }

        public IEnumerable<Point3> Points()
        {
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    yield return new Point3(x, y, _grid[x, y]);
                }
            }
        }


        public IEnumerable<Point3> Adjacencies(Point2 point, Adjacencies adjacency)
        {
            if (point.X < -1 || point.X > _width || point.Y < -1 || point.Y > _height)
            {
                yield break;
            }

            if (point.X - 1 >= 0)
            {
                yield return new Point3(point.X - 1, point.Y, _grid[point.X - 1, point.Y]);
            }

            if (point.X + 1 < _width)
            {
                yield return new Point3(point.X + 1, point.Y, _grid[point.X + 1, point.Y]);
            }

            if (point.Y - 1 >= 0)
            {
                yield return new Point3(point.X, point.Y - 1, _grid[point.X, point.Y - 1]);
            }

            if (point.Y + 1 < _height)
            {
                yield return new Point3(point.X, point.Y + 1, _grid[point.X, point.Y + 1]);
            }

            if (adjacency == Solution.Adjacencies.OrthagonalAndDiagonals)
            {
                if (point.X - 1 >= 0 && point.Y - 1 >= 0)
                {
                    yield return new Point3(point.X - 1, point.Y - 1, _grid[point.X - 1, point.Y - 1]);
                }

                if (point.X + 1 < _width && point.Y - 1 >= 0)
                {
                    yield return new Point3(point.X + 1, point.Y - 1, _grid[point.X + 1, point.Y - 1]);
                }

                if (point.X - 1 >= 0 && point.Y + 1 < _height)
                {
                    yield return new Point3(point.X - 1, point.Y + 1, _grid[point.X - 1, point.Y + 1]);
                }

                if (point.X + 1 < _width && point.Y + 1 < _height)
                {
                    yield return new Point3(point.X + 1, point.Y + 1, _grid[point.X + 1, point.Y + 1]);
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    sb.Append($"{_grid[x, y]:00} " );
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string ToStringOne()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    sb.Append($"{_grid[x, y]:0}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public struct Point2
    {
        public int X { get; }
        public int Y { get; }
        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public struct Point3
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Point2(Point3 p) => new(p.X, p.Y);

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}