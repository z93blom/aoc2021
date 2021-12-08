using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day03;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 3;

    public string GetName() => "Binary Diagnostic";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
     }

    static object PartOne(string input)
    {
        var lines = input.Lines().ToArray();
        var mostCommon = "";
        var leastCommon = "";
        for(var i = 0; i < lines[0].Length; i++)
        {
            var zeroes = lines.Select(l => l[i]).Count(c => c == '0');
            if (zeroes > lines.Length / 2 )
            {
                mostCommon+= "0";
                leastCommon += "1";
            }
            else
            {
                mostCommon += "1";
                leastCommon += "0";
            }
        }

        var gamma = Convert.ToInt64(mostCommon, 2);
        var epsilon = Convert.ToInt64(leastCommon, 2);

        return gamma*epsilon;
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines().ToArray();
        var index = 0;
        var oxygen = "";
        var co2 = "";
        var co2Lines = lines;
        var o2Lines = lines;
        while (o2Lines.Length > 1 || co2Lines.Length > 1)
        {
            if (o2Lines.Length > 1)
            {
                var zeroes = o2Lines.Select(l => l[index]).Count(c => c == '0');
                var ones = o2Lines.Select(l => l[index]).Count(c => c == '1');
                oxygen += zeroes > ones ? "0" : "1";
                o2Lines = o2Lines.Where(l => l.StartsWith(oxygen)).ToArray();
            }

            if (co2Lines.Length > 1)
            {
                var zeroes = co2Lines.Select(l => l[index]).Count(c => c == '0');
                var ones = co2Lines.Select(l => l[index]).Count(c => c == '1');
                co2 += zeroes <= ones ? "0" : "1";
                co2Lines = co2Lines.Where(l => l.StartsWith(co2)).ToArray();
            }

            index++;
        }

        var o2rating = Convert.ToInt64(o2Lines[0], 2);
        var co2rating = Convert.ToInt64(co2Lines[0], 2);

        return o2rating * co2rating;
    }
}