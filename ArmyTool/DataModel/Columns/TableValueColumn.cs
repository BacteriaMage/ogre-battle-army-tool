// bacteriamage.wordpress.com

using System.Reflection;
using BacteriaMage.OgreBattle.DataFiles;
using BacteriaMage.OgreBattle.DataFiles.Columns;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel.Columns
{
    public class TableValueColumn : HexColumn
    {
        public ValuesTable Table
        {
            get; private set;
        }

        public TableValueColumn(string caption, IDataSet dataSet, PropertyInfo property, ValuesTable table)
            : base(caption, dataSet, property)
        {
            Table = table;
        }

        public override string FormatCell()
        {
            int? value = GetInt();

            if (value.HasValue && Table.TryGetValueName(value.Value, out string valueName))
            {
                return valueName;
            }
            else
            {
                return base.FormatCell();
            }
        }

        public override bool TryParseCell(string value)
        {
            if (Table.TryGetValueId(value, out int valueId))
            {
                SetInt(valueId);
                return true;
            }
            else
            {
                return base.TryParseCell(value);
            }
        }
    }
}
