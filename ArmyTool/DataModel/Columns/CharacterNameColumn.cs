// bacteriamage.wordpress.com

using System.Reflection;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.DataFiles;
using BacteriaMage.OgreBattle.DataFiles.Columns;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel.Columns
{
    public class CharacterNameColumn : BaseColumn
    {
        public ValuesTable Table
        {
            get; private set;
        }

        private CharacterName Name
        {
            get => (CharacterName)GetObject();
            set => SetObject(value);
        }

        public CharacterNameColumn(string caption, IDataSet dataSet, PropertyInfo property, ValuesTable table)
            : base(caption, dataSet, property)
        {
            Table = table;
        }

        public override string FormatCell()
        {
            return FormatText() ?? FormatId() ?? string.Empty;
        }

        private string FormatText()
        {
            string text = Name?.NameText;
            return string.IsNullOrEmpty(text) ? null : text;
        }

        private string FormatId()
        {
            int? id = Name?.NameId;

            if (id == null)
            {
                return null;
            }
            else if (Table.TryGetValueName(id.Value, out string text))
            {
                return text;
            }
            else
            {
                return id.Value.FormatHexWord();
            }
        }

        public override bool TryParseCell(string value)
        {
            Name = new CharacterName()
            {
                NameText = value,
                NameId = TryParseText(value),
            };

            return true;
        }

        private int? TryParseText(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (Table.TryGetValueId(text, out int nameId) || text.TryParseHex(out nameId))
                {
                    return nameId;
                }
            }

            return null;
        }
    }
}
