// bacteriamage.wordpress.com

namespace BacteriaMage.OgreBattle.NamedValues
{
    public class TableEntry
    {
        public int? LineNumber { get; internal set; }
        public int? ValueId { get; internal set; }
        public string ValueName { get; internal set; }

        public TableEntry(int valueId, string valueName)
        {
            ValueId = valueId;
            ValueName = valueName;
        }

        public TableEntry(int lineNumber, int valueId, string valueName)
        {
            LineNumber = lineNumber;
            ValueId = valueId;
            ValueName = valueName;
        }

        public TableEntry()
        {
        }
    }
}
