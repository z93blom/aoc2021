using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using AdventOfCode.Utilities;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ShortestPath;

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

        var graph = new AdjacencyGraph<Node, Edge>();
        var nodes = new Dictionary<Point2, Node>();
        var y = 0;
        foreach (var line in lines)
        {
            for (var x = 0; x < line.Length; x++)
            {
                var point = new Point2(x, y);
                var node = new Node(lines, point);
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
                    graph.AddEdge(new Edge(node, nodes[point]));
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

    public class Edge : IEdge<Node>
    {
        public Node Source { get; }
        public Node Target { get; }

        public Edge(Node source, Node target)
        {
            Source = source;
            Target = target;
        }

        public override string ToString()
        {
            return $"{Source} -> {Target}";
        }
    }

    public class Node
    {
        public Point2 Point { get; }
        private readonly string[] _lines;

        public Node(string[] lines, Point2 point)
        {
            Point = point;
            _lines = lines;
        }

        public int Value => _lines[Point.Y][(int)Point.X] - '0';

        public override string ToString()
        {
            return $"{Point} = {Value}";
        }
    }

    static object PartTwo(string input)
    {
        return 0;
    }
}