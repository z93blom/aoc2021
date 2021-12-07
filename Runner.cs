using Spectre.Console;
using System.Diagnostics;

namespace AdventOfCode;

public class Runner
{

    public static void RunAll(params Type[] tsolvers)
    {
        var lastYear = -1;
        foreach (var solver in tsolvers.Select(tsolver => Activator.CreateInstance(tsolver) as ISolver))
        {
            if (solver == null)
            {
                continue;
            }

            if (lastYear != solver.Year())
            {
                solver.SplashScreen().Show();
                lastYear = solver.Year();
            }

            var workingDir = solver.WorkingDir();
            var currentDirectory = Environment.CurrentDirectory;
            AnsiConsole.MarkupLine($"[white]{solver.DayName()}: {solver.GetName()}[/]");
            var directories = new[]
            {
                Path.Combine(currentDirectory, workingDir, "test"),
                Path.Combine(currentDirectory, @"..\..\..", workingDir, "test"),
                Path.Combine(currentDirectory, workingDir),
                Path.Combine(currentDirectory, @"..\..\..", workingDir),
            };
            var allFiles = directories.SelectMany(dir =>
            {
                if (!Directory.Exists(dir))
                {
                    return Enumerable.Empty<string>();
                }

                return Directory.EnumerateFiles(dir).Where(file => file.EndsWith(".in"));
            }).ToArray();

            var commonPrefix = GetLongestCommonPrefix(allFiles);

            var table = new Table();
            table.AddColumn("File");
            table.AddColumn("Status");
            table.AddColumn("Value");
            table.AddColumn("Time", tc => tc.Alignment(Justify.Right));
            table.AddColumn("Error");
            var stopWatch = new Stopwatch();
            foreach (var file in allFiles)
            {
                var refoutFile = file.Replace(".in", ".refout");
                var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                var input = File.ReadAllText(file).TrimEnd();
                var iline = 0;
                stopWatch.Start();
                foreach (var line in solver.Solve(input))
                {
                    var elapsed = stopWatch.ElapsedMilliseconds;
                    var parts = new string[5];
                    parts[0] = file[commonPrefix.Length..];
                    parts[4] = "";
                    if (refout == null || refout.Length <= iline)
                    {
                        parts[1] = "[cyan]?[/]";
                    }
                    else if (refout[iline] == line.ToString())
                    {
                        parts[1] = "[darkgreen]✓[/]";
                    }
                    else
                    {
                        parts[1] = "[red]X[/]";
                        parts[4] = $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'";
                    }

                    parts[2] = line.ToString();

                    if (elapsed > 1000)
                    {
                        parts[3] = $"[red]{elapsed}[/]";
                    }
                    else if (elapsed > 500)
                    {
                        parts[3] = $"[orange]{elapsed}[/]";
                    }
                    else
                    {
                        parts[3] = $"[darkgreen]{elapsed}[/]";
                    }

                    table.AddRow(parts);
                    stopWatch.Reset();
                    iline++;
                }

            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();
        }
    }

    private static string GetLongestCommonPrefix(string[] s)
    {
        int k = s[0].Length;
        for (int i = 1; i < s.Length; i++)
        {
            k = Math.Min(k, s[i].Length);
            for (int j = 0; j < k; j++)
                if (s[i][j] != s[0][j])
                {
                    k = j;
                    break;
                }
        }
        return s[0].Substring(0, k);
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        Write(color, text + "\n");
    }
    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
}
