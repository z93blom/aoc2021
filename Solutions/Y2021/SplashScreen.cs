
namespace AdventOfCode.Y2021;

class SplashScreenImpl : SplashScreen
{
    public override void Show()
    {
        Write(0xffff66, "\r\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \r\n / _\\ (    \\/ )");
        Write(0xffff66, "( \\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \r\n/    \\ ) D (\\ \\/ / ) _) /    /  ");
        Write(0xffff66, ")(    (  O )) _)   ( (__(  O )) D ( ) _)         \r\n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)  ");
        Write(0xffff66, "   \\___)\\__/(____/(____)  2021\r\n\n           ");
        Write(0x00c8ff, "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  ");
        Write(0x888888, " 1 ");
        Write(0xffff66, "**\n           ");
        Write(0x00b5ed, "   '   .       .              .  ");
        Write(0xffffff, ". ");
        Write(0x00b5ed, "      . ");
        Write(0xa47a4d, "..''''  ");
        Write(0x888888, " 2 ");
        Write(0xffff66, "**\n           ");
        Write(0x00a2db, " .  .  .     ..  '  '       .   ");
        Write(0xffffff, ". ");
        Write(0x00a2db, "    '   ");
        Write(0xa47a4d, ":        ");
        Write(0x888888, " 3 ");
        Write(0xffff66, "**\n           ");
        Write(0x0091cc, "    .  .          .      .  .   ");
        Write(0xffffff, ".' ");
        Write(0x0091cc, " . ");
        Write(0xa47a4d, "....'        ");
        Write(0x888888, " 4 ");
        Write(0xffff66, "**\n             ");
        Write(0x0085c0, " .. ' .    .   .. ''  .     ");
        Write(0xc74c30, ".");
        Write(0xff0000, ".");
        Write(0xffffff, "|\\");
        Write(0xff0000, ".");
        Write(0xc74c30, ".");
        Write(0xa47a4d, "''             ");
        Write(0x888888, " 5 ");
        Write(0xffff66, "**\n                ");
        Write(0x0079b5, ".    .                  ");
        Write(0xa47a4d, ":                     ");
        Write(0x888888, " 6 ");
        Write(0xffff66, "**\n                      ");
        Write(0x888888, "   .         .  ");
        Write(0xa47a4d, ":'                      ");
        Write(0x888888, " 7 ");
        Write(0xffff66, "**\n           ");
        Write(0x888888, "                    .       '''''                   8\n                             . .     :'..     ");
        Write(0x888888, "                 9\n                                     :                        10\n                ");
        Write(0x888888, "                                              11\n                                                   ");
        Write(0x888888, "           12\n                                                              13\n                     ");
        Write(0x888888, "                                         14\n                                                        ");
        Write(0x888888, "      15\n                                                              16\n                          ");
        Write(0x888888, "                                    17\n                                                             ");
        Write(0x888888, " 18\n                                                              19\n                               ");
        Write(0x888888, "                               20\n                                                              21\n ");
        Write(0x888888, "                                                             22\n                                    ");
        Write(0x888888, "                          23\n                                                              24\n      ");
        Write(0x888888, "                                                        25\n           \n");
        
        Console.WriteLine();
    }
}