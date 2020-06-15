// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;
using System.Text;

using BSOA.Extensions;

namespace BSOA.IO
{
    public class BinaryTreeReader : ITreeReader
    {
        private BinaryReader _reader;

        public TreeSerializationSettings Settings { get; }
        public TreeToken TokenType { get; private set; }
        public long Position => _reader.BaseStream.Position;

        private byte _hint;
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
                _reader.SkipBlockArray(_hint);
                _wasBlockArrayRead = true;
            }

            if (_reader.BaseStream.Position == _reader.BaseStream.Length)
            {
                TokenType = TreeToken.None;
                return false;
            }

            byte marker = _reader.ReadByte();
            _hint = (byte)(marker >> 4);
            TokenType = (TreeToken)(marker & 15);

            switch (TokenType)
            {
                case TreeToken.Boolean:
                    _valueBool = (_hint != 0);
                    break;
                case TreeToken.Integer:
                    _valueLong = _reader.ReadLong(_hint);
                    break;
                case TreeToken.Float:
                    _valueDouble = _reader.ReadDouble();
                    break;
                case TreeToken.String:
                case TreeToken.PropertyName:
                    _valueString = _reader.ReadString(_hint, ref Settings.Buffer);
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
            return _reader.ReadBlockArray<T>(_hint, ref Settings.Buffer);
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
