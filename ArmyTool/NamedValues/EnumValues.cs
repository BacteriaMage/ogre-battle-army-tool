// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BacteriaMage.OgreBattle.Common;

namespace BacteriaMage.OgreBattle.NamedValues
{
    public class EnumValues<T> : ReadOnlyDictionary<string, T> where T : struct
    {
        public EnumValues() : base(BuildDictionary())
        {
            if (!TypeIsEnum())
            {
                throw new ArgumentException($"{typeof(T).ToString()} is not an Enum type");
            }
        }

        public static List<string> ToList()
        {
            List<string> list = new List<string>(new EnumValues<T>().Keys);

            list.Sort();

            return list;
        }

        private static bool TypeIsEnum()
        {
            // unfortunately there's no way to check this at compile time
            return typeof(T).IsEnum;
        }

        private static Dictionary<string, T> BuildDictionary()
        {
            Dictionary<string, T> map = new Dictionary<string, T>(CaseInsensitiveComparer.Instance);

            if (TypeIsEnum())
            {
                foreach (T value in YieldValues())
                {
                    map.Add(value.ToString(), value);
                }
            }

            return map;
        }

        private static IEnumerable<T> YieldValues()
        {
            foreach (object value in typeof(T).GetEnumValues())
            {
                yield return (T)value;
            }
        }
    }
}
