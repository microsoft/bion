// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using BSOA.IO;

using Newtonsoft.Json;

namespace BSOA.Test.Components
{
    [JsonConverter(typeof(SampleConverter))]
    public class Sample : ITreeSerializable
    {
        public bool IsActive { get; set; }
        public short Age { get; set; }
        public int Count { get; set; }
        public long Position { get; set; }
        public byte[] Data { get; set; }

        public override bool Equals(object o)
        {
            Sample other = o as Sample;
            if (other == null) { return false; }

            return this.IsActive == other.IsActive
                && this.Age == other.Age
                && this.Count == other.Count
                && this.Position == other.Position;
        }

        public override int GetHashCode()
        {
            return this.IsActive.GetHashCode() ^ this.Age.GetHashCode() ^ this.Count.GetHashCode() ^ this.Position.GetHashCode() ^ this.Data.GetHashCode();
        }

        public void Trim()
        { }

        public void Clear()
        {
            IsActive = false;
            Age = 0;
            Count = 0;
            Position = 0;
            Data = null;
        }

        private static Dictionary<string, Setter<Sample>> setters => new Dictionary<string, Setter<Sample>>()
        {
            [nameof(IsActive)] = (r, me) => me.IsActive = r.ReadAsBoolean(),
            [nameof(Age)] = (r, me) => me.Age = r.ReadAsInt16(),
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [nameof(Position)] = (r, me) => me.Position = r.ReadAsInt64(),
            [nameof(Data)] = (r, me) => me.Data = r.ReadBlockArray<byte>()
        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject<Sample>(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(nameof(IsActive), IsActive);
            writer.Write(nameof(Age), Age);
            writer.Write(nameof(Count), Count);
            writer.Write(nameof(Position), Position);
            writer.WriteBlockArray(nameof(Data), Data);
            writer.WriteEndObject();
        }
    }

    public class SampleConverter : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Sample>> setters => new Dictionary<string, Action<JsonReader, Sample>>()
        {
            [nameof(Sample.IsActive)] = (r, me) => me.IsActive = r.ReadAsBoolean().Value,
            [nameof(Sample.Age)] = (r, me) => me.Age = (short)r.ReadAsInt32().Value,
            [nameof(Sample.Count)] = (r, me) => me.Count = r.ReadAsInt32().Value,
            [nameof(Sample.Position)] = (r, me) => me.Position = r.ReadAsInt32().Value,
            [nameof(Sample.Data)] = (r, me) => me.Data = r.ReadAsBytes()
        };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Sample));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Sample value = new Sample();

            reader.Expect(JsonToken.StartObject);
            reader.Read();

            while(reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = (string)reader.Value;
                
                //reader.Read();  // ReadAsXXX advances to next token

                if(!setters.TryGetValue(propertyName, out var setter))
                {
                    throw new IOException($"{nameof(SampleConverter)} encountered unexpected property name parsing {nameof(Sample)}.  Read \"{propertyName}\", expected one of \"{String.Join("; ", setters.Keys)}\"at {reader.Position()}");
                }

                setter(reader, value);
                reader.Read();   // Must advance to next PropertyName or EndObject
            }

            reader.Expect(JsonToken.EndObject);
            //reader.Read();

            return value;
        }

        public override void WriteJson(JsonWriter writer, object v, JsonSerializer serializer)
        {
            Sample value = (Sample)v;

            writer.WriteStartObject();

            writer.WritePropertyName(nameof(value.IsActive));
            writer.WriteValue(value.IsActive);

            writer.WritePropertyName(nameof(value.Age));
            writer.WriteValue(value.Age);

            writer.WritePropertyName(nameof(value.Count));
            writer.WriteValue(value.Count);

            writer.WritePropertyName(nameof(value.Position));
            writer.WriteValue(value.Position);

            if (value.Data != null)
            {
                writer.WritePropertyName(nameof(value.Data));
                writer.WriteStartArray();

                for(int i = 0; i < value.Data.Length; ++i)
                {
                    writer.WriteValue(value.Data[i]);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndObject();
        }
    }

    public static class JsonReaderExtensions
    {
        public static void Expect(this JsonReader reader, JsonToken token)
        {
            if (reader.TokenType != token)
            {
                throw new IOException($"Expected {token}, got {reader.TokenType} at {reader.Position()}");
            }
        }

        public static string Position(this JsonReader reader)
        {
            JsonTextReader jtr = reader as JsonTextReader;
            return (jtr == null ? "<unknown>" : $"({jtr.LineNumber:n0}, {jtr.LinePosition:n0})");
        }
    }
}
