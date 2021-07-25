// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.Common
{
    public class OgreBattleException : Exception
    {
        public OgreBattleException(string message)
            : base(message)
        {
        }

        public static OgreBattleException New(string message)
        {
            return new OgreBattleException(message);
        }

        public static void Throw(string message)
        {
            throw New(message);
        }
    }
}
