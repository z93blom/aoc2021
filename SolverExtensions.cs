using System.Reflection;

namespace AdventOfCode;

public static class SolverExtensions
{
    public static string DayName(this ISolver solver)
    {
        return $"Day {solver.Day()}";
    }

    public static int Year(this ISolver solver)
    {
        return Year(solver.GetType());
    }
#pragma warning disable CS8602 // Dereference of a possibly null reference.

    public static int Year(Type t)
    {
        return int.Parse(t.FullName.Split('.')[1][1..]);
    }

    public static int Day(Type t)
    {
        return int.Parse(t.FullName.Split('.')[2][3..]);
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.


    public static int Day(this ISolver solver)
    {
        return Day(solver.GetType());
    }

    public static string WorkingDir(int year)
    {
        return Path.Combine("src", "app", "Solutions", year.ToString());
    }

    public static string WorkingDir(int year, int day)
    {
        return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
    }

    public static string WorkingDir(this ISolver solver)
    {
        return WorkingDir(solver.Year(), solver.Day());
    }

    public static SplashScreen SplashScreen(this ISolver solver)
    {
        var tsplashScreen = solver.GetType().Assembly.GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(SplashScreen).IsAssignableFrom(t))
             .Single(t => Year(t) == solver.Year());
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        return (SplashScreen)Activator.CreateInstance(tsplashScreen);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
