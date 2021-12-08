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
            table.AddColumn("Part");
            table.AddColumn("Value");
            table.AddColumn("Time (ms)", tc => tc.Alignment(Justify.Right));
            table.AddColumn("Error");
            var fileIndex = 0;
            var partIndex = 1;
            var valueIndex = 2;
            var timeIndex = 3;
            var errorIndex = 4;
            var stopWatch = new Stopwatch();
            foreach (var file in allFiles)
            {
                var refoutFile = file.Replace(".in", ".refout");
                var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                var input = File.ReadAllText(file).TrimEnd();
                var iline = 0;
                stopWatch.Start();
                var partNumber = 1;
                foreach (var line in solver.Solve(input))
                {
                    var elapsed = stopWatch.Elapsed;
                    var parts = new string[5];
                    if (partNumber == 1)
                    {
                        parts[fileIndex] = file[commonPrefix.Length..];
                    }
                    else
                    {
                        parts[fileIndex] = string.Empty;
                    }

                    if (refout == null || refout.Length <= iline)
                    {
                        parts[partIndex] = $"{partNumber++} [cyan]?[/]";
                        parts[errorIndex] = "";
                    }
                    else if (refout[iline] == line.ToString())
                    {
                        parts[partIndex] = $"{partNumber++} [darkgreen]✓[/]";
                        parts[errorIndex] = "";
                    }
                    else
                    {
                        parts[partIndex] = $"{partNumber++} [red]X[/]";
                        parts[errorIndex] = $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'";
                    }

                    parts[valueIndex] = line.ToString();

                    var milliseconds = elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond;
                    if (elapsed > TimeSpan.FromMilliseconds(1000))
                    {
                        parts[timeIndex] = $"[red]{milliseconds:0.###}[/]";
                    }
                    else if (elapsed > TimeSpan.FromMilliseconds(500))
                    {
                        parts[timeIndex] = $"[darkorange3_1]{milliseconds:0.###}[/]";
                    }
                    else
                    {
                        parts[timeIndex] = $"[darkgreen]{milliseconds:0.###}[/]";
                    }

                    table.AddRow(parts);
                    stopWatch.Restart();
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
