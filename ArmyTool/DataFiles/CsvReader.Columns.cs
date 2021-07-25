// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using BacteriaMage.OgreBattle.DataFiles.Columns;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public partial class CsvReader
    {
        private class Columns
        {
            private readonly List<BaseColumn> _dataSetColumns;
            private readonly List<BaseColumn> _matchedColumns;

            public int Count => _matchedColumns.Count;

            public BaseColumn this[int index] => _matchedColumns[index];

            public event EventHandler<ReadErrorEventArgs> OnMatchError;

            public Columns(IDataSet dataSet)
            {
                _dataSetColumns = new List<BaseColumn>(dataSet.GetColumns());
                _matchedColumns = new List<BaseColumn>();
            }

            public bool MatchColumns(string[] headers)
            {
                bool success = true;

                for (int i = 0; i < headers.Length; i++)
                {
                    success &= MatchColumn(i, headers[i]);
                }

                return success;
            }

            private bool MatchColumn(int columnNumber, string header)
            {
                if (ColumnAlreadyMatched(header))
                {
                    InvokeError(ReadErrors.DuplicateHeader, columnNumber, header);
                    return false;
                }

                BaseColumn column = FindDataSetColumn(header);

                if (column == null)
                {
                    InvokeError(ReadErrors.UnrecognizedHeader, columnNumber, header);
                    return false;
                }

                AddMatchedColumn(column);
                return true;
            }

            private void AddMatchedColumn(BaseColumn column)
            {
                _matchedColumns.Add(column);
            }

            private bool ColumnAlreadyMatched(string name)
            {
                return FindColumnIndex(_matchedColumns, name) != null;
            }

            private BaseColumn FindDataSetColumn(string name)
            {
                int? columnIndex = FindColumnIndex(_dataSetColumns, name);

                if (columnIndex.HasValue)
                {
                    return _dataSetColumns[columnIndex.Value];
                }
                else
                {
                    return null;
                }
            }

            private int? FindColumnIndex(List<BaseColumn> columns, string name)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    if (string.Equals(columns[i].Caption, name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return i;
                    }
                }

                return null;
            }

            private void InvokeError(ReadErrors error, int column, string cell)
            {
                ReadErrorEventArgs eventArgs = new ReadErrorEventArgs(error, 1, column, string.Empty, cell);

                OnMatchError?.Invoke(this, eventArgs);
            }
        }
    }
}
