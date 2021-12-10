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
        var grid = new Grid(input);

        var riskLevels = grid.Points()
            .Where(p => grid.IsLow(p.x, p.y))
            .Select(p => p.z + 1)
            .Sum();
        
        return riskLevels;
    }

    public static int ToInt(char c)
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
            _ => -1,
        };
    }

    static object PartTwo(string input)
    {
        var grid = new Grid(input);

        var lows = grid.Points()
            .Where(p => grid.IsLow(p.x, p.y))
            .ToArray();

        var basins = new Dictionary<Point3, HashSet<Point3>>();

        foreach(var p in lows)
        {
            basins.Add(p, new HashSet<Point3>());
            var basin = basins[p];
            var queue = new Queue<Point3>();
            queue.Enqueue(p);
            while(queue.Count > 0)
            {
                var pp = queue.Dequeue();
                basin.Add(pp);
                foreach(var adj in grid.Adjacencies(pp.x, pp.y))
                {
                    if(adj.z < 9 && !basin.Contains(adj) && !queue.Contains(adj))
                    {
                        queue.Enqueue(adj);
                    }
                }
            }
        }

        //var topThree = basins.OrderBy(b => b.Value.Count)
        //            .Take(3)
        //            .ToArray();
        //var value = topThree[0].Value.Count * topThree[1].Value.Count * topThree[2].Value.Count;
        var value = basins.OrderByDescending(b => b.Value.Count)
                    .Take(3)
            .Aggregate(1, (agg, b) => b.Value.Count * agg);

        return value;
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
            int y = 0;
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

        public IEnumerable<Point3> Points()
        {
            for(var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    yield return  new Point3(x, y, _grid[x, y]);
                }
            }
        }

        public bool IsLow(int x, int y)
        {
            var adjLow = Adjacencies(x, y).Select(p => p.z).Min();
            return _grid[x, y] < adjLow;
        }

        public IEnumerable<Point3> Adjacencies(int x, int y)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                yield break;
            }

            if (x - 1 >= 0)
            {
                yield return  new Point3(x - 1, y, _grid[x - 1, y]);
            }

            if (x + 1 < _width)
            {
                yield return new Point3(x +1, y, _grid[x + 1, y]);
            }

            if (y - 1 >= 0)
            {
                yield return new Point3(x, y - 1, _grid[x, y - 1]);
            }

            if (y + 1 < _height)
            {
                yield return new Point3(x, y + 1, _grid[x, y + 1]);
            }
        }

    }

    public struct Point3
    {
        public int x;
        public int y;
        public int z;

        public Point3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}