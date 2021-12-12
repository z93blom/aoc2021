using System.Linq;
using System.Net.Http.Headers;
using AdventOfCode.Utilities;
using QuikGraph;
using QuikGraph.Algorithms.Observers;

namespace AdventOfCode.Y2021.Day12;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 12;

    public string GetName() => "Passage Pathing";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var graph = CreateGraph(input);
        var start = graph.Vertices.First(v => v.Name == "start");
        var end = graph.Vertices.First(v => v.Name == "end");

        var paths = new HashSet<Path>();
        var pathsToTry = new Queue<Path>();
        var firstPath = new Path(start);
        pathsToTry.Enqueue(firstPath);
        while (pathsToTry.Count > 0)
        {
            var path = pathsToTry.Dequeue();
            if (path.Start == start && path.End == end)
            {
                paths.Add(path);
                continue;
            }

            graph.TryGetOutEdges(path.End, out var outEdges);
            foreach (var outEdge in outEdges)
            {
                if (path.Vertices.Contains(outEdge.Target) && outEdge.Target.IsSmall)
                {
                    // Can only visit a small cave once.
                    continue;
                }

                var newPath = new Path(path, outEdge.Target);
                pathsToTry.Enqueue(newPath);
            }
        }

        return paths.Count;
    }

    static object PartTwo(string input)
    {
        var graph = CreateGraph(input);
        var start = graph.Vertices.First(v => v.Name == "start");
        var end = graph.Vertices.First(v => v.Name == "end");
        var paths = new HashSet<Path>();
        var pathsToTry = new Queue<Path>();
        var firstPath = new Path(start);
        pathsToTry.Enqueue(firstPath);
        while (pathsToTry.Count > 0)
        {
            var path = pathsToTry.Dequeue();
            if (path.Start == start && path.End == end)
            {
                paths.Add(path);
                continue;
            }

            graph.TryGetOutEdges(path.End, out var outEdges);
            foreach (var outEdge in outEdges)
            {
                if (path.Vertices.Contains(outEdge.Target) && outEdge.Target.IsSmall)
                {
                    if (outEdge.Target == start || path.HasVisitedSmallCaveTwice)
                    {
                        continue;
                    }
                }

                var newPath = new Path(path, outEdge.Target);
                pathsToTry.Enqueue(newPath);
            }
        }

        return paths.Count;
    }

    private static AdjacencyGraph<Vertex, Edge> CreateGraph(string input)
    {
        var graph = new AdjacencyGraph<Vertex, Edge>();
        var groups = input.Lines()
            .Matches(@"(\w+)-(\w+)")
            .ToArray();

        var nameToCave = new Dictionary<string, Vertex>();
        foreach (var group in groups)
        {
            var name = group[0].Value;
            if (!nameToCave.ContainsKey(name))
            {
                var vertex = new Vertex(name);
                graph.AddVertex(vertex);
                nameToCave[name] = vertex;
            }

            var name2 = group[1].Value;
            if (!nameToCave.ContainsKey(name2))
            {
                var vertex = new Vertex(name2);
                graph.AddVertex(vertex);
                nameToCave[name2] = vertex;
            }

            var edge = new Edge(nameToCave[name], nameToCave[name2]);
            graph.AddEdge(edge);
            edge = new Edge(nameToCave[name2], nameToCave[name]);
            graph.AddEdge(edge);
        }

        return graph;
    }

    private class Path
    {
        private readonly Vertex[] _vertices;

        public Path(Vertex vertex)
        {
            _vertices = new Vertex[] { vertex };
        }

        public Path(Path path, Vertex vertex)
        {
            _vertices = path._vertices.Append(vertex).ToArray();
        }

        public Vertex[] Vertices => _vertices;

        public Vertex Start => _vertices[0];
        
        public Vertex End => _vertices[^1];

        public bool HasVisitedSmallCaveTwice =>
            _vertices
                .Where(v => v.IsSmall)
                .GroupBy(v => v)
                .Any(g => g.Count() > 1);

        public override string ToString()
        {
            return string.Join(",", _vertices.Select(v => v.ToString()));
        }
    }

    private class Vertex
    {
        private readonly string _name;

        public Vertex(string name)
        {
            _name = name;
        }

        public bool IsSmall => _name.All(char.IsLower);

        public string Name => _name;

        public override string ToString()
        {
            return _name;
        }
    }

    private class Edge : IEdge<Vertex>
    {
        public Edge(Vertex source, Vertex target)
        {
            Source = source;
            Target = target;
        }

        public Vertex Source { get; }
        public Vertex Target { get; }

        public override string ToString()
        {
            return $"{Source} - {Target}";
        }
    }
}