// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public static class Extensions
    {
        public static int?[] ToNumbers(this List<Character> characters)
        {
            return characters.ConvertAll((character) => character.Number).ToArray();
        }

        public static int[] ToLines(this List<Character> characters)
        {
            return characters.ConvertAll((character) => character.LineNumber).ToArray();
        }

        public static List<Character> GetLordCharacters(this List<Character> characters)
        {
            return characters.Where((character) => character.IsLord).ToList();
        }

        public static List<Character> GetUnitLeaders(this List<Character> characters)
        {
            return characters.Where((character) => character.UnitLeader ?? false).ToList();
        }

        public static List<Character> GetLargeCharacters(this List<Character> characters)
        {
            return characters.Where((character) => character.IsLarge).ToList();
        }

        public static List<Character> GetSmallCharacters(this List<Character> characters)
        {
            return characters.Where((character) => !character.IsLarge).ToList();
        }

        public static void ReplaceRange(this List<Character> characters, IEnumerable<Character> collection)
        {
            characters.Clear();
            characters.AddRange(collection);
        }

        public static void StableSort<T>(this List<Character> characters, Func<Character, T> keySelector)
        {
            characters.ReplaceRange(new List<Character>(characters.OrderBy(keySelector)));
        }
    }
}
