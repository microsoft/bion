using BSOA.IO;
using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test.Components
{
    internal class Empty : ITreeSerializable
    {
        private static Dictionary<string, Setter<Empty>> setters = new Dictionary<string, Setter<Empty>>();

        public void Clear()
        { }

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters, throwOnUnknown: false);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
    }

    /// <summary>
    ///  Sample class for all supported single basic types
    /// </summary>
    internal class Sample : ITreeSerializable
    {
        // Basic Types
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public long Position { get; set; }
        public double Age { get; set; }

        // Extended Types
        public short Type { get; set; }
        public int Count { get; set; }
        public DateTime When { get; set; }
        public Guid Guid { get; set; }

        public Sample()
        { }

        public Sample(Random r)
        {
            IsActive = (r.Next(2) != 0);
            Name = r.Next().ToString();
            Position = r.Next();
            Age = r.NextDouble();

            Type = (short)r.Next(0, short.MaxValue);
            Count = r.Next();
            When = DateTime.UtcNow;
            Guid = Guid.NewGuid();
        }

        public void Clear()
        {
            IsActive = false;
            Name = null;
            Position = 0;
            Age = 0;
            Type = 0;
            Count = 0;
            When = default(DateTime);
            Guid = default(Guid);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.Write(nameof(IsActive), IsActive);
            writer.Write(nameof(Name), Name);
            writer.Write(nameof(Position), Position);
            writer.Write(nameof(Age), Age);
            writer.Write(nameof(Type), Type);
            writer.Write(nameof(Count), Count);
            writer.Write(nameof(When), When);
            writer.Write(nameof(Guid), Guid);
            writer.WriteEndObject();
        }

        public void Read(ITreeReader reader)
        {
            reader.ReadObject<Sample>(this, setters);
        }

        private static Dictionary<string, Setter<Sample>> setters => new Dictionary<string, Setter<Sample>>()
        {
            [nameof(IsActive)] = (r, me) => me.IsActive = r.ReadAsBoolean(),
            [nameof(Name)] = (r, me) => me.Name = r.ReadAsString(),
            [nameof(Position)] = (r, me) => me.Position = r.ReadAsInt64(),
            [nameof(Age)] = (r, me) => me.Age = r.ReadAsDouble(),
            [nameof(Type)] = (r, me) => me.Type = r.ReadAsInt16(),
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [nameof(When)] = (r, me) => me.When = r.ReadAsDateTime(),
            [nameof(Guid)] = (r, me) => me.Guid = r.ReadAsGuid()
        };

        public void AssertEqual(Sample other)
        {
            Assert.Equal(this.IsActive, other.IsActive);
            Assert.Equal(this.Name, other.Name);
            Assert.Equal(this.Position, other.Position);
            Assert.Equal(this.Age, other.Age);
            Assert.Equal(this.Type, other.Type);
            Assert.Equal(this.Count, other.Count);
            Assert.Equal(this.When, other.When);
            Assert.Equal(this.Guid, other.Guid);
        }

        public override bool Equals(object obj)
        {
            Sample other = obj as Sample;
            if (other == null) { return false; }

            return this.IsActive == other.IsActive
                && this.Name == other.Name
                && this.Position == other.Position
                && this.Age == other.Age
                && this.Type == other.Type
                && this.Count == other.Count
                && this.When == other.When
                && this.Guid == other.Guid;
        }

        public override int GetHashCode()
        {
            return this.IsActive.GetHashCode()
                ^ this.Name.GetHashCode()
                ^ this.Position.GetHashCode()
                ^ this.Age.GetHashCode()
                ^ this.Type.GetHashCode()
                ^ this.Count.GetHashCode()
                ^ this.When.GetHashCode()
                ^ this.Guid.GetHashCode();
        }
    }

    /// <summary>
    ///  Sample class for all primitive array types
    /// </summary>
    /// <typeparam name="T">Type of Array elements</typeparam>
    internal class ArrayContainer<T> : ITreeSerializable where T : unmanaged
    {
        public T[] Array { get; set; }

        public ArrayContainer()
        { }

        public ArrayContainer(T[] array)
        {
            Array = array;
        }

        public void AssertEqual(ArrayContainer<T> other)
        {
            // NOTE: Null arrays expected to round trip to empty
            Assert.Equal(Array ?? new T[0], other.Array);
        }

        public void Clear()
        {
            Array = null;
        }

        public void Read(ITreeReader reader)
        {
            // Verify classes can serialize a single item directly
            Array = reader.ReadBlockArray<T>();
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteBlockArray(Array);
        }
    }

    /// <summary>
    ///  Sample class for recursive containment of any other ITreeSerializable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SingleContainer<T> : ITreeSerializable where T : ITreeSerializable, new()
    {
        public T Item { get; set; }

        public SingleContainer()
        { }

        public SingleContainer(T item)
        {
            Item = item;
        }

        public void AssertEqual(SingleContainer<T> other)
        {
            Assert.Equal(this.Item, other.Item);
        }

        public void Clear()
        {
            Item = default(T);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteObject(nameof(Item), this.Item);
            writer.WriteEndObject();
        }

        public void Read(ITreeReader reader)
        {
            reader.ReadObject<SingleContainer<T>>(this, setters);
        }

        private static Dictionary<string, Setter<SingleContainer<T>>> setters => new Dictionary<string, Setter<SingleContainer<T>>>()
        {
            [nameof(Item)] = (r, me) => { me.Item = new T(); me.Item.Read(r); },
        };
    }

    /// <summary>
    ///  Sample class for basic collections with direct serialization support.
    ///  (Currently, IList, IDictionary&lt;string, T&gt;, IDictionary&lt;int, T&gt;)
    /// </summary>
    /// <typeparam name="T">Type of item in Collections</typeparam>
    internal class CollectionContainer<T> : ITreeSerializable where T : ITreeSerializable, new()
    {
        public List<T> List { get; set; }
        public Dictionary<string, T> StringDictionary { get; set; }
        public Dictionary<int, T> IntDictionary { get; set; }

        public CollectionContainer()
        {
            Clear();
        }

        public void Clear()
        {
            List = new List<T>();
            StringDictionary = new Dictionary<string, T>();
            IntDictionary = new Dictionary<int, T>();
        }

        public void SetCollectionsNull()
        {
            List = null;
            StringDictionary = null;
            IntDictionary = null;
        }

        public void Add(T item)
        {
            int index = List.Count;

            List.Add(item);
            StringDictionary[index.ToString()] = item;
            IntDictionary[index] = item;
        }

        public void AssertEqual(CollectionContainer<T> other)
        {
            Assert.Equal(this.List, other.List);
            Assert.Equal(this.StringDictionary, other.StringDictionary);
            Assert.Equal(this.IntDictionary, other.IntDictionary);
        }

        private static Dictionary<string, Setter<CollectionContainer<T>>> setters = new Dictionary<string, Setter<CollectionContainer<T>>>()
        {
            [nameof(List)] = (r, me) => me.List = r.ReadList(() => new T()),
            [nameof(StringDictionary)] = (r, me) => me.StringDictionary = r.ReadStringDictionary<T>(),
            [nameof(IntDictionary)] = (r, me) => me.IntDictionary = r.ReadIntDictionary<T>()
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
