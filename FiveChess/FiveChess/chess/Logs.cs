using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
    class Logs
    {
        public const int level1 = 1;
        public const int level2 = 2;
        public const int level3 = 3;
        public const int level4 = 4;

        public static bool writeLog { get; set; } = true;
        public static int level { get; set; } = level3;

        public static void writeln(string log, int level = level1)
        {
            if(writeLog && level >= Logs.level)
            {
                System.Console.WriteLine(log);
            }
        }

    }
}
