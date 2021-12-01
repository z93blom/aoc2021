using Spectre.Console;

namespace AdventOfCode;

public abstract class SplashScreen
{
    public abstract void Show();

    protected static void Write(int rgb, string text)
    {
        AnsiConsole.Markup($"[#{rgb:x6}]{text}[/]");
    }
}
