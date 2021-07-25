// bacteriamage.wordpress.com

using static System.Globalization.NumberStyles;
using static System.Globalization.CultureInfo;

namespace BacteriaMage.OgreBattle.Common
{
    public static class HexExtensions
    {
        #region Formatting

        public static string FormatHex(this int value)
        {
            return value > 0xff ? value.FormatHexWord() : value.FormatHexByte();
        }

        public static string FormatHexByte(this int value)
        {
            return $"0x{(value & 0xff):X2}";
        }

        public static string FormatHexWord(this int value)
        {
            return $"0x{(value & 0xffff):X4}";
        }

        #endregion

        #region Parsing

        public static int ParseHex(this string value)
        {
            return int.Parse(StripPrefix(value), HexNumber, InvariantCulture);
        }

        public static int? TryParseHex(this string value)
        {
            if (value.TryParseHex(out int hex))
            {
                return hex;
            }

            return null;
        }

        public static bool TryParseHex(this string value, out int hex)
        {
            return int.TryParse(StripPrefix(value), HexNumber, InvariantCulture, out hex);
        }

        private static string StripPrefix(string value)
        {
            return value.StripPrefix(new string[] { "0x", "&h", "$" });
        }

        #endregion
    }
}
