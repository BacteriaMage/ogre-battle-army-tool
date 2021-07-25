// bacteriamage.wordpress.com

using System;

namespace BacteriaMage.OgreBattle.DataFiles
{
    public enum ReadErrors
    {
        UnrecognizedHeader,
        DuplicateHeader,
        UnexpectedCell,
        InvalidCellValue,
    }

    public class ReadErrorEventArgs : EventArgs
    {
        public ReadErrors Error { get; private set; }

        public int Row { get; private set; }

        public int Column { get; private set; }

        public string HeaderText { get; private set; }

        public string CellText { get; private set; }

        public string Message
        {
            get
            {
                switch (Error)
                {
                    case ReadErrors.UnrecognizedHeader:
                        return $"\"{CellText}\" is not a recognized header in column {Column}.";
                    case ReadErrors.DuplicateHeader:
                        return $"The header {CellText} in column {Column} was already specified in another column.";
                    case ReadErrors.UnexpectedCell:
                        return $"Unexpected value \"{CellText}\" in {Column}.";
                    default:
                        return $"\"{CellText}\" is not a valid value for {HeaderText}.";
                }
            }
        }

        public ReadErrorEventArgs(ReadErrors error, int row, int column, string headerText, string cellText)
        {
            Error = error;
            Row = row;
            Column = column;
            HeaderText = headerText;
            CellText = cellText;
        }
    }
}
