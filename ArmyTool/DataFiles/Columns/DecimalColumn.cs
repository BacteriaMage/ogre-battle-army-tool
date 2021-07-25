// bacteriamage.wordpress.com

using System.Reflection;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public class DecimalColumn : BaseColumn
    {
        public DecimalColumn(string caption, IDataSet dataSet, PropertyInfo property)
            : base(caption, dataSet, property)
        {
        }

        public override string FormatCell()
        {
            int? value = GetInt();

            if (value.HasValue)
            {
                return value.Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public override bool TryParseCell(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                SetInt(null);
                return true;
            }
            else if (int.TryParse(value, out int dec))
            {
                SetInt(dec);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
