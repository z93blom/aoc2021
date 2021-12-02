using System.Text;
using AdventOfCode.Model;

namespace AdventOfCode.Generator;

public class SplashScreenGenerator {
    public string Generate(Calendar calendar, Dictionary<string, int> themeColors) {
        string calendarPrinter = CalendarPrinter(calendar, themeColors);
        return $@"
            |namespace AdventOfCode.Y{calendar.Year};
            |
            |class SplashScreenImpl : SplashScreen
            |{{
            |    public override void Show()
            |    {{
            |        {calendarPrinter.Indent(8)}
            |        Console.WriteLine();
            |    }}
            |}}".StripMargin();
    }

    private static string CalendarPrinter(Calendar calendar, Dictionary<string, int> themeColors) {
        var lines = calendar.Lines.Select(line =>
            new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();
        lines.Insert(0, new[]{new CalendarToken {
            Styles = new []{"title"},
            Text = $@"
                |  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         
                | / _\ (    \/ )( \(  __)(  ( \(_  _)   /  \(  __)   / __)/  \(    \(  __)        
                |/    \ ) D (\ \/ / ) _) /    /  )(    (  O )) _)   ( (__(  O )) D ( ) _)         
                |\_/\_/(____/ \__/ (____)\_)__) (__)    \__/(__)     \___)\__/(____/(____)  {calendar.Year}
                |"
            .StripMargin()
        }});

        var bw = new BufferWriter();
        foreach (var line in lines)
        {
            foreach (var token in line)
            {
                var consoleColor = 0x888888;
                foreach (var s in token.Styles)
                {
                    if (themeColors.ContainsKey(s))
                    {
                        consoleColor = themeColors[s];
                        break;
                    }
                }

                bw.Write(consoleColor, token.Text);
            }

            bw.Write(-1, "\n");
        }

        return bw.GetContent();
    }

    class BufferWriter {
        StringBuilder sb = new StringBuilder();
        int bufferColor = -1;
        string buffer = "";

        public void Write(int color, string text) {
            if (!string.IsNullOrWhiteSpace(text)) {
                if (color != bufferColor && !string.IsNullOrWhiteSpace(buffer)) {
                    Flush();
                }
                bufferColor = color;
            }
            buffer += text;
        }

        private void Flush() {
            while (buffer.Length > 0) {
                var block = buffer.Substring(0, Math.Min(100, buffer.Length));
                buffer = buffer.Substring(block.Length);
                block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                sb.AppendLine($@"Write(0x{bufferColor:x6}, ""{block}"");");
            }
            buffer = "";
        }

        public string GetContent() {
            Flush();
            return sb.ToString();
        }
    }
}
