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
            samples.Add(sample);
            samples.Add(sample2);

            SampleCollections<TreeSample> samplesRoundTripped = TreeSerializable.RoundTripJson(samples, () => new SampleCollections<TreeSample>());
            samples.AssertEqual(samplesRoundTripped);
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
            public Dictionary<string, T> StringDictionary { get; set; }
            public Dictionary<int, T> IntDictionary { get; set; }

            public SampleCollections()
            {
                List = new List<T>();
                StringDictionary = new Dictionary<string, T>();
                IntDictionary = new Dictionary<int, T>();
            }

            public void Add(T item)
            {
                int index = List.Count;
                
                List.Add(item);
                StringDictionary[index.ToString()] = item;
                IntDictionary[index] = item;
            }

            public void AssertEqual(SampleCollections<T> other)
            {
                Assert.Equal(this.List, other.List);
                Assert.Equal(this.StringDictionary, other.StringDictionary);
                Assert.Equal(this.IntDictionary, other.IntDictionary);
            }

            private static Dictionary<string, Setter<SampleCollections<T>>> setters = new Dictionary<string, Setter<SampleCollections<T>>>()
            {
                [nameof(List)] = (r, me) => r.ReadList(() => new T(), me.List),
                [nameof(StringDictionary)] = (r, me) => r.ReadDictionary(() => new T(), me.StringDictionary),
                [nameof(IntDictionary)] = (r, me) => r.ReadDictionary(() => new T(), me.IntDictionary)
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

                writer.WritePropertyName(nameof(StringDictionary));
                writer.WriteDictionary(StringDictionary);

                writer.WritePropertyName(nameof(IntDictionary));
                writer.WriteDictionary(IntDictionary);

                writer.WriteEndObject();
            }
        }
    }
}
