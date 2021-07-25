// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.Common;
using System.Reflection;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public class HexColumn : BaseColumn
    {
        public HexColumn(string caption, IDataSet dataSet, PropertyInfo property)
            : base(caption, dataSet, property)
        {
        }

        public override string FormatCell()
        {
            int? value = GetInt();

            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.Value.FormatHex();
            }
        }

        public override bool TryParseCell(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SetInt(null);
                return true;
            }
            else if (value.TryParseHex(out int hexValue))
            {
                SetInt(hexValue);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
