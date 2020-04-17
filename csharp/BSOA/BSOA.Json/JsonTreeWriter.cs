using BSOA.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace BSOA.Json
{
    public class JsonTreeWriter : ITreeWriter
    {
        private JsonWriter _writer;
        private TreeSerializationSettings _settings;

        public JsonTreeWriter(Stream stream, TreeSerializationSettings settings = null)
        {
            settings = settings ?? TreeSerializationSettings.DefaultSettings;

            _writer = new JsonTextWriter(new StreamWriter(stream, Encoding.UTF8, 8 * 1024, leaveOpen: settings.LeaveStreamOpen));
            _writer.Formatting = (settings.Compact ? Formatting.None : Formatting.Indented);
            _settings = settings;
        }

        public void WriteStartObject()
        {
            _writer.WriteStartObject();
        }

        public void WriteEndObject()
        {
            _writer.WriteEndObject();
        }

        public void WriteStartArray()
        {
            _writer.WriteStartArray();
        }

        public void WriteEndArray()
        {
            _writer.WriteEndArray();
        }

        public void WriteNull()
        {
            _writer.WriteNull();
        }

        public void WritePropertyName(string name)
        {
            _writer.WritePropertyName(name);
        }

        public void WriteValue(bool value)
        {
            _writer.WriteValue(value);
        }

        public void WriteValue(string value)
        {
            _writer.WriteValue(value);
        }

        public void WriteValue(long value)
        {
            _writer.WriteValue(value);
        }

        public void WriteValue(double value)
        {
            _writer.WriteValue(value);
        }

        public void WriteBlockArray<T>(T[] array, int index = 0, int count = -1) where T : unmanaged
        {
            if (count < 0) { count = array?.Length - index ?? 0; }

            _writer.WriteStartArray();
            _writer.WriteValue(count);

            for (int i = index; i < index + count; ++i)
            {
                _writer.WriteValue(array[i]);
            }

            _writer.WriteEndArray();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            ((IDisposable)_writer)?.Dispose();
            _writer = null;
        }
    }
}
