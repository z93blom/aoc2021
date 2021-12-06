using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utilities
{
    struct Point
    {
        public long X { get; }
        public long Y { get; }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }
    }
}
