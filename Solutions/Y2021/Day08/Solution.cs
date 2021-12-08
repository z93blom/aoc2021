using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day08;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 8;

    public string GetName() => "Seven Segment Search";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    object PartOne(string input)
    {
        var inOut = input.Lines()
            .Select(l => l.Split('|').ToArray())
            .Select(a => new { In = a[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray(), Out = a[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray() })
            .ToArray();

        var outs = inOut.Select(a => a.Out).ToArray();

        var uniqueLengths = new List<int>() { 2, 3, 4, 7 };
        var lengths = outs.Select(v => v.Select(t => t.Length).ToArray()).ToArray();
        return lengths.Select(v => v.Count(t => uniqueLengths.Contains(t))).Sum();
    }

    object PartTwo(string input)
    {
        var inOut = input.Lines()
            .Select(l => l.Split('|').ToArray())
            .Select(a => new 
            { 
                Wires = a[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).OrderBy(arr => arr.Length) .ToArray(),
                Numbers = a[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray()
            })
            .ToArray();

        var values = inOut.Select(a => CalculateValue(a.Wires, a.Numbers))
            .ToArray();

        return values.Sum();
    }

    public int CalculateValue(string[] signals, string[] numbers)
    {
        var map = MapWireToSegment(signals);
        var v = numbers.Aggregate(0, (agg, t) => agg * 10 + MapToNumber(t, map));
        return v;
    }

    public int MapToNumber(string s, Dictionary<char, char> map)
    {
        var v = new string(s.Select(c => map[c]).OrderBy(c => c).ToArray());
        switch(v)
        {
            case "abcefg": return 0;
            case "cf": return 1;
            case "acdeg": return 2;
            case "acdfg": return 3;
            case "bcdf": return 4;
            case "abdfg": return 5;
            case "abdefg": return 6;
            case "acf": return 7;
            case "abcdefg": return 8;
            case "abcdfg": return 9;
            default: throw new Exception($"Invalid number : {v}");
        }
    }

    public Dictionary<char, char> MapWireToSegment(string[] signals)
    {
        var map = new Dictionary<char, char>();

        // Map segment 'a' by taking the only wire that is lit by 7, but not by 1
        var unknownSignals = signals.ToList();
        var one = signals[0]; unknownSignals.Remove(one);
        var four = signals[2]; unknownSignals.Remove(four);
        var seven = signals[1]; unknownSignals.Remove(seven);
        var eight = signals[9]; unknownSignals.Remove(eight);

        // If we remove all the segments making up '7' (acf) we get two new unique lengths for numbers three (2) and six (4)
        var three = unknownSignals.Where(x => x.RemoveAllChars(seven).Length == 2).Single(); unknownSignals.Remove(three);
        var six = unknownSignals.Where(x => x.RemoveAllChars(seven).Length == 4).Single(); unknownSignals.Remove(six);

        var a = seven.Where(c => !one.Contains(c)).Single();

        var rightSegments = one;

        // c is the only segment lit in three that isn't lit it six.
        var c = three.Where(c => !six.Contains(c)).Single();

        // f is the other lit segment in one.
        var f = one.RemoveChar(c)[0];

        // b and e are in six, but not in three
        var leftSegments = six.RemoveAll(three.ToArray());

        // zero is the only one where we have a single lit segment if we remove the left segments and seven
        var zero = unknownSignals.Where(x => x.RemoveAllChars(leftSegments).RemoveAllChars(seven).Length == 1).Single();
        unknownSignals.Remove(zero);
        var g = zero.RemoveAllChars(seven).RemoveAllChars(leftSegments)[0];

        // nine is the only one with length 6 left.
        var nine = unknownSignals.Where(x => x.Length == 6).Single();

        var e = eight.RemoveAllChars(nine)[0];
        var b = leftSegments.RemoveChar(e)[0];
        var d = four.RemoveChar(b).RemoveChar(c).RemoveChar(f)[0];

        map[a] = 'a'; 
        map[b] = 'b';
        map[c] = 'c';
        map[d] = 'd';
        map[e] = 'e';
        map[f] = 'f';
        map[g] = 'g';
        return map;
    }
}