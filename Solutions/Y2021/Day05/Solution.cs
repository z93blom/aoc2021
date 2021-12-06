using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day05;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 5;

    public string GetName() => "Hydrothermal Venture";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    object PartOne(string input)
    {
        var lines = input.Lines()
            .Matches(@"(\d+),(\d+)\s*->\s*(\d+),(\d+)")
            .Select(lineGroup => lineGroup.Select(g => int.Parse(g.Value)).ToArray())
            .Select(i =>  new Line(new Point(i[0], i[1]), new Point(i[2], i[3])))
            .ToArray();

        // Get the number of points where at least two lines overlap.

        var covered = new Dictionary<Point, int>();
        foreach (var line in lines
            .Where(line => line.IsHorizontal || line.IsVertical))
        {
            foreach(var p in line.CoveredPoints)
            {
                if (covered.ContainsKey(p))
                {
                    covered[p]++;
                }
                else
                {
                    covered.Add(p, 1);
                }
            }
        }

        return covered.Where(kvp => kvp.Value > 1).Count();

    }

    object PartTwo(string input)
    {
        var lines = input.Lines()
            .Matches(@"(\d+),(\d+)\s*->\s*(\d+),(\d+)")
            .Select(lineGroup => lineGroup.Select(g => int.Parse(g.Value)).ToArray())
            .Select(i => new Line(new Point(i[0], i[1]), new Point(i[2], i[3])))
            .ToArray();

        // Get the number of points where at least two lines overlap.

        var covered = new Dictionary<Point, int>();
        foreach (var line in lines)
        {
            foreach (var p in line.CoveredPoints)
            {
                if (covered.ContainsKey(p))
                {
                    covered[p]++;
                }
                else
                {
                    covered.Add(p, 1);
                }
            }
        }

        return covered.Where(kvp => kvp.Value > 1).Count();
    }

    private class Line
    {
        public Point P1 { get; }
        public Point P2 { get; }

        public bool IsHorizontal => P1.Y == P2.Y;
        public bool IsVertical => P1.X == P2.X;

        public Line(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                if (p1.Y < p2.Y)
                {
                    P1 = p1;
                    P2 = p2;
                }
                else
                {
                    P1 = p2;
                    P2 = p1;
                }
            }
            else
            {
                if (p1.X < p2.X)
                {
                    P1 = p1;
                    P2 = p2;
                }
                else
                {
                    P1 = p2;
                    P2 = p1;
                }
            }
        }

        public IEnumerable<Point> CoveredPoints
        {
            get
            {
                if (IsHorizontal)
                {
                    var x = P1.X;
                    while(x <= P2.X)
                    {
                        yield return new Point(x++, P1.Y);
                    }
                }
                else if (IsVertical)
                {
                    var y = P1.Y;
                    while (y <= P2.Y)
                    {
                        yield return new Point(P1.X, y++);
                    }
                }
                else
                {
                    var x = P1.X;
                    var y = P1.Y;
                    var increasingY = P1.Y < P2.Y;
                    while (x <= P2.X)
                    {
                        yield return new Point(x, y);
                        x++;
                        y += increasingY ? 1 : -1;
                    }
                }
            }
        }
    }


}