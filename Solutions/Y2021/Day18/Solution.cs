using System.Diagnostics;
using Newtonsoft.Json;

namespace AdventOfCode.Y2021.Day18;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 18;

    public string GetName() => "Snailfish";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var reader = new NodeReader(input);
        var numbers = new Queue<Node>();
        while (!reader.End)
        {
            numbers.Enqueue(reader.Read());
        }

        var number = numbers.Dequeue();

        while (numbers.Count > 0)
        {
            var right = numbers.Dequeue();
            number = number.Add(right);
        }

        return number.Magnitude;
    }

    static object PartTwo(string input)
    {
        var reader = new NodeReader(input);
        var numbers = new List<Node>();
        while (!reader.End)
        {
            numbers.Add(reader.Read());
        }

        var maxMagnitude = long.MinValue;
        foreach (var left in numbers)
        {
            foreach (var right in numbers)
            {
                var nr = new NodeReader(left + "\r\n" + right);
                var l = nr.Read();
                var r = nr.Read();
                var node = l.Add(r);
                maxMagnitude = Math.Max(maxMagnitude, node.Magnitude);
            }
        }

        return maxMagnitude;

    }

    public class Node
    {
        public int Value { get; private set; } = -1;
        public Node? Left { get; private set; }

        public Node? Right { get; private set; }
        public Node? Parent { get; set; }

        public Node SetValue(int value)
        {
            Value = value;
            Left = null;
            Right = null;
            return this;
        }

        public Node SetNodes(Node left, Node right)
        {
            Value = -1;
            
            Left = left;
            Left.Parent = this;
            
            Right = right;
            Right.Parent = this;
            
            return this;
        }

        public int Level => Parent == null ? 0 : Parent.Level + 1;

        public bool IsValue => Value >= 0;


        public long Magnitude
        {
            get
            {
                if (IsValue)
                {
                    return Value;
                }

                Debug.Assert(Left != null);
                Debug.Assert(Right != null);
                var leftValue = 3 * Left.Magnitude;
                var rightValue = 2 * Right.Magnitude;
                return leftValue + rightValue;
            }
        }

        public bool IsRegular => Left is { IsValue: true } && 
                                 Right is { IsValue: true };

        private Node? SplittableNode
        {
            get
            {
                Node? node = null;

                if (IsValue && Value >= 10)
                {
                    node = this;
                }

                if (node == null && Left != null)
                {
                    node = Left.SplittableNode;
                }
                if (node == null && Right != null)
                {
                    node = Right.SplittableNode;
                }

                return node;
            }
        }

        private Node? ExplodableNode
        {
            get
            {
                Node? node = null;
                if (IsRegular && Level >= 4)
                {
                    node = this;
                }
                
                if (node == null && Left != null)
                {
                    node = Left.ExplodableNode;
                }
                if (node == null && Right != null)
                {
                    node = Right.ExplodableNode;
                }

                return node;
            }
        }

        public bool CanBeReduced => CanExplode || CanSplit;

        public Node Reduce()
        {
            while (CanBeReduced)
            {
                if (CanExplode)
                {
                    Explode();
                }
                else
                {
                    Split();
                }
            }

            return this;
        }

        public Node Add(Node right)
        {
            var top = new Node().SetNodes(this, right);
            top.Reduce();
            return top;
        }

        public bool CanSplit
        {
            get
            {
                var node = SplittableNode;
                return node != null;
            }
        }

        public bool CanExplode
        {
            get
            {
                var node = ExplodableNode;
                return node != null;
            }
        }

        public void Split()
        {
            var node = SplittableNode;
            node?.SplitNode();
        }

        public void Explode()
        {
            var node = ExplodableNode;
            node?.ExplodeNode();
        }

        private void SplitNode()
        {
            Debug.Assert(IsValue);

            var leftValue = Value / 2;
            var leftNode = new Node().SetValue(leftValue);
            
            var rightValue = Value - leftValue;
            var rightNode = new Node().SetValue(rightValue);
            
            this.SetNodes(leftNode, rightNode);
        }

        private void ExplodeNode()
        {
            Debug.Assert(Left != null);
            Debug.Assert(Right != null);
            Debug.Assert(IsRegular);

            var leftNode = NextValueNode(SearchMode.RightMost);
            leftNode?.SetValue(leftNode.Value + Left.Value);
            var rightNode = NextValueNode(SearchMode.LeftMost);
            rightNode?.SetValue(rightNode.Value + Right.Value);
            this.SetValue(0);
        }

        public Node? NextValueNode(SearchMode mode)
        {
            return Parent?.NextValueNode(this, mode);
        }


        private Node? NextValueNode(Node previous, SearchMode searchMode)
        {
            if (this.IsValue)
            {
                return this;
            }

            var nodeSearchOrder = new Node?[3];

            Node? node = null;
            if (searchMode == SearchMode.RightMost)
            {
                nodeSearchOrder[0] = previous == Parent ? Right : null;
                nodeSearchOrder[1] = Left;
                nodeSearchOrder[2] = Parent;

            }
            else
            {
                nodeSearchOrder[0] = previous == Parent ? Left : null;
                nodeSearchOrder[1] = Right;
                nodeSearchOrder[2] = Parent;
            }

            var index = 0;
            while (node == null && index < nodeSearchOrder.Length)
            {
                var searchNode = nodeSearchOrder[index];
                if (previous != searchNode && searchNode != null)
                {
                    node = searchNode.NextValueNode(this, searchMode);
                }

                index++;
            }

            return node;
        }

        public override string ToString()
        {
            if (IsValue)
            {
                return Value.ToString();
            }
            else
            {
                return $"[{Left},{Right}]";
            }
        }
    }

    public enum SearchMode
    {
        LeftMost,
        RightMost
    }

    public ref struct NodeReader
    {
        private readonly ReadOnlySpan<char> _input;
        private int _index;

        public NodeReader(ReadOnlySpan<char> input)
        {
            _input = input;
            _index = 0;
        }

        public bool End => _index >= _input.Length;

        public Node Read()
        {
            ReadWhiteSpace();
            var node = ReadNode(null);
            ReadWhiteSpace();
            return node;
        }

        private Node ReadNode(Node? parent)
        {
            var node = new Node
            {
                Parent = parent
            };

            Read('[');
            var left = ReadNodeA(node);
            Read(',');
            var right = ReadNodeA(node);
            Read(']');
            return node.SetNodes(left, right);
        }

        private Node ReadNodeA(Node? parent)
        {
            var c = Peek();
            if (c == '[')
            {
                return ReadNode(parent);
            }

            if (char.IsDigit(c))
            {
                var value = ReadNumber();
                var node = new Node { Parent = parent }.SetValue(value);
                return node;
            }

            throw new InvalidOperationException($"Unexpected value: '{c}'.");
        }

        private int ReadNumber()
        {
            var start = _index;
            while (_index < _input.Length && char.IsDigit(_input[_index]))
            {
                _index++;
            }

            var value = int.Parse(_input.Slice(start, _index - start));
            return value;
        }

        private void Read(char c)
        {
            Debug.Assert(_input[_index++] == c);
        }

        private void ReadWhiteSpace()
        {
            while (_index < _input.Length && char.IsWhiteSpace(_input[_index]))
            {
                _index++;
            }
        }

        private char Peek()
        {
            return _input[_index];
        }
    }
}