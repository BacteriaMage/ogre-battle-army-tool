// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;

namespace BacteriaMage.OgreBattle.Common
{
    public class CaseInsensitiveComparer : IEqualityComparer<string>
    {
        public static readonly CaseInsensitiveComparer Instance = new CaseInsensitiveComparer();

        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLower().GetHashCode();
        }
    }
}
