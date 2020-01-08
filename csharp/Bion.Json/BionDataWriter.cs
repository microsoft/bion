using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public class BionDataWriter : JsonWriter
    {
        private BionWriter _writer;

        public BionDataWriter(BionWriter writer)
        {
            _writer = writer;
        }

        public BionDataWriter(Stream stream)
        {
            _writer = new BionWriter(stream);
        }

        public override void WriteEndArray()
        {
            _writer.WriteEndArray();
        }

        public override void WriteEndObject()
        {
            _writer.WriteEndObject();
        }

        public override void WriteNull()
        {
            _writer.WriteNull();
        }

        public override void WritePropertyName(string name)
        {
            _writer.WritePropertyName(name);
        }

        public override void WriteStartArray()
        {
            _writer.WriteStartArray();
        }

        public override void WriteStartObject()
        {
            _writer.WriteStartObject();
        }

        public override void WriteValue(bool value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(float value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(double value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(sbyte value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(byte value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(short value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(ushort value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(int value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(uint value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(long value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(ulong value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(string value)
        {
            _writer.WriteValue(value);
        }

        public override void WriteValue(DateTime value)
        {
            _writer.WriteValue(value.ToUniversalTime().ToString("u"));
        }

        public override void WriteValue(TimeSpan value)
        {
            _writer.WriteValue(value.ToString());
        }

        public override void Close()
        {
            _writer.Dispose();
            _writer = null;
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        #region Not Implemented or Available
        public override void WriteComment(string text)
        {
            throw new NotImplementedException();
        }

        public override void WriteEndConstructor()
        {
            throw new NotImplementedException();
        }

        public override void WriteStartConstructor(string name)
        {
            throw new NotImplementedException();
        }

        public override void WriteRaw(string json)
        {
            throw new NotImplementedException();
        }

        public override void WriteUndefined()
        {
            throw new NotImplementedException();
        }

        public override void WriteEnd()
        {
            // Nothing to do
        }
        #endregion
    }
}
