// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.Common;
using System.Reflection;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public class BooleanColumn : BaseColumn
    {
        public BooleanColumn(string caption, IDataSet dataSet, PropertyInfo property) 
            : base(caption, dataSet, property)
        {
        }

        public override string FormatCell()
        {
            return GetBool()?.ToYesNull() ?? string.Empty;
        }

        public override bool TryParseCell(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SetBool(null);
                return true;
            }
            else if (value.TryParseBool(out bool boolValue))
            {
                SetBool(boolValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool? GetBool() => (bool?)GetObject();

        private void SetBool(bool? value) => SetObject(value);
    }
}
