using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day17;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 17;

    public string GetName() => "Trick Shot";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var boundValues = input.Integers().ToArray();

        var lowBounds = new Point2(Math.Min(boundValues[0], boundValues[1]), Math.Min(boundValues[2], boundValues[3]));
        var highBounds = new Point2(Math.Max(boundValues[0], boundValues[1]), Math.Max(boundValues[2], boundValues[3]));

        // Just simulate a range of possible x and y velocities
        var maxHeight = 0;
        for (var yVelocity = 0; yVelocity < 1000; yVelocity++)
        {
            for (var xVelocity = 0; xVelocity < 1000; xVelocity++)
            {
                var (hit, yMax) = Simulate(xVelocity, yVelocity, lowBounds, highBounds);
                if (hit)
                {
                    maxHeight = Math.Max(maxHeight, yMax);
                }
            }
        }

        return maxHeight;
    }

    public static (bool, int) Simulate(int startXVelocity, int startYVelocity, Point2 low, Point2 high)
    {
        var x = 0;
        var y = 0;
        var xSpeed = startXVelocity;
        var ySpeed = startYVelocity;
        var maxHeight = y;
        while (x <= high.X && y >= low.Y)
        {
            x += xSpeed;
            y += ySpeed;
            xSpeed += xSpeed < 0 ? 1 : xSpeed > 0 ? -1 : 0;
            ySpeed -= 1;
            maxHeight = Math.Max(maxHeight, y);

            if (x >= low.X && x <= high.X && y >= low.Y && y <= high.Y)
            {
                return (true, maxHeight);
            }
        }

        return (false, maxHeight);
    }

    static object PartTwo(string input)
    {
        var boundValues = input.Integers().ToArray();

        var lowBounds = new Point2(Math.Min(boundValues[0], boundValues[1]), Math.Min(boundValues[2], boundValues[3]));
        var highBounds = new Point2(Math.Max(boundValues[0], boundValues[1]), Math.Max(boundValues[2], boundValues[3]));

        // Just simulate a range of possible x and y velocities
        var hits = 0;
        for (var yVelocity = -1000; yVelocity < 1000; yVelocity++)
        {
            for (var xVelocity = 0; xVelocity < 1000; xVelocity++)
            {
                var (hit, _) = Simulate(xVelocity, yVelocity, lowBounds, highBounds);
                if (hit)
                {
                    hits++;
                }
            }
        }

        return hits;
    }
}