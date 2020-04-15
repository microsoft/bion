using BSOA.Extensions;
using System.IO;
using System.Text;

namespace BSOA.IO
{
    public class BinaryTreeReader : ITreeReader
    {
        private BinaryReader _reader;
        private TreeSerializationSettings _settings;

        public TreeToken TokenType { get; private set; }
        public long Position => _reader.BaseStream.Position;

        public BinaryTreeReader(Stream stream, TreeSerializationSettings settings)
        {
            _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: settings.LeaveStreamOpen);
            _settings = settings;
        }

        public bool Read()
        {
            if (_reader.BaseStream.Position == _reader.BaseStream.Length)
            {
                TokenType = TreeToken.None;
                return false;
            }
            else
            {
                TokenType = (TreeToken)_reader.ReadByte();
                return true;
            }
        }

        public bool ReadAsBoolean()
        {
            return _reader.ReadBoolean();
        }

        public string ReadAsString()
        {
            return _reader.ReadString();
        }

        public long ReadAsInt64()
        {
            return _reader.ReadInt64();
        }

        public double ReadAsDouble()
        {
            return _reader.ReadDouble();
        }

        public T[] ReadBlockArray<T>() where T : unmanaged
        {
            return _reader.ReadArray<T>(ref _settings.Buffer);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            _reader?.Dispose();
            _reader = null;
        }
    }
}
