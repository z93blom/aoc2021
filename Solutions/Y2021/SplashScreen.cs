
namespace AdventOfCode.Y2021;

class SplashScreenImpl : SplashScreen
{
    public override void Show()
    {
        Write(0xffff66, "\r\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \r\n / _\\ (    \\/ )");
        Write(0xffff66, "( \\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \r\n/    \\ ) D (\\ \\/ / ) _) /    /  ");
        Write(0xffff66, ")(    (  O )) _)   ( (__(  O )) D ( ) _)         \r\n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)  ");
        Write(0xffff66, "   \\___)\\__/(____/(____)  2021\r\n\n           ");
        Write(0x888888, "                   ~  ");
        Write(0x00c8ff, "~");
        Write(0x888888, " ~ ");
        Write(0x00c8ff, "~");
        Write(0x888888, "~ ");
        Write(0x00c8ff, "~");
        Write(0x888888, "~");
        Write(0x00c8ff, "~~");
        Write(0x888888, "~");
        Write(0x00c8ff, "~~~~~~~~~~~~~~~  ");
        Write(0x888888, " 1 ");
        Write(0xffff66, "**\n                                                ");
        Write(0x888888, " ..   ");
        Write(0xa47a4d, "..''''  ");
        Write(0x888888, " 2 ");
        Write(0xffff66, "**\n           ");
        Write(0x888888, "                                          :         3\n                                              ");
        Write(0x888888, "                 4\n                                                               5\n                ");
        Write(0x888888, "                                               6\n                                                   ");
        Write(0x888888, "            7\n                                                               8\n                     ");
        Write(0x888888, "                                          9\n                                                        ");
        Write(0x888888, "      10\n                                                              11\n                          ");
        Write(0x888888, "                                    12\n                                                             ");
        Write(0x888888, " 13\n                                                              14\n                               ");
        Write(0x888888, "                               15\n                                                              16\n ");
        Write(0x888888, "                                                             17\n                                    ");
        Write(0x888888, "                          18\n                                                              19\n      ");
        Write(0x888888, "                                                        20\n                                         ");
        Write(0x888888, "                     21\n                                                              22\n           ");
        Write(0x888888, "                                                   23\n                                              ");
        Write(0x888888, "                24\n                                                              25\n           \n");
        
        Console.WriteLine();
    }
}