using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode;

class Program
{
    static void Main(string[] args)
    {
        var usageProvider = new ApplicationUsage();

        var assemblies = new List<Assembly>
        {
            typeof(Program).Assembly
        };

        var solverTypes = assemblies.SelectMany(a => a.GetTypes())
            .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
            .OrderBy(t => t.FullName)
            .ToArray();

        var action =
            Command(args, Args("update", "([0-9]+)[/-]([0-9]+)"), m =>
            {
                var year = int.Parse(m[1]);
                var day = int.Parse(m[2]);
                return () => Updater.Update(year, day, solverTypes).Wait();
            }) ??
            Command(args, Args("update", "last"), _ =>
            {
                var dt = DateTime.Now;
                if (dt.Month == 12 && dt.Day is >= 1 and <= 25)
                {
                    return () => Updater.Update(dt.Year, dt.Day, solverTypes).Wait();
                }
                else
                {
                    throw new Exception("Event is not active. This option works in Dec 1-25 only)");
                }
            }) ??
             Command(args, Args("([0-9]+)[/-]([0-9]+)"), m =>
             {
                 var year = int.Parse(m[0]);
                 var day = int.Parse(m[1]);
                 var selectedSolverType = solverTypes.First(solverType =>
                                 SolverExtensions.Year(solverType) == year &&
                                 SolverExtensions.Day(solverType) == day);
                 return () => Runner.RunAll(selectedSolverType);
             }) ??
             Command(args, Args("[0-9]+"), m =>
             {
                 var year = int.Parse(m[0]);
                 var selectedSolverTypes = solverTypes.Where(solverType =>
                                 SolverExtensions.Year(solverType) == year);
                 return () => Runner.RunAll(selectedSolverTypes.ToArray());
             }) ??
            Command(args, Args("([0-9]+)[/-]last"), m =>
            {
                var year = int.Parse(m[0]);
                var selectedSolverTypes = solverTypes.Last(solverType =>
                    SolverExtensions.Year(solverType) == year);
                return () => Runner.RunAll(selectedSolverTypes);
            }) ??
            Command(args, Args("([0-9]+)[/-]all"), m =>
            {
                var year = int.Parse(m[0]);
                var selectedSolverTypes = solverTypes.Where(solverType =>
                    SolverExtensions.Year(solverType) == year);
                return () => Runner.RunAll(selectedSolverTypes.ToArray());
            }) ??
            Command(args, Args("all"), _ =>
            {
                return () => Runner.RunAll(solverTypes);
            }) ??
            Command(args, Args("last"), _ =>
            {
                var selectedSolverTypes = solverTypes.Last();
                return () => Runner.RunAll(selectedSolverTypes);
            }) ??
            new Action(() =>
            {
                Console.WriteLine(usageProvider.Usage());
            });

        action();
    }

    private static Action? Command(IReadOnlyCollection<string> args, IReadOnlyCollection<string> regularExpressions, Func<string[], Action> parse)
    {
        if (args.Count != regularExpressions.Count)
        {
            return null;
        }
        var matches = args.Zip(regularExpressions, (arg, regex) => new Regex("^" + regex + "$").Match(arg)).ToArray();
        if (!matches.All(match => match.Success))
        {
            return null;
        }
        try
        {

            return parse(matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new[] { m.Value }).ToArray());
        }
        catch
        {
            return null;
        }
    }

    private static string[] Args(params string[] regex)
    {
        return regex;
    }

}
