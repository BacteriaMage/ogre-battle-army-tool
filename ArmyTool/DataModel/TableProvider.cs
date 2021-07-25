// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.ArmyTool.Metadata;
using BacteriaMage.OgreBattle.NamedValues;

namespace BacteriaMage.OgreBattle.ArmyTool.DataModel
{
    public class TableProvider
    {
        public ValuesTable NamesTable
        {
            get; set;
        }

        public ValuesTable IdentitiesTable
        {
            get; set;
        }

        public ClassesTable ClassesTable
        {
            get; set;
        }

        public ValuesTable ItemsTable
        {
            get; set;
        }
    }
}
