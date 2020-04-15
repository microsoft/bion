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
        private BinaryWriter _writer;
        private TreeSerializationSettings _settings;

        public BinaryTreeWriter(Stream stream, TreeSerializationSettings settings)
        {
            _writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: settings.LeaveStreamOpen);
            _settings = settings;
        }

        public void WriteStartObject()
        {
            _writer.Write((byte)TreeToken.StartObject);
        }

        public void WriteEndObject()
        {
            _writer.Write((byte)TreeToken.EndObject);
        }

        public void WriteStartArray()
        {
            _writer.Write((byte)TreeToken.StartArray);
        }

        public void WriteEndArray()
        {
            _writer.Write((byte)TreeToken.EndArray);
        }

        public void WriteNull()
        {
            _writer.Write((byte)TreeToken.Null);
        }

        public void WritePropertyName(string name)
        {
            _writer.Write((byte)TreeToken.PropertyName);
            _writer.Write(name);
        }

        public void WriteValue(bool value)
        {
            _writer.Write((byte)TreeToken.Boolean);
            _writer.Write(value);
        }

        public void WriteValue(string value)
        {
            if (value == null)
            {
                WriteNull();
            }
            else
            {
                _writer.Write((byte)TreeToken.String);
                _writer.Write(value);
            }
        }

        public void WriteValue(long value)
        {
            _writer.Write((byte)TreeToken.Integer);
            _writer.Write(value);
        }

        public void WriteValue(double value)
        {
            _writer.Write((byte)TreeToken.Float);
            _writer.Write(value);
        }

        public void WriteBlockArray<T>(T[] array, int index, int count) where T : unmanaged
        {
            _writer.Write((byte)TreeToken.BlockArray);
            _writer.WriteArray<T>(array, index, count, ref _settings.Buffer);
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
