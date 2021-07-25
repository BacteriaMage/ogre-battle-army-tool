// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.Common
{
    public static class TypeExtensions
    {
        public static Type NormalizeNullable(this Type type)
        {
            if (type.IsNullable())
            {
                return Nullable.GetUnderlyingType(type);
            }

            return type;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
