using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace RoughBench
{
    /// <summary>
    ///  ConsoleTable writes formatted tables in the console, supporting highlighting and right-aligned columns.
    ///  Output is Markdown compatible for easy copy and paste.
    /// </summary>
    public class ConsoleTable
    {
        public IReadOnlyList<TableCell> Columns { get; }
        private List<TableCell[]> Rows { get; }
        private int[] ColumnWidths { get; }

        private TableColor DefaultColor { get; set; }
        private Point Start { get; set; }

        public ConsoleTable(IEnumerable<string> columnNames) :
            this(columnNames.Select((name) => new TableCell(name)).ToArray())
        { }

        public ConsoleTable(params string[] columnNames) :
            this(columnNames.Select((name) => new TableCell(name)).ToArray())
        { }

        public ConsoleTable(IEnumerable<TableCell> columns) : this(columns.ToArray())
        { }

        public ConsoleTable(params TableCell[] columns)
        {
            Columns = columns;
            Rows = new List<TableCell[]>();
            ColumnWidths = columns.Select((col) => col.Text?.Length ?? 0).ToArray();
        }

        public void AppendRow(IEnumerable<string> values)
        {
            AppendRow(values.Select((text) => new TableCell(text)).ToArray());
        }

        public void AppendRow(params string[] values)
        {
            AppendRow(values.Select((text) => new TableCell(text)).ToArray());
        }

        public void AppendRow(IEnumerable<TableCell> values)
        {
            AppendRow(values.ToArray());
        }

        public void AppendRow(params TableCell[] values)
        {
            if (values.Length > Columns.Count) { throw new FormatException($"Row had too many columns. ({Columns.Count} columns, {values.Length} in this row"); }
            
            Rows.Add(values);

            bool redrawRequired = UpdateColumnWidths(values);

            if (Rows.Count == 1)
            {
                // If this is the first row, capture the start position and color (just in time)
                DefaultColor = (TableColor)Console.ForegroundColor;
                Start = new Point(Console.CursorLeft, Console.CursorTop);

                WriteHeader(Console.Out);
                WriteRow(Console.Out, values);

            }
            else if (redrawRequired)
            {
                WriteTable(Console.Out);
            }
            else
            {
                WriteRow(Console.Out, values);
            }
        }

        private bool UpdateColumnWidths(TableCell[] values)
        {
            bool redrawRequired = false;
            for (int i = 0; i < ColumnWidths.Length; ++i)
            {
                int valueLength = values[i].Text.Length;
                if (ColumnWidths[i] < valueLength)
                {
                    redrawRequired = true;
                    ColumnWidths[i] = valueLength;
                }
            }

            return redrawRequired;
        }

        /// <summary>
        ///  Save a copy of the Table (so far) to a stream.
        /// </summary>
        /// <param name="stream">Stream to write table copy to</param>
        public void Save(Stream stream)
        {
            using (stream)
            using (StreamWriter writer = new StreamWriter(stream))
            {
                WriteTable(writer);
            }
        }

        private void WriteTable(TextWriter writer)
        {
            if (writer == Console.Out)
            {
                Console.CursorTop = Start.Y;
                Console.CursorLeft = Start.X;
            }

            WriteHeader(writer);

            foreach (TableCell[] row in Rows)
            {
                WriteRow(writer, row);
            }
        }

        private void WriteHeader(TextWriter writer)
        {
            if (writer == Console.Out)
            {
                Console.WriteLine();
            }

            // Write column headings
            WriteRow(writer, Columns);

            // Write separator row
            WriteRow(writer, Separators());
        }

        private void WriteRow(TextWriter writer, IReadOnlyList<TableCell> row)
        {
            writer.Write(" | ");

            for (int i = 0; i < row.Count; ++i)
            {
                WriteCell(writer, row[i], i);
            }

            writer.WriteLine();
        }

        private void WriteCell(TextWriter writer, TableCell cell, int columnIndex)
        {
            // Resolve Alignment and Color to column values, if cell has defaulted them
            TableCell resolved = cell.Resolve(Columns[columnIndex]);
            int padLength = Math.Max(0, ColumnWidths[columnIndex] - cell.Text.Length);

            // Set color, if needed
            if (resolved.Color != TableColor.Default && writer == Console.Out)
            {
                Console.ForegroundColor = (ConsoleColor)resolved.Color;
            }

            // Write any left padding
            if (padLength > 0 && cell.Align == Align.Right)
            {
                writer.Write(new string(' ', padLength));
            }

            // Write text
            writer.Write(cell.Text);

            // Write any right padding
            if (padLength > 0 && cell.Align != Align.Right)
            {
                writer.Write(new string(' ', padLength));
            }

            // Reset color, if required
            if (resolved.Color != TableColor.Default && writer == Console.Out)
            {
                Console.ForegroundColor = (ConsoleColor)DefaultColor;
            }

            // Write cell delimiter
            writer.Write(" | ");
        }

        // Return Markdown-compatible separators for each column (indicating alignment)
        private TableCell[] Separators()
        {
            TableCell[] separators = new TableCell[Columns.Count];

            for (int i = 0; i < Columns.Count; ++i)
            {
                char lastTick = (Columns[i].Align == Align.Right ? ':' : '-');
                separators[i] = new TableCell(new string('-', ColumnWidths[i] - 1) + lastTick);
            }

            return separators;
        }
    }

    /// <summary>
    ///  TableCell defines a value to draw in a ConsoleTable: the text, alignment, and color.
    /// </summary>
    public struct TableCell
    {
        public string Text { get; }
        public Align Align { get; }
        public TableColor Color { get; }

        public TableCell(string text, Align align = Align.Default, TableColor color = TableColor.Default)
        {
            Text = text ?? "<null>";
            Align = align;
            Color = color;
        }

        // Return a TableCell with the text, alignment, and color to draw (the column values if the cell specifies 'Default')
        public TableCell Resolve(TableCell column)
        {
            return new TableCell(
                Text,
                (Align != Align.Default ? Align : column.Align),
                (Color != TableColor.Default ? Color : column.Color));
        }

        public static TableCell String(string text)
        {
            return new TableCell(text);
        }

        public static TableCell Size(long sizeInBytes)
        {
            return new TableCell(Format.Size(sizeInBytes), Align.Right);
        }

        public static TableCell Time(double seconds)
        {
            return new TableCell(Format.Time(seconds), Align.Right);
        }

        public static TableCell Percentage(double numerator, double denominator)
        {
            return new TableCell(Format.Percentage(numerator, denominator), Align.Right);
        }

        public static TableCell Rate(long sizeInBytes, double elapsedSeconds)
        {
            return new TableCell(Format.Rate(sizeInBytes, elapsedSeconds), Align.Right);
        }

        public static TableCell Ratio(double current, double baseline, TableColor color = TableColor.Default)
        {
            return new TableCell(Format.Ratio(current, baseline), Align.Right, color);
        }
    }

    /// <summary>
    ///  Enum for ConsoleTable cell alignments (left/right).
    /// </summary>
    public enum Align : byte
    {
        Default = 0,
        Left = 1,
        Right = 2
    }

    /// <summary>
    ///  Enum for ConsoleTable colors; same as ConsoleColor but with a 'Default' value to inherit from Column.
    /// </summary>
    public enum TableColor : byte
    {
        Black = ConsoleColor.Black,
        DarkBlue = ConsoleColor.DarkBlue,
        DarkGreen = ConsoleColor.DarkGreen,
        DarkCyan = ConsoleColor.DarkCyan,
        DarkRed = ConsoleColor.DarkRed,
        DarkMagenta = ConsoleColor.DarkMagenta,
        DarkYellow = ConsoleColor.DarkYellow,
        Gray = ConsoleColor.Gray,
        DarkGray = ConsoleColor.DarkGray,
        Blue = ConsoleColor.Blue,
        Green = ConsoleColor.Green,
        Cyan = ConsoleColor.Cyan,
        Red = ConsoleColor.Red,
        Magenta = ConsoleColor.Magenta,
        Yellow = ConsoleColor.Yellow,
        White = ConsoleColor.White,

        Default = 255
    }
}
