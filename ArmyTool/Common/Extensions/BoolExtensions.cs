// bacteriamage.wordpress.com

using static System.StringComparison;

namespace BacteriaMage.OgreBattle.Common
{
    public static class BoolExtensions
    {
        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }

        public static string ToYesNull(this bool value)
        {
            return value ? value.ToYesNo() : string.Empty;
        }

        public static bool TryParseBool(this string value, out bool result)
        {
            if (MatchAny(value, "Yes", "Y", "True", "T"))
            {
                result = true;
                return true;
            }
            else if (MatchAny(value, "No", "N", "False", "F"))
            {
                result = false;
                return true;
            }
            else if (int.TryParse(value, out int numberValue))
            {
                result = (numberValue != 0);
                return true;
            }
            else
            {
                result = false;
                return false;
            }
        }

        private static bool MatchAny(string value, params string[] matches)
        {
            foreach (string match in matches)
            {
                if (value != null && string.Equals(value, match, InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
