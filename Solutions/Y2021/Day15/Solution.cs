using AdventOfCode.Utilities;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AdventOfCode.Y2021.Day15;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 15;

    public string GetName() => "Chiton";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var lines = input.Lines()
            .ToArray();

        var graph = new AdjacencyGraph<Point2Node<int>, Point2Edge<int>>();
        var nodes = new Dictionary<Point2, Point2Node<int>>();
        var y = 0;
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                var point = new Point2(x, y);
                var node = new Point2Node<int>(point, lines[y][x] - '0');
                nodes.Add(point, node);
                graph.AddVertex(node);
            }

            y++;
        }

        foreach (var node in nodes.Values)
        {
            foreach (var point in node.Point.OrthogonalPoints)
            {
                if (nodes.ContainsKey(point))
                {
                    graph.AddEdge(new Point2Edge<int>(node, nodes[point]));
                }
            }
        }

        var start = nodes[new Point2(0, 0)];
        var end = nodes[new Point2(lines[0].Length - 1, lines.Length - 1)];
        var shortestPath = graph.ShortestPathsDijkstra(edge => edge.Target.Value, start);

        if (shortestPath(end, out var result))
        {
            var totalRisk = result.Sum(edge => edge.Target.Value);
            return totalRisk;
        }
        else
        {
            throw new Exception("Unable to find a path!");
        }
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines()
            .ToArray();

        var graph = new AdjacencyGraph<Point2Node<int>, Point2Edge<int>>();
        var nodes = new Dictionary<Point2, Point2Node<int>>();
        var width = lines[0].Length;
        var height = lines.Length;
        var y = 0;
        foreach (var line in lines)
        {
            for (var x = 0; x < width; x++)
            {
                var value = lines[y][x] - '1';

                for (var yOffset = 0; yOffset < 5; yOffset++)
                {
                    for (var xOffset = 0; xOffset < 5; xOffset++)
                    {
                        var point = new Point2(x + xOffset * width, y + yOffset * width);
                        var val = (value + xOffset + yOffset) % 9 + 1;
                        var node = new Point2Node<int>(point, val);
                        nodes.Add(point, node);
                        graph.AddVertex(node);
                    }
                }

            }

            y++;
        }

        foreach (var node in nodes.Values)
        {
            foreach (var point in node.Point.OrthogonalPoints)
            {
                if (nodes.ContainsKey(point))
                {
                    graph.AddEdge(new Point2Edge<int>(node, nodes[point]));
                }
            }
        }

        var start = nodes[new Point2(0, 0)];
        var end = nodes[new Point2(lines[0].Length * 5 - 1, lines.Length * 5 - 1)];
        var shortestPath = graph.ShortestPathsDijkstra(edge => edge.Target.Value, start);

        if (shortestPath(end, out var result))
        {
            var totalRisk = result.Sum(edge => edge.Target.Value);
            return totalRisk;
        }
        else
        {
            throw new Exception("Unable to find a path!");
        }
    }
}