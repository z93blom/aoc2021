
namespace AdventOfCode.Y2021;

class SplashScreenImpl : SplashScreen
{
    public override void Show()
    {
        WriteFiglet("Advent of code 2021", Spectre.Console.Color.Yellow);
        Write(0x00c8ff, "           ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  ");
        Write(0x888888, " 1 ");
        Write(0xffff66, "**\n           ");
        Write(0x00b5ed, "    .                      ..    ");
        Write(0xffffff, ".         ");
        Write(0xa47a4d, "..''''  ");
        Write(0x888888, " 2 ");
        Write(0xffff66, "**\n           ");
        Write(0x00a2db, " .       ..  .        . '     . ");
        Write(0xffffff, ". ");
        Write(0x00a2db, "    .   ");
        Write(0xa47a4d, ":        ");
        Write(0x888888, " 3 ");
        Write(0xffff66, "**\n           ");
        Write(0x0091cc, ".         . ..           ...    ");
        Write(0xffffff, ".'    ");
        Write(0xa47a4d, "....'        ");
        Write(0x888888, " 4 ");
        Write(0xffff66, "**\n           ");
        Write(0x0085c0, "  .   .      ' .   ~   '.     ");
        Write(0xc74c30, ".");
        Write(0xff0000, ".");
        Write(0xffffff, "|\\");
        Write(0xff0000, ".");
        Write(0xc74c30, ".");
        Write(0xa47a4d, "''             ");
        Write(0x888888, " 5 ");
        Write(0xffff66, "**\n           ");
        Write(0x0079b5, "     .         '  .    .     ");
        Write(0xa47a4d, ":                     ");
        Write(0x888888, " 6 ");
        Write(0xffff66, "**\n           ");
        Write(0x006daa, "  .     '      '   .    '  ");
        Write(0xa47a4d, ":'                      ");
        Write(0x888888, " 7 ");
        Write(0xffff66, "**\n           ");
        Write(0x00619f, " .. .'    . ~     .  . .    ");
        Write(0xa47a4d, "'''''.....  ..");
        Write(0xc74c30, ".");
        Write(0xff0000, ".       ");
        Write(0x888888, " 8 ");
        Write(0xffff66, "**\n           ");
        Write(0x005a98, "            .    .        ");
        Write(0xa47a4d, ":'..  ..    ''    ");
        Write(0xff0000, "':     ");
        Write(0x888888, " 9 ");
        Write(0xffff66, "**\n           ");
        Write(0x005291, " ..         ...   ' . .   ");
        Write(0xa47a4d, ":   ''  ''''..     ");
        Write(0xc74c30, "'");
        Write(0xa47a4d, ".    ");
        Write(0x888888, "10 ");
        Write(0xffff66, "**\n           ");
        Write(0x888888, "    .           '   . .   ");
        Write(0xa47a4d, ":             '..'. :    ");
        Write(0x888888, "11 ");
        Write(0xffff66, "**\n           ");
        Write(0x888888, ".           . .   .  .                    ..' :    12\n                                              ");
        Write(0x888888, "     '' ...:    13\n                                                   ..'        14\n                ");
        Write(0x888888, "                                              15\n                                                   ");
        Write(0x888888, "           16\n                                                              17\n                     ");
        Write(0x888888, "                                         18\n                                                        ");
        Write(0x888888, "      19\n                                                              20\n                          ");
        Write(0x888888, "                                    21\n                                                             ");
        Write(0x888888, " 22\n                                                              23\n                               ");
        Write(0x888888, "                               24\n                                                              25\n ");
        Write(0x888888, "          \n");
        
        Console.WriteLine();
    }
}