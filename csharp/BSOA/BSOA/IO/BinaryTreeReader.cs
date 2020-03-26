using BSOA.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSOA.IO
{
    public class BinaryTreeReader : ITreeReader
    {
        private BinaryReader _reader;
        private TreeSerializationSettings _settings;

        public BinaryTreeReader(Stream stream, TreeSerializationSettings settings)
        {
            _reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: settings.LeaveStreamOpen);
            _settings = settings;
        }

        public T[] ReadArray<T>() where T : unmanaged
        {
            return _reader.ReadArray<T>(ref _settings.Buffer);
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }

        public bool ReadBoolean()
        {
            return _reader.ReadBoolean();
        }

        public short ReadInt16()
        {
            return _reader.ReadInt16();
        }

        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        public long ReadInt64()
        {
            return _reader.ReadInt64();
        }

        public void ReadObject<T>(T instance, Dictionary<string, Action<ITreeReader, T>> setters)
        {
            Expect(BinaryTreeWriter.StartObject);

            string propertyName = _reader.ReadString();
            while (propertyName != BinaryTreeWriter.EndObject)
            {
                if(!setters.TryGetValue(propertyName, out Action<ITreeReader, T> setter))
                {
                    throw new IOException($"BinaryTreeReader encountered unexpected property name parsing {typeof(T).Name}. Read \"{propertyName}\", expected one of \"{String.Join("; ", setters.Keys)}\"at {_reader.BaseStream.Position - (propertyName.Length * 2):n0}");
                }

                setter(this, instance);
                propertyName = _reader.ReadString();
            }
        }

        private void Expect(string expected)
        {
            string marker = _reader.ReadString();

            if (marker != expected)
            {
                throw new IOException($"BinaryTreeReader expected \"{expected}\" but found \"{marker}\" at {_reader.BaseStream.Position - 1:n0}");
            }
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
