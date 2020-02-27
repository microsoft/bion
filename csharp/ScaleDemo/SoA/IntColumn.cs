using System;
using System.IO;

namespace ScaleDemo.SoA
{
    public class IntColumn : IColumn<int>
    {
        private int _defaultValue;
        private int[] _array;

        public IntColumn(int defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public int Count { get; private set; }

        public int this[int index]
        {
            get
            {
                if (_array == null || _array.Length < index || index < 0)
                {
                    return _defaultValue;
                }

                return _array[index];
            }

            set
            {
                if (_defaultValue == value) { return; }

                if (Count <= index)
                {
                    Count = (index + 1);

                    if (_array == null)
                    {
                        _array = new int[Math.Max(Count, 32)];
                        Array.Fill(_array, _defaultValue);
                    }
                    else if (_array.Length <= index)
                    {
                        int newLength = Math.Max(Count, (_array.Length + _array.Length / 2));
                        int[] newArray = new int[newLength];
                        _array.CopyTo(newArray, 0);
                        Array.Fill(newArray, _defaultValue, _array.Length, newLength - _array.Length);
                        _array = newArray;
                    }
                }

                _array[index] = value;
            }
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            int byteLength = reader.ReadInt32();
            Count = byteLength / 4;
            _array = null;

            if (Count > 0)
            {
                _array = new int[Count];

                if (buffer == null || buffer.Length < byteLength)
                {
                    buffer = new byte[byteLength];
                }

                reader.Read(buffer, 0, byteLength);
                Buffer.BlockCopy(buffer, 0, _array, 0, byteLength);
            }
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            writer.Write(Count * 4);
            if (Count == 0) { return; }

            int byteLength = Count * 4;
            if (buffer == null || buffer.Length < byteLength)
            {
                buffer = new byte[byteLength];
            }

            Buffer.BlockCopy(_array, 0, buffer, 0, byteLength);
            writer.Write(buffer, 0, byteLength);
        }
    }
}
