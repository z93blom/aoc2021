using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day02;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 2;

    public string GetName() => "Dive!";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
     }

    object PartOne(string input)
    {
        var depth = 0;
        var horizontal = 0;
        foreach(var line in input.Lines())
        {
            var parts = line.Split(' ');
            var distance = int.Parse(parts[1]);
            switch (parts[0])
            {
                case "forward": horizontal += distance; break;
                case "up": depth-= distance; break;
                case "down": depth+= distance; break;
                default:
                    throw new Exception("Unknown input");
            }
        }

        return depth*horizontal;
    }

    object PartTwo(string input)
    {
        var aim = 0;
        var depth = 0;
        var horizontal = 0;
        foreach (var line in input.Lines())
        {
            var parts = line.Split(' ');
            var value = int.Parse(parts[1]);
            switch (parts[0])
            {
                case "forward": 
                    horizontal += value;
                    depth += aim * value;
                    break;

                case "up": 
                    aim -= value; 
                    break;

                case "down": 
                    aim += value; 
                    break;

                default:
                    throw new Exception("Unknown input");
            }
        }

        return depth*horizontal;
    }
}