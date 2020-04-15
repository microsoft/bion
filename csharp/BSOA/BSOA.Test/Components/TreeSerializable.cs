using BSOA.IO;
using BSOA.Json;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace BSOA.Test.Components
{
    public class TreeSerializable
    {
        public static void Basics(
            Func<Stream, TreeSerializationSettings, ITreeWriter> buildWriter,
            Func<Stream, TreeSerializationSettings, ITreeReader> buildReader)
        {
            Sample sample = new Sample()
            {
                IsActive = true,
                Age = 27,
                Count = 4137,
                Position = 16 * 1024 * 1024 + 5,
                Data = Enumerable.Range(15, 5).Select((i) => (byte)i).ToArray()
            };

            Sample sampleRoundTripped = RoundTrip<Sample>(sample, () => new Sample(), buildWriter, buildReader);
            sample.AssertEqual(sampleRoundTripped);

            SampleContainer container = new SampleContainer();
            container.Main = sample;

            SampleContainer containerRoundTripped = RoundTrip<SampleContainer>(container, () => new SampleContainer(), buildWriter, buildReader);
            sample.AssertEqual(containerRoundTripped.Main);
        }

        public static T RoundTripBinary<T>(T value, Func<T> builder) where T : ITreeSerializable
        {
            return RoundTrip<T>(
                value,
                builder,
                (stream, settings) => new BinaryTreeWriter(stream, settings),
                (stream, settings) => new BinaryTreeReader(stream, settings)
            );
        }

        public static T RoundTripJson<T>(T value, Func<T> builder) where T : ITreeSerializable
        {
            return RoundTrip<T>(
                value,
                builder,
                (stream, settings) => new JsonTreeWriter(stream, settings),
                (stream, settings) => new JsonTreeReader(stream, settings)
            );
        }

        public static T RoundTrip<T>(T value, 
            Func<T> buildT, 
            Func<Stream, TreeSerializationSettings, ITreeWriter> buildWriter,
            Func<Stream, TreeSerializationSettings, ITreeReader> buildReader) where T : ITreeSerializable
        {
            T roundTripped = buildT();

            // Leave stream open and write non-compact (Json indented)
            TreeSerializationSettings settings = new TreeSerializationSettings(leaveStreamOpen: true) { Compact = false };

            using (MemoryStream stream = new MemoryStream())
            {
                using (ITreeWriter writer = buildWriter(stream, settings))
                {
                    value.Write(writer);
                }

                long position = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (ITreeReader reader = buildReader(stream, settings))
                {
                    roundTripped.Read(reader);
                }

                // Ensure all bytes read back
                Assert.Equal(position, stream.Position);
            }

            return roundTripped;
        }
    }
}
