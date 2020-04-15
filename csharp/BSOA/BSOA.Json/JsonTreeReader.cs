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
        private TreeSerializationSettings _settings;

        public TreeToken TokenType { get; private set; }
        public long Position => _reader.LineNumber;

        public JsonTreeReader(Stream stream, TreeSerializationSettings settings)
        {
            _reader = new JsonTextReader(new StreamReader(stream, Encoding.UTF8, true, 8 * 1024, leaveOpen: settings.LeaveStreamOpen));
            _settings = settings;
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

            T[] array = new T[count];
            for (int i = 0; i < count; ++i)
            {
                if (!_reader.Read()) { throw new IOException($"ReadBlockArray expected {count:n0} items, but end found trying to read item {i:n0}."); }
                array[i] = (T)_reader.Value;
            }

            _reader.Read();
            
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
