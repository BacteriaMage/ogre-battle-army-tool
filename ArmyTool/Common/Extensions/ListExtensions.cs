// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;

namespace BacteriaMage.OgreBattle.Common
{
    public static class ListExtensions
    {
        public static string Join(this List<string> list, string separator)
        {
            return string.Join(separator, list);
        }

        public static string Join(this List<string> list)
        {
            return string.Join(string.Empty, list);
        }

        public static string JoinLines(this List<string> list)
        {
            return list.Join(Environment.NewLine);
        }
    }
}
