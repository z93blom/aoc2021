using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day01;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 1;

    public string GetName() => "Sonar Sweep";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
     }

    object PartOne(string input)
    {
        var measurements = input.Integers().ToArray();
        var increasedValues = Enumerable.Range(1, measurements.Length - 1)
            .Count(i => measurements[i] > measurements[i - 1]);
        return increasedValues;
    }

    object PartTwo(string input)
    {
        var measurements = input.Integers().ToArray();
        var windows = Enumerable.Range(0, measurements.Length - 2)
            .Select(i => measurements[i] + measurements[i + 1] + measurements[i + 2])
            .ToArray();
        var increasedValues = Enumerable.Range(1, windows.Length - 1)
            .Count(i => windows[i] > windows[i - 1]);
        return increasedValues;
    }
}