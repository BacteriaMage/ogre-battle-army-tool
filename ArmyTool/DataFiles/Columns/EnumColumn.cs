// bacteriamage.wordpress.com

using System;
using System.Reflection;
using BacteriaMage.OgreBattle.Common;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public class EnumColumn : BaseColumn
    {
        public Type EnumType
        {
            get; private set;
        }

        public EnumColumn(string caption, IDataSet dataSet, PropertyInfo property, Type enumType) 
            : base(caption, dataSet, property)
        {
            EnumType = enumType.NormalizeNullable();
        }

        public override string FormatCell()
        {
            object value = GetObject();

            if (value == null)
            {
                return string.Empty;

            }
            else
            {
                return Enum.GetName(EnumType, value);
            }
        }

        public override bool TryParseCell(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SetObject(null);
                return true;
            }
            else if (TryParseEnum(value, out object enumValue))
            {
                SetObject(enumValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryParseEnum(string value, out object enumValue)
        {
            try
            {
                enumValue = Enum.Parse(EnumType, value);
                return true;
            }
            catch
            {
                enumValue = null;
                return false;
            }
        }
    }
}
