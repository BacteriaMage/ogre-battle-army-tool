// bacteriamage.wordpress.com

using System;
using System.Collections.Generic;
using System.Linq;
using BacteriaMage.OgreBattle.Common;

namespace BacteriaMage.OgreBattle.ArmyTool.CLI
{
    public class MessageDisplay
    {
        private const int ColumnPadding = 2;

        private Column _lineCol;
        private Column _errorCol;
        private Column _messageColumn;

        private void BuildColumns()
        {
            _lineCol = BuildColumn("Line", 4, Align.Right);
            _errorCol = BuildColumn("Err", 3);
            _messageColumn = BuildColumn("Message", RemainingColumnWidth());
        }

        public void WriteMessage(int? line, string message)
        {
            WriteMessage(line, message, false);
        }

        public void WriteError(int? line, string message)
        {
            WriteMessage(line, message, true);
        }

        private void WriteMessage(int? line, string message, bool isError)
        {
            Queue<string> lines = new Queue<string>(WrapMessage(message));

            DisplayHeaders();

            WriteMessageLine(line?.ToString() ?? " -- ", lines.Dequeue(), isError);

            foreach (string lineText in lines)
            {
                WriteMessageLine(string.Empty, lineText, false);
            }
        }

        private void WriteMessageLine(string lineNumber, string lineText, bool isError)
        {
            string errorText = isError ? " X " : string.Empty;

            _lineCol.DisplayText(lineNumber);
            _errorCol.DisplayText(errorText, ConsoleColor.DarkRed);
            _messageColumn.DisplayText(lineText);

            Console.WriteLine();
        }

        private List<string> WrapMessage(string message)
        {
            // don't wrap if the message just fits in the space
            if (_messageColumn.MessageFits(message))
            {
                return new List<string>(new string[] { message });
            }

            List<string> lines = message.Replace(". ", "." + Environment.NewLine).ToLines(Environment.NewLine);

            // first try to fit a full sentence on each line
            if (lines.All((line) => _messageColumn.MessageFits(line)))
            {
                return lines;
            }
            
            // otherwise use the usual word wrap
            return message.Wrap(_messageColumn.Width).ToLines();
        }

        #region Headers

        private bool _headersDisplayed;

        private void DisplayHeaders()
        {
            if (!_headersDisplayed)
            {
                Console.WriteLine();

                _columns.ForEach((column) => column.DisplayHeader());

                Console.WriteLine();

                _headersDisplayed = true;
            }
        }

        #endregion

        #region Columns

        private List<Column> _columns = new List<Column>();

        private Column BuildColumn(string caption, int width)
        {
            return BuildColumn(caption, width, Align.Left);
        }

        private Column BuildColumn(string caption, int width, Align align)
        {
            Column column = new Column()
            {
                Caption = caption,
                Width = width,
                Align = align,
            };

            _columns.Add(column);

            return column;
        }

        private int RemainingColumnWidth()
        {
            // get the base width
            int width = Math.Max(Console.BufferWidth, 40);

            // remove the existing column widths
            _columns.ForEach((column) => width -= column.Width);

            // remove spacing between the existing columns
            width -= ColumnPadding * _columns.Count;

            // remove spacing around the remaining space
            width -= ColumnPadding * 2;

            return Math.Max(width, 0);
        }

        private enum Align
        {
            Left,
            Right,
        }

        private struct Column
        {
            public string Caption;
            public int Width;
            public Align Align;

            public void DisplayHeader()
            {
                DisplayText(Caption, Align.Left, ConsoleColor.Yellow);
            }

            public bool MessageFits(string text)
            {
                return text.Length <= Width;
            }

            public void DisplayText(string text)
            {
                DisplayText(text, Align);
            }

            public void DisplayText(string text, ConsoleColor color)
            {
                DisplayText(text, Align, color);
            }

            private void DisplayText(string text, Align align, ConsoleColor color)
            {
                ConsoleColor oldColor = Console.ForegroundColor;

                try
                {
                    Console.ForegroundColor = color;
                    DisplayText(text, align);
                }
                finally
                {
                    Console.ForegroundColor = oldColor;
                }
            }

            private void DisplayText(string text, Align align)
            {
                text = text ?? string.Empty;

                if (text.Length > Width)
                {
                    text = text.Substring(0, Width);
                }
                else if (align == Align.Left)
                {
                    text = text.PadRight(Width, ' ');
                }

                text = text.PadLeft(Width + ColumnPadding, ' ');

                Console.Write(text);
            }
        }

        #endregion

        #region Constructors

        public MessageDisplay()
        {
            BuildColumns();
        }

        #endregion
    }
}
