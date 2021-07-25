// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace BacteriaMage.OgreBattle.Common
{
    public static class StringExtensions
    {
        public static string Quote(this string value)
        {
            return string.Concat(@"""", value.Replace(@"""", @""""""), @"""");
        }

        public static string StripPrefix(this string value, IEnumerable<string> prefixes)
        {
            foreach (string prefix in prefixes)
            {
                if (value?.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase) ?? false)
                {
                    return value.Substring(prefix.Length);
                }
            }

            return value;
        }

        public static string Wrap(this string text, int width)
        {
            List<string> inputLines = text.ToLines();
            List<string> outputLines = new List<string>();

            inputLines.ForEach((line) =>
            {
                while (line.Length > width)
                {
                    int position = line.LastIndexOf(' ', width);

                    if (position < 0)
                    {
                        outputLines.Add(line.Substring(0, width));
                        line.Substring(width);
                    }
                    else
                    {
                        outputLines.Add(line.Substring(0, position).TrimEnd());
                        line = line.Substring(position).TrimStart();
                    }
                }

                outputLines.Add(line.TrimEnd());
            });

            return string.Join(Environment.NewLine, outputLines);
        }

        public static List<string> ToLines(this string text)
        {
            return text.ToLines(Environment.NewLine);
        }

        public static List<string> ToLines(this string text, params string[] lineBreaks)
        {
            string[] lines = text.Split(lineBreaks, StringSplitOptions.None);

            return new List<string>(lines);
        }

        public static bool ContainsCaseInsensitive(this string[] strings, string value)
        {
            return strings.Contains(value, CaseInsensitiveComparer.Instance);
        }
    }
}
