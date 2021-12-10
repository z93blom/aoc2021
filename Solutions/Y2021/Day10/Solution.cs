using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day10;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 10;

    public string GetName() => "Syntax Scoring";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var lines = input.Lines().ToArray();

        var pairs = new Dictionary<char, char>
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };

        var scores = new Dictionary<char, int>
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 },
        };

        var score = 0;

        foreach (var line in lines)
        {
            var expected = new Stack<char>();
            expected.Push(pairs[line[0]]);
            var valid = true;
            for(var i = 1; i < line.Length && valid; i++)
            {
                var c = line[i];
                if (pairs.ContainsKey(c))
                {
                    expected.Push(pairs[c]);
                }
                else if (c == expected.Peek())
                {
                    expected.Pop();
                }
                else
                {
                    score += scores[c];
                    valid = false;
                }
            }
        }

        return score;
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines().ToArray();

        var pairs = new Dictionary<char, char>
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };

        var scores = new Dictionary<char, int>
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 },
        };

        var lineScores = new List<long>();

        foreach (var line in lines)
        {
            var expected = new Stack<char>();
            expected.Push(pairs[line[0]]);
            var valid = true;
            for (var i = 1; i < line.Length && valid; i++)
            {
                var c = line[i];
                if (pairs.ContainsKey(c))
                {
                    expected.Push(pairs[c]);
                }
                else if (c == expected.Peek())
                {
                    expected.Pop();
                }
                else
                {
                    valid = false;
                }
            }

            if (valid)
            {
                var linescore = 0L;
                while(expected.Count> 0)
                {
                    var c = expected.Pop();
                    linescore =  linescore * 5 + scores[c];
                }

                lineScores.Add(linescore);
            }
        }

        lineScores.Sort();

        return lineScores[lineScores.Count / 2];
    }
}