// bacteriamage.wordpress.com

using System;
using System.Runtime.InteropServices;

namespace BacteriaMage.OgreBattle.Common
{
    public static class UserUtils
    {
        public static bool RunFromShell()
        {
            return GetConsoleProcessList(new int[2], 2) <= 1;
        }

        public static bool RunFromConsole()
        {
            return GetConsoleProcessList(new int[2], 2) > 1;
        }

        public static void WaitForAnyKey()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        public static void WaitIfRunFromShell()
        {
            if (RunFromShell())
            {
                WaitForAnyKey();
            }
        }

        [DllImport("kernel32.dll")]
        private static extern int GetConsoleProcessList(int[] buffer, int size);
    }
}
