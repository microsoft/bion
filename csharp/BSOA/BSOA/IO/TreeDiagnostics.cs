// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSOA.IO
{
    /// <summary>
    ///  TreeDiagnostics reports the name, position, length, and subtree of an ITreeSerializable.
    ///  They are produced by TreeDiagnosticsReader to show the size consumed by different tree components.
    /// </summary>
    public class TreeDiagnostics
    {
        public string Name { get; set; }
        public long StartPosition { get; set; }
        public long EndPosition { get; set; }
        public long Length => EndPosition - StartPosition;

        public List<TreeDiagnostics> Children { get; set; }

        public TreeDiagnostics(string name, long startPosition)
        {
            Name = name;
            StartPosition = startPosition;
        }

        public TreeDiagnostics AddChild(TreeDiagnostics child)
        {
            Children = Children ?? new List<TreeDiagnostics>();
            Children.Add(child);
            return child;
        }

        public void Write(TextWriter writer, int logToDepth)
        {
            WriteRecursive(writer, logToDepth, (Length.ToString("n0").Length), 0);
        }

        private void WriteRecursive(TextWriter writer, int logToDepth, int padToLength, int depth)
        {
            writer.WriteLine($"{PadLeft(Length.ToString("n0"), padToLength)}  {new string(' ', 2 * depth)}{Name ?? "[]"}");

            if (Children != null && depth != logToDepth)
            {
                foreach (TreeDiagnostics child in Children)
                {
                    child.WriteRecursive(writer, logToDepth, padToLength, depth + 1);
                }
            }
        }

        private string PadLeft(string value, int length)
        {
            if (value.Length < length)
            {
                return new string(' ', length - value.Length) + value;
            }

            return value;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            using (StringWriter writer = new StringWriter(output))
            {
                Write(writer, -1);
            }

            return output.ToString();
        }
    }
}
