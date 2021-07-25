// bacteriamage.wordpress.com

using System.Collections.Generic;

namespace BacteriaMage.OgreBattle.Common
{
    public static class DictionaryExtensions
    {
        public static V TryGetValue<K, V>(this Dictionary<K, V> dictionary, K key) where V : class
        {
            if (dictionary.TryGetValue(key, out V value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
