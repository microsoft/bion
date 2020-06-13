using BSOA.Extensions;
using System.IO;
using System.Text;

namespace BSOA.IO
{
    public class BinaryTreeReader : ITreeReader
    {
        private BinaryReader _reader;

        public TreeSerializationSettings Settings { get; }
        public TreeToken TokenType { get; private set; }
        public long Position => _reader.BaseStream.Position;

        private bool _valueBool;
        private long _valueLong;
        private double _valueDouble;
        private string _valueString;

        private bool _wasBlockArrayRead;

        public BinaryTreeReader(Stream stream, TreeSerializationSettings settings = null)
        {
            Settings = settings ?? TreeSerializationSettings.DefaultSettings;

            _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: Settings.LeaveStreamOpen);
            _wasBlockArrayRead = true;

            // Readers are required to read the first token immediately, so reading single values directly works
            // (All methods don't have to Read if the TokenType is still None.)
            Read();
        }

        public bool Read()
        {
            if (_wasBlockArrayRead == false && TokenType == TreeToken.BlockArray)
            {
                _reader.SkipBlockArray();
                _wasBlockArrayRead = true;
            }

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
                    _valueLong = _reader.ReadInt32();
                    break;
                case TreeToken.Long:
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
                case TreeToken.BlockArray:
                    _wasBlockArrayRead = false;
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

        public int ReadAsInt32()
        {
            return (int)_valueLong;
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
            _wasBlockArrayRead = true;
            return _reader.ReadBlockArray<T>(ref Settings.Buffer);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            Settings.Buffer = null;

            _reader?.Dispose();
            _reader = null;
        }
    }
}
