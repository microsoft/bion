using BSOA.IO;
using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test.Components
{
    internal class Sample : ITreeSerializable
    {
        public bool IsActive { get; set; }
        public short Age { get; set; }
        public int Count { get; set; }
        public long Position { get; set; }
        public byte[] Data { get; set; }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(nameof(IsActive), IsActive);
            writer.Write(nameof(Age), Age);
            writer.Write(nameof(Count), Count);
            writer.Write(nameof(Position), Position);
            writer.WriteArray(nameof(Data), Data);
            writer.WriteEndObject();
        }

        public void Read(ITreeReader reader)
        {
            reader.ReadObject<Sample>(this, setters);
        }

        private static Dictionary<string, Setter<Sample>> setters => new Dictionary<string, Setter<Sample>>()
        {
            [nameof(IsActive)] = (r, me) => me.IsActive = r.ReadBoolean(),
            [nameof(Age)] = (r, me) => me.Age = r.ReadInt16(),
            [nameof(Count)] = (r, me) => me.Count = r.ReadInt32(),
            [nameof(Position)] = (r, me) => me.Position = r.ReadInt64(),
            [nameof(Data)] = (r, me) => me.Data = r.ReadArray<byte>()
        };

        public void AssertEqual(Sample other)
        {
            Assert.Equal(this.IsActive, other.IsActive);
            Assert.Equal(this.Age, other.Age);
            Assert.Equal(this.Count, other.Count);
            Assert.Equal(this.Position, other.Position);
            Assert.Equal(this.Data, other.Data);
        }
    }

    internal class SampleContainer : ITreeSerializable
    {
        public Sample Main { get; set; }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteComponent(nameof(Main), this.Main);
            writer.WriteEndObject();
        }

        public void Read(ITreeReader reader)
        {
            reader.ReadObject<SampleContainer>(this, setters);
        }

        private static Dictionary<string, Setter<SampleContainer>> setters => new Dictionary<string, Setter<SampleContainer>>()
        {
            [nameof(Main)] = (r, me) => { me.Main = new Sample(); me.Main.Read(r); },
        };
    }
}
