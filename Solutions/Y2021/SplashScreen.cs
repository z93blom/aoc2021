
namespace AdventOfCode.Y2021;

class SplashScreenImpl : SplashScreen
{
    public override void Show()
    {
        Write(0xffff66, "\r\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \r\n / _\\ (    \\/ )");
        Write(0xffff66, "( \\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \r\n/    \\ ) D (\\ \\/ / ) _) /    /  ");
        Write(0xffff66, ")(    (  O )) _)   ( (__(  O )) D ( ) _)         \r\n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)  ");
        Write(0xffff66, "   \\___)\\__/(____/(____)  2021\r\n\n           ");
        Write(0x888888, "              ~  ~ ");
        Write(0x00c8ff, "~");
        Write(0x888888, " ~");
        Write(0x00c8ff, "~ ~");
        Write(0x888888, "~");
        Write(0x00c8ff, "~~");
        Write(0x888888, "~");
        Write(0x00c8ff, "~~~~~~~~~~~~~~~~~~~~  ");
        Write(0x888888, " 1 ");
        Write(0xffff66, "**\n                                              ");
        Write(0x00c8ff, "   .    ");
        Write(0xa47a4d, "..''''  ");
        Write(0x888888, " 2 ");
        Write(0xffff66, "**\n                                                     ");
        Write(0xa47a4d, ":        ");
        Write(0x888888, " 3 ");
        Write(0xffff66, "**\n           ");
        Write(0x888888, "                                      ....'         4\n                                              ");
        Write(0x888888, "                 5\n                                                               6\n                ");
        Write(0x888888, "                                               7\n                                                   ");
        Write(0x888888, "            8\n                                                               9\n                     ");
        Write(0x888888, "                                         10\n                                                        ");
        Write(0x888888, "      11\n                                                              12\n                          ");
        Write(0x888888, "                                    13\n                                                             ");
        Write(0x888888, " 14\n                                                              15\n                               ");
        Write(0x888888, "                               16\n                                                              17\n ");
        Write(0x888888, "                                                             18\n                                    ");
        Write(0x888888, "                          19\n                                                              20\n      ");
        Write(0x888888, "                                                        21\n                                         ");
        Write(0x888888, "                     22\n                                                              23\n           ");
        Write(0x888888, "                                                   24\n                                              ");
        Write(0x888888, "                25\n           \n");
        
        Console.WriteLine();
    }
}