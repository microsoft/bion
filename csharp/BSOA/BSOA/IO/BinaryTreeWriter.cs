using BSOA.Extensions;
using System.IO;
using System.Text;

namespace BSOA.IO
{
    /// <summary>
    ///  BinaryTreeWriter is an ITreeWriter on a direct binary format.
    /// </summary>
    public class BinaryTreeWriter : ITreeWriter
    {
        internal const string StartObject = "\u0002"; // STX (start of text)
        internal const string EndObject = "\u0003";   // ETX (end of text)

        private BinaryWriter _writer;
        private TreeSerializationSettings _settings;

        public BinaryTreeWriter(Stream  stream, TreeSerializationSettings settings)
        {
            _writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: settings.LeaveStreamOpen);
            _settings = settings;
        }

        public void WriteStartObject()
        {
            _writer.Write(StartObject);
        }

        public void WriteEndObject()
        {
            _writer.Write(EndObject);
        }
        
        public void Write(string name, string value)
        {
            _writer.Write(name);
            _writer.Write(value);
        }

        public void Write(string name, bool value)
        {
            _writer.Write(name);
            _writer.Write(value);
        }

        public void Write(string name, short value)
        {
            _writer.Write(name);
            _writer.Write(value);
        }

        public void Write(string name, int value)
        {
            _writer.Write(name);
            _writer.Write(value);
        }

        public void Write(string name, long value)
        {
            _writer.Write(name);
            _writer.Write(value);
        }

        public void WriteArray<T>(string name, T[] array, int index, int count) where T : unmanaged
        {
            _writer.Write(name);
            _writer.WriteArray<T>(array, index, count, ref _settings.Buffer);
        }

        public void WriteComponent(string name, ITreeSerializable component)
        {
            _writer.Write(name);
            component.Write(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _writer?.Dispose();
            _writer = null;
        }
    }
}
