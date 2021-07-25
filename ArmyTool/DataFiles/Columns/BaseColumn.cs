// bacteriamage.wordpress.com

using System.Reflection;

namespace BacteriaMage.OgreBattle.DataFiles.Columns
{
    public abstract class BaseColumn
    {
        public string Caption
        {
            get; private set;
        }

        public IDataSet DataSet
        {
            get; private set;
        }

        public PropertyInfo Property
        {
            get; private set;
        }

        public BaseColumn(string caption, IDataSet dataSet, PropertyInfo property)
        {
            Caption = caption;
            DataSet = dataSet;
            Property = property;
        }

        public abstract string FormatCell();

        public abstract bool TryParseCell(string value);

        protected void SetObject(object value)
        {
            Property.SetValue(DataSet.GetRow(), value);
        }

        protected void SetInt(int? value)
        {
            SetObject(value);
        }

        protected object GetObject()
        {
            return Property.GetValue(DataSet.GetRow());
        }

        protected int? GetInt()
        {
            return (int?)GetObject();
        }
    }
}
