// bacteriamage.wordpress.com

using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public partial class CsvReader : IDisposable
    {
        public event EventHandler<ReadErrorEventArgs> OnReadError;

        public CsvReader(IDataSet dataSet, TextReader reader)
            : this(dataSet, new TextFieldParser(reader))
        {
        }

        public bool ReadDataSet()
        {
            return MatchColumns() && ReadRows();
        }

        #region Core

        private readonly IDataSet _dataSet;
        private readonly TextFieldParser _parser;

        private CsvReader(IDataSet dataSet, TextFieldParser parser)
        {
            _dataSet = dataSet;

            _parser = parser;
            _parser.TextFieldType = FieldType.Delimited;
            _parser.HasFieldsEnclosedInQuotes = true;
            _parser.SetDelimiters(",");

            _columns = new Columns(dataSet);
            _columns.OnMatchError += InvokeError;
        }

        public void Dispose()
        {
            _parser.Dispose();
        }

        #endregion

        #region Columns

        private readonly Columns _columns;

        private bool MatchColumns()
        {
            string[] headers = ReadFields();

            if (headers != null)
            {
                return _columns.MatchColumns(headers);
            }

            return false;
        }

        private string ColumnName(int column)
        {
            if (column >= 0 && column < _columns.Count)
            {
                return _columns[column].Caption;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Rows

        private bool ReadRows()
        {
            bool success = true;

            while (HaveMoreRows())
            {
                int rowNumber = RowNumber();

                string[] fields = ReadFields();

                if (!RowIsEmpty(fields))
                {
                    _dataSet.NewRow(rowNumber);

                    bool okay = ReadRow(fields);

                    _dataSet.RowDone(okay);

                    success = success & okay;
                }
            }

            return success;
        }

        private bool RowIsEmpty(string[] fields)
        {
            if (fields != null)
            {
                foreach (string cell in fields)
                {
                    if (!string.IsNullOrEmpty(cell))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ReadRow(string[] fields)
        {
            bool success = true;

            for (int column = 0; column < fields.Length; column++)
            {
                string value = fields[column];

                if (column < _columns.Count)
                {
                    success &= ReadCell(column, value);
                }
                else
                {
                    InvokeError(ReadErrors.UnexpectedCell, column, value);
                    success = false;
                }
            }

            return success;
        }

        private bool ReadCell(int column, string value)
        {
            if (!_columns[column].TryParseCell(value))
            {
                InvokeError(ReadErrors.InvalidCellValue, column, value);
                return false;
            }

            return true;
        }

        private bool HaveMoreRows()
        {
            return !_parser.EndOfData;
        }

        private int RowNumber()
        {
            return (int)_parser.LineNumber - 1;
        }

        private string[] ReadFields()
        {
            return _parser.EndOfData ? null : _parser.ReadFields();
        }

        #endregion

        #region Error Event

        private void InvokeError(ReadErrors error, int column, string value)
        {
            ReadErrorEventArgs eventArgs = new ReadErrorEventArgs(error, RowNumber(), column + 1, ColumnName(column), value);

            InvokeError(this, eventArgs);
        }

        private void InvokeError(object sender, ReadErrorEventArgs eventArgs)
        {
            OnReadError?.Invoke(this, eventArgs);
        }

        #endregion
    }
}