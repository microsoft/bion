using BSOA.IO;
using BSOA.Test.Components;
using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test.IO
{
    public class TreeReaderWriterTests
    {
        [Fact]
        public void TreeReaderWriter_ExtendedType_Tests()
        {
            TreeSample sample = new TreeSample(DateTime.UtcNow, Guid.NewGuid());
            TreeSample sample2 = new TreeSample(DateTime.UtcNow.AddDays(-2), Guid.NewGuid());

            // Direct DateTime and Guid serialization
            TreeSample roundTripped = TreeSerializable.RoundTripJson(sample, () => new TreeSample());
            Assert.True(sample.Equals(roundTripped));

            // List and Dictionary serialization built-ins
            SampleCollections<TreeSample> samples = new SampleCollections<TreeSample>();

            samples.List.Add(sample);
            samples.List.Add(sample2);

            samples.Dictionary["One"] = sample;
            samples.Dictionary["Two"] = sample2;

            SampleCollections<TreeSample> samplesRoundTripped = TreeSerializable.RoundTripJson(samples, () => new SampleCollections<TreeSample>());

            Assert.Equal(samples.List, samplesRoundTripped.List);
            Assert.Equal(samples.Dictionary, samplesRoundTripped.Dictionary);
        }

        public class TreeSample : ITreeSerializable
        {
            public DateTime When { get; set; }
            public Guid Guid { get; set; }

            public TreeSample()
            { }

            public TreeSample(DateTime when, Guid guid)
            {
                When = when;
                Guid = guid;
            }

            private static Dictionary<string, Setter<TreeSample>> setters = new Dictionary<string, Setter<TreeSample>>()
            {
                [nameof(When)] = (r, me) => me.When = r.ReadAsDateTime(),
                [nameof(Guid)] = (r, me) => me.Guid = r.ReadAsGuid()
            };

            public void Read(ITreeReader reader)
            {
                reader.ReadObject<TreeSample>(this, setters);
            }

            public void Write(ITreeWriter writer)
            {
                writer.WriteStartObject();
                writer.Write(nameof(When), When);
                writer.Write(nameof(Guid), Guid);
                writer.WriteEndObject();
            }

            public override bool Equals(object obj)
            {
                TreeSample other = obj as TreeSample;
                if (other == null) { return false; }

                return this.When.Equals(other.When) && this.Guid.Equals(other.Guid);
            }

            public override int GetHashCode()
            {
                return this.When.GetHashCode() ^ this.Guid.GetHashCode();
            }
        }

        public class SampleCollections<T> : ITreeSerializable where T : ITreeSerializable, new()
        {
            public List<T> List { get; set; }
            public Dictionary<string, T> Dictionary { get; set; }

            public SampleCollections()
            {
                List = new List<T>();
                Dictionary = new Dictionary<string, T>();
            }

            private static Dictionary<string, Setter<SampleCollections<T>>> setters = new Dictionary<string, Setter<SampleCollections<T>>>()
            {
                [nameof(List)] = (r, me) => r.ReadList(() => new T(), me.List),
                [nameof(Dictionary)] = (r, me) => r.ReadDictionary(() => new T(), me.Dictionary)
            };

            public void Read(ITreeReader reader)
            {
                reader.ReadObject(this, setters);
            }

            public void Write(ITreeWriter writer)
            {
                writer.WriteStartObject();

                writer.WritePropertyName(nameof(List));
                writer.WriteList(List);

                writer.WritePropertyName(nameof(Dictionary));
                writer.WriteDictionary(Dictionary);

                writer.WriteEndObject();
            }
        }
    }
}
