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

        private bool _valueBool;
        private long _valueLong;
        private double _valueDouble;
        private string _valueString;

        public BinaryTreeReader(Stream stream, TreeSerializationSettings settings)
        {
            _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: settings.LeaveStreamOpen);
            _settings = settings;
            
            // Readers are required to read the first token immediately, so reading single values directly works
            // (All methods don't have to Read if the TokenType is still None.)
            Read();
        }

        public bool Read()
        {
            if (_reader.BaseStream.Position == _reader.BaseStream.Length)
            {
                TokenType = TreeToken.None;
                return false;
            }

            TokenType = (TreeToken)_reader.ReadByte();

            switch (TokenType)
            {
                case TreeToken.Boolean:
                    _valueBool = _reader.ReadBoolean();
                    break;
                case TreeToken.Integer:
                    _valueLong = _reader.ReadInt64();
                    break;
                case TreeToken.Float:
                    _valueDouble = _reader.ReadDouble();
                    break;
                case TreeToken.String:
                case TreeToken.PropertyName:
                    _valueString = _reader.ReadString();
                    break;
                case TreeToken.Null:
                    _valueString = null;
                    break;
                default:
                    // Nothing to read or not pre-read
                    break;
            }

            return true;
        }

        public bool ReadAsBoolean()
        {
            return _valueBool;
        }

        public long ReadAsInt64()
        {
            return _valueLong;
        }

        public double ReadAsDouble()
        {
            return _valueDouble;
        }

        public string ReadAsString()
        {
            return _valueString;
        }

        public T[] ReadBlockArray<T>() where T : unmanaged
        {
            return _reader.ReadBlockArray<T>(ref _settings.Buffer);
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
