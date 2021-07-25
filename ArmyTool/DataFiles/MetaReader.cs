// bacteriamage.wordpress.com

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BacteriaMage.OgreBattle.DataFiles.Columns;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public class MetaReader : IDisposable
    {
        private readonly IDataSet _dataSet;
        private readonly BaseColumn[] _columns;
        private readonly TextReader _reader;

        private int _lineNumber;
        private bool _success = true;
        private bool _rowOkay = true;

        public event EventHandler<ReadErrorEventArgs> OnReadError;

        public MetaReader(IDataSet dataSet, string filePath)
            : this(dataSet, new StreamReader(filePath))
        {
        }

        public MetaReader(IDataSet dataSet, TextReader reader)
        {
            _dataSet = dataSet;
            _columns = dataSet.GetColumns();
            _reader = reader;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public bool ReadDataSet()
        {
            while (ReadLine(out string lineText))
            {
                ParseLine(lineText);
            }

            return _success;
        }

        private bool ReadLine(out string lineText)
        {
            _lineNumber++;

            lineText = _reader.ReadLine();

            return (lineText != null);
        }

        private void ParseLine(string lineText)
        {
            lineText = StripComment(lineText);

            string[] cells = SplitEntryCells(lineText);

            if (LineNotEmpty(cells))
            {
                NewRow();
                ParseCells(cells);
                RowDone();
            }
        }

        private string StripComment(string lineText)
        {
            int position = lineText.IndexOf(";");

            if (position < 0)
            {
                return lineText;
            }
            else if (position == 0)
            {
                return string.Empty;
            }
            else
            {
                return lineText.Substring(0, position).TrimEnd();
            }
        }

        private bool LineNotEmpty(string[] cells)
        {
            return !cells.All(cell => string.IsNullOrWhiteSpace(cell));
        }

        private string[] SplitEntryCells(string lineText)
        {
            if (lineText.Contains('\t'))
            {
                return lineText.Split('\t');
            }
            else
            {
                return SplitOnSpaces(lineText);
            }
        }

        private string[] SplitOnSpaces(string lineText)
        {
            string delimitedText = Regex.Replace(lineText.TrimEnd(), "  +", "\t");

            string[] cells = delimitedText.Split('\t');

            NormalizeEmptyCells(cells);

            return cells;
        }

        private void NormalizeEmptyCells(string[] cells)
        {
            for (int index = 0; index < cells.Length; index++)
            {
                if (cells[index] == "-")
                {
                    cells[index] = "";
                }
            }
        }

        private void NewRow()
        {
            _dataSet.NewRow(_lineNumber);
            _rowOkay = true;
        }

        private void RowDone()
        {
            _dataSet.RowDone(_rowOkay);
        }

        private void ParseCells(string[] cells)
        {
            for (int index = 0; index < cells.Length; index++)
            {
                if (index < _columns.Length)
                {
                    ParseCell(index, cells[index]);
                }
                else
                {
                    UnexpectedCell(index, cells[index]);
                }
            }
        }

        private void ParseCell(int index, string text)
        {
            if (!_columns[index].TryParseCell(text.Trim()))
            {
                InvalidValue(index, text);
            }
        }

        private void UnexpectedCell(int index, string text)
        {
            InvokeError(ReadErrors.UnexpectedCell, index, text);
        }

        private void InvalidValue(int index, string text)
        {
            InvokeError(ReadErrors.InvalidCellValue, index, text);
        }

        private void InvokeError(ReadErrors error, int index, string text)
        {
            InvokeError(new ReadErrorEventArgs(error, _lineNumber, index + 1, ColumnName(index), text));
        }

        private void InvokeError(ReadErrorEventArgs eventArgs)
        {
            _success = false;
            _rowOkay = false;
            OnReadError?.Invoke(this, eventArgs);
        }

        private string ColumnName(int index)
        {
            return index < _columns.Length ? _columns[index].Caption : null;
        }
    }
}