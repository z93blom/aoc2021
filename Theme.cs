namespace AdventOfCode;

public static class Theme
{
    public static Dictionary<string,int> GetDefaultTheme()
    {
        return new Dictionary<string, int>()
        {
            ["title"] = 0xffff66,
            ["calendar-star"] = 0xffff66,
            ["calendar-mark-complete"] = 0xffff66,
            ["calendar-mark-verycomplete"] = 0xffff66,

            ["calendar-edge"] = 0xcccccc,
            ["calendar-print-edge"] = 0x999999,
            ["calendar-print-text"] = 0xcccccc,

            ["calendar-ornament0"] = 0x0066ff,
            ["calendar-ornament1"] = 0xff9900,
            ["calendar-ornament2"] = 0xff0000,
            ["calendar-ornament3"] = 0xffff66,

            ["calendar-tree-star"] = 0xffff66,
            ["calendar-antenna-star"] = 0xffff66,
            ["calendar-antenna-signal0"] = 0xffff66,
            ["calendar-antenna-signal1"] = 0xffff66,
            ["calendar-antenna-signal2"] = 0xffff66,
            ["calendar-antenna-signal3"] = 0xffff66,
            ["calendar-antenna-signal4"] = 0xffff66,
            ["calendar-antenna-signal5"] = 0xffff66,
            ["calendar-tree-ornament0"] = 0x0066ff,
            ["calendar-tree-ornament1"] = 0xff9900,
            ["calendar-tree-ornament2"] = 0xff0000,
            ["calendar-tree-branches"] = 0x009900,
            ["calendar-tree-trunk"] = 0xaaaaaa,
            ["calendar-trunk"] = 0xcccccc,
            ["calendar-streets"] = 0x666666,
            ["calendar-window-dark"] = 0x333333,
            ["calendar-window-red"] = 0xff0000,
            ["calendar-window-green"] = 0x009900,
            ["calendar-window-blue"] = 0x0066ff,
            ["calendar-window-yellow"] = 0xffff66,
            ["calendar-window-brown"] = 0x553322,

            ["calendar-lightbeam"] = 0xffff66,

            ["calendar-color-s"] = 0x999999,
            ["calendar-color-b"] = 0x0066ff,
            ["calendar-color-e"] = 0xcccccc,
            ["calendar-color-r"] = 0xff0000,
            ["calendar-color-d"] = 0x880000,
            ["calendar-color-n"] = 0x886655,
            ["calendar-color-k"] = 0xcccccc,
            ["calendar-color-g"] = 0x009900,
            ["calendar-color-w"] = 0xcccccc,
            ["calendar-color-t"] = 0xcccccc,
            ["calendar-color-i"] = 0xff0000,
            ["calendar-color-y"] = 0xffff66,
            ["calendar-color-w1"] = 0x00c8ff,
        };
    }
}