
using System;

namespace AdventOfCode.Y2021;

class SplashScreenImpl : AdventOfCode.SplashScreen
{

    public override void Show()
    {
        var color = Console.ForegroundColor;
            Write(0xffff66, "\r\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \r\n / _\\ (    \\/ )");
            Write(0xffff66, "( \\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \r\n/    \\ ) D (\\ \\/ / ) _) /    /  ");
            Write(0xffff66, ")(    (  O )) _)   ( (__(  O )) D ( ) _)         \r\n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)  ");
            Write(0xffff66, "   \\___)\\__/(____/(____)  2021\r\n\n           ");
            Write(0x888888, "                      ~   ~  ~ ~~ ~~~~~~~~~~~~~~~   1 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n           ");
            Write(0x888888, "                                      . .  ..''''   2\n                                              ");
            Write(0x888888, "                 3\n                                                               4\n                ");
            Write(0x888888, "                                               5\n                                                   ");
            Write(0x888888, "            6\n                                                               7\n                     ");
            Write(0x888888, "                                          8\n                                                        ");
            Write(0x888888, "       9\n                                                              10\n                          ");
            Write(0x888888, "                                    11\n                                                             ");
            Write(0x888888, " 12\n                                                              13\n                               ");
            Write(0x888888, "                               14\n                                                              15\n ");
            Write(0x888888, "                                                             16\n                                    ");
            Write(0x888888, "                          17\n                                                              18\n      ");
            Write(0x888888, "                                                        19\n                                         ");
            Write(0x888888, "                     20\n                                                              21\n           ");
            Write(0x888888, "                                                   22\n                                              ");
            Write(0x888888, "                23\n                                                              24\n                ");
            Write(0x888888, "                                              25\n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
    }
}