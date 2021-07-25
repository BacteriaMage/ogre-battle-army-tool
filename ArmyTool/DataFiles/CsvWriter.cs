// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.IO;
using BacteriaMage.OgreBattle.Common;
using BacteriaMage.OgreBattle.DataFiles.Columns;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public class CsvWriter : IDisposable
    {
        private readonly IDataSet _dataSet;
        private readonly TextWriter _writer;

        private readonly BaseColumn[] _columns;

        private List<string> _cells;

        public CsvWriter(IDataSet dataSet, string filePath)
            : this(dataSet, new StreamWriter(filePath))
        {
        }

        public CsvWriter(IDataSet dataSet, TextWriter writer)
        {
            _dataSet = dataSet;
            _writer = writer;

            _columns = dataSet.GetColumns();

            _cells = new List<string>();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }

        public void WriteDataSet()
        {
            WriteHeaders();
            WriteAllRows();
        }

        private void WriteHeaders()
        {
            foreach(BaseColumn column in _columns)
            {
                Append(column.Caption);
            }

            NextRow();
        }

        private void WriteAllRows()
        {
            for (; ; )
            {
                object row = _dataSet.NextRow();

                if (row != null)
                {
                    WriteRow(row);
                }
                else
                {
                    break;
                }
            }
        }

        private void WriteRow(object row)
        {
            foreach(BaseColumn column in _columns)
            {
                Append(column.FormatCell());
            }

            NextRow();
        }

        private void Append(string cell)
        {
            _cells.Add(cell?.Quote() ?? string.Empty.Quote());
        }

        private void NextRow()
        {
            string row = _cells.Join(",");

            _writer.WriteLine(row);

            _cells = new List<string>();
        }
    }
}