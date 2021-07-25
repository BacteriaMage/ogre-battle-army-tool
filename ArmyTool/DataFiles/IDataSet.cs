// bacteriamage.wordpress.com

using BacteriaMage.OgreBattle.DataFiles.Columns;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public interface IDataSet
    {
        BaseColumn[] GetColumns();

        object GetRow();

        object NextRow();

        object NewRow(int number);
        void RowDone(bool okay);
    }
}
