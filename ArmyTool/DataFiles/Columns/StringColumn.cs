// bacteriamage.wordpress.com

using System.Reflection;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public class StringColumn : BaseColumn
    {
        public StringColumn(string caption, IDataSet dataSet, PropertyInfo property) 
            : base(caption, dataSet, property)
        {
        }

        public override string FormatCell()
        {
            return GetString() ?? string.Empty;
        }

        public override bool TryParseCell(string value)
        {

            SetString(value ?? string.Empty);
            return true;
        }

        private string GetString() => (string)GetObject();

        private void SetString(string value) => SetObject(value);
    }
}
