using BSOA.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace BSOA.Json
{
    public class JsonTreeReader : ITreeReader
    {
        private JsonTextReader _reader;

        // Note: TreeToken values chosen to match JsonToken enum
        public TreeSerializationSettings Settings { get; }
        public TreeToken TokenType => (TreeToken)_reader.TokenType;
        public long Position => _reader.LineNumber;

        public JsonTreeReader(Stream stream, TreeSerializationSettings settings = null)
        {
            Settings = settings ?? TreeSerializationSettings.DefaultSettings;

            _reader = new JsonTextReader(new StreamReader(stream, Encoding.UTF8, true, 8 * 1024, leaveOpen: Settings.LeaveStreamOpen));

            // TreeReaders required to read the first token on open, to allow single value reading to work
            Read();
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public bool ReadAsBoolean()
        {
            return (bool)_reader.Value;
        }

        public string ReadAsString()
        {
            return (string)_reader.Value;
        }

        public long ReadAsInt64()
        {
            return (long)_reader.Value;
        }

        public double ReadAsDouble()
        {
            return (double)_reader.Value;
        }

        public T[] ReadBlockArray<T>() where T : unmanaged
        {
            this.Expect(TreeToken.StartArray);

            int count = _reader.ReadAsInt32().Value;
            _reader.Read();

            T[] array = new T[count];

            PrimitiveArrayReader.ReadArray<T>(_reader, array, count);

            this.Expect(TreeToken.EndArray);
            // Leave EndArray token for outer reader to Read() past

            return array;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            ((IDisposable)_reader)?.Dispose();
            _reader = null;
        }

        
    }
}
