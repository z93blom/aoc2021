
namespace AdventOfCode.Y2021;

class Theme : ITheme
{
    public Dictionary<string, int> Override(Dictionary<string, int> themeColors)
    {
        themeColors["calendar-color-g"] = 0xa47a4d;
        themeColors["calendar-color-s"] = 0xffffff;
        return themeColors;
    }
}