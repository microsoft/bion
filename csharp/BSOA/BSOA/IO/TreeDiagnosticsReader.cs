using System.Collections.Generic;

namespace BSOA.IO
{
    /// <summary>
    ///  TreeDiagnosticsReader wraps another ITreeReader and provides a TreeDiagnostics
    ///  on the Tree property reporting the size of each container found. It is used to
    ///  identify the size consumed by tree components.
    /// </summary>
    public class TreeDiagnosticsReader : ITreeReader
    {
        private ITreeReader Inner { get; set; }

        private string LastPropertyName { get; set; }
        private Stack<TreeDiagnostics> Path { get; }

        public TreeDiagnostics Tree { get; }

        public TreeSerializationSettings Settings => Inner.Settings;
        public TreeToken TokenType => Inner.TokenType;
        public long Position => Inner.Position;

        public TreeDiagnosticsReader(ITreeReader inner)
        {
            Inner = inner;

            LastPropertyName = "<Database>";
            Path = new Stack<TreeDiagnostics>();
            Tree = Open();
        }

        public bool Read()
        {
            bool result = Inner.Read();

            switch (TokenType)
            {
                case TreeToken.StartArray:
                case TreeToken.StartObject:
                    Open();
                    break;
                case TreeToken.EndArray:
                case TreeToken.EndObject:
                    Close();
                    break;
                case TreeToken.BlockArray:
                    Open();
                    break;
            }

            // Property Name, if it applies, only applies to the very next token
            if (TokenType != TreeToken.PropertyName) { LastPropertyName = null; }

            return result;
        }

        private TreeDiagnostics Open()
        {
            TreeDiagnostics item = new TreeDiagnostics(LastPropertyName, Position);

            if (Path.Count > 0) { Path.Peek().AddChild(item); }
            Path.Push(item);

            return item;
        }

        private void Close()
        {
            TreeDiagnostics item = Path.Pop();
            item.EndPosition = Position;
        }

        public bool ReadAsBoolean()
        {
            return Inner.ReadAsBoolean();
        }

        public string ReadAsString()
        {
            string value = Inner.ReadAsString();
            if (TokenType == TreeToken.PropertyName) { LastPropertyName = value; }
            return value;
        }

        public int ReadAsInt32()
        {
            return Inner.ReadAsInt32();
        }

        public long ReadAsInt64()
        {
            return Inner.ReadAsInt64();
        }

        public double ReadAsDouble()
        {
            return Inner.ReadAsDouble();
        }

        public T[] ReadBlockArray<T>() where T : unmanaged
        {
            // Add array type to diagnostics for array
            TreeDiagnostics diagnostics = Path.Peek();
            diagnostics.Name = $"{typeof(T).Name}[] {diagnostics.Name ?? ""}";

            T[] array = Inner.ReadBlockArray<T>();

            // Close array (no separate end token for BlockArray)
            Close();

            return array;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Inner?.Dispose();
            Inner = null;
        }
    }
}
