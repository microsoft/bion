using BSOA.IO;
using BSOA.Json;
using BSOA.Test.Components;
using System;
using System.IO;
using Xunit;

namespace BSOA.Test.Components
{
    public enum TreeFormat
    {
        Binary = 0,
        Json = 1
    }

    public class TreeSerializable
    {
        public static ITreeReader Reader(TreeFormat format, Stream stream, TreeSerializationSettings settings)
        {
            switch(format)
            {
                case TreeFormat.Binary:
                    return new BinaryTreeReader(stream, settings);
                case TreeFormat.Json:
                    return new JsonTreeReader(stream, settings);
                default:
                    throw new NotImplementedException($"Reader doesn't know how to build for '{format}'.");
            }
        }

        public static ITreeWriter Writer(TreeFormat format, Stream stream, TreeSerializationSettings settings)
        {
            switch (format)
            {
                case TreeFormat.Binary:
                    return new BinaryTreeWriter(stream, settings);
                case TreeFormat.Json:
                    return new JsonTreeWriter(stream, settings);
                default:
                    throw new NotImplementedException($"Writer doesn't know how to build for '{format}'.");
            }
        }

        public static void Basics(TreeFormat format)
        {
            Random r = new Random();

            // Test serialization of each primitive value type (bool, string, long, double)
            Sample sample = new Sample(new Random());
            sample.AssertEqual(RoundTrip<Sample>(sample, format));

            // Test serialization of containers (read must leave last token of nested items so loop finds next property name properly)
            SingleContainer<Sample> container = new SingleContainer<Sample>(sample);
            container.AssertEqual(RoundTrip<SingleContainer<Sample>>(container, format));

            // Test serialization of all supported primitive array types
            RoundTripArray(new byte[] { 0, 4, 16 }, format);
            RoundTripArray(new char[] { 'S', 'o', 'A' }, format);
            RoundTripArray(new sbyte[] { 0, 4, 16 }, format);
            RoundTripArray(new ushort[] { 0, 4, 16 }, format);
            RoundTripArray(new short[] { 0, 4, 16 }, format);
            RoundTripArray(new uint[] { 0, 4, 16 }, format);
            RoundTripArray(new int[] { 0, 4, 16 }, format);
            RoundTripArray(new ulong[] { 0, 4, 16 }, format);
            RoundTripArray(new long[] { 0, 4, 16 }, format);
            RoundTripArray(new float[] { 0.0f, 100.5f, -2.05f }, format);
            RoundTripArray(new double[] { 0.0f, 100.5f, -2.05f }, format);

            // Verify exception on (expected) unsupported type
            Assert.Throws<NotSupportedException>(() => RoundTripArray(new decimal[] { 0.01M, 0.02M }, format));

            // Null/Empty array handling (currently expected to come back as empty array)
            RoundTripArray<byte>(null, format);
            RoundTripArray<byte>(new byte[] { }, format);

            // Test double Dispose handled correctly, 'Compact == false' works, 'LeaveStreamOpen' respected
            container.AssertEqual(RoundTrip(container, format, testDoubleDispose: true));
            container.AssertEqual(RoundTrip(container, format, new TreeSerializationSettings() { LeaveStreamOpen = true }));
            container.AssertEqual(RoundTrip(container, format, new TreeSerializationSettings() { Compact = false }));

            // Test null string handling
            sample.Name = null;
            sample.AssertEqual(RoundTrip(sample, format));

            // Test settings defaulting
            sample.AssertEqual(RoundTrip_NullSettings(sample, format));

            // Test serialization exceptions
            using (MemoryStream stream = new MemoryStream())
            {
                TreeSerializationSettings settings = new TreeSerializationSettings() { LeaveStreamOpen = true };

                using (ITreeWriter writer = Writer(format, stream, settings))
                {
                    sample.Write(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (ITreeReader reader = Reader(format, stream, settings))
                {
                    // Test Expect failure (should not be 'None' when file just opened
                    Assert.NotEqual(TreeToken.None, reader.TokenType);
                    Assert.Throws<IOException>(() => reader.Expect(TreeToken.None));

                    // Test reading back as wrong type (exception from ReadObject unexpected property name)
                    Assert.Throws<IOException>(() => new SingleContainer<Sample>().Read(reader));
                }

                // Read tokens individually and verify 'None' returned at end
                stream.Seek(0, SeekOrigin.Begin);
                using (ITreeReader reader = Reader(format, stream, settings))
                {
                    while (reader.Read()) { };
                    Assert.Equal(TreeToken.None, reader.TokenType);
                }
            }
        }

        public static void RoundTripArray<T>(T[] sample, TreeFormat format) where T : unmanaged
        {
            // Wrap primitive array in a contained implementing ITreeSerializable
            // and calling WriteBlockArray / ReadBlockArray
            ArrayContainer<T> arrayContainer = new ArrayContainer<T>(sample);

            // Verify roundtrip reconstructs array properly
            ArrayContainer<T> roundTripped = RoundTrip(arrayContainer, format);
            arrayContainer.AssertEqual(roundTripped);
        }

        // Reference RoundTrip implementation - no extra verification
        private static T RoundTrip_Basic<T>(T value, Func<T> buildT, TreeFormat format) where T : ITreeSerializable
        {
            TreeSerializationSettings settings = new TreeSerializationSettings() { LeaveStreamOpen = true };

            using (MemoryStream stream = new MemoryStream())
            {
                using (ITreeWriter writer = Writer(format, stream, settings))
                {
                    value.Write(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (ITreeReader reader = Reader(format, stream, settings))
                {
                    T roundTripped = buildT();
                    roundTripped.Read(reader);
                    return roundTripped;
                }
            }
        }

        // Use default constructor if available
        public static T RoundTrip<T>(
            T value, 
            TreeFormat format,
            TreeSerializationSettings settings = null,
            bool testDoubleDispose = false) where T : ITreeSerializable, new()
        {
            return RoundTrip(value, () => new T(), format, settings, testDoubleDispose);
        }

        // RoundTrip with all verification
        public static T RoundTrip<T>(
            T value,
            Func<T> buildT,
            TreeFormat format,
            TreeSerializationSettings settings = null,
            bool testDoubleDispose = false) where T : ITreeSerializable
        {
            T roundTripped = buildT();

            // Request non-compact stream (debuggability)
            settings ??= new TreeSerializationSettings() { Compact = false };

            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream())
            {
                using (ITreeWriter writer = Writer(format, stream, settings))
                {
                    value.Write(writer);

                    // Debuggability: To see serialized text
                    //string serializedText = StreamString(stream);

                    if (testDoubleDispose) { writer.Dispose(); }
                }

                if (!settings.LeaveStreamOpen)
                {
                    // Verify stream disposed
                    Assert.Throws<ObjectDisposedException>(() => stream.Position);
                }
                else
                {
                    // Verify stream left open if requested
                    long verifyStreamNotDisposed = stream.Position;
                }

                // Get bytes to read back from with reader
                buffer = stream.ToArray();
            }

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (ITreeReader reader = Reader(format, stream, settings))
                {
                    // Ensure Readers pre-read first token by default
                    // Needed to allow single value reading to work without everything having to handle this case everywhere
                    Assert.NotEqual(TreeToken.None, reader.TokenType);

                    roundTripped.Read(reader);
                    
                    // Verify everything read back
                    Assert.Equal(buffer.Length, stream.Position);
                    
                    if (testDoubleDispose) { reader.Dispose(); }
                }

                if (!settings.LeaveStreamOpen)
                {
                    // Verify stream disposed
                    Assert.Throws<ObjectDisposedException>(() => stream.Position);
                }
                else
                {
                    // Verify stream left open if requested
                    long verifyStreamNotDisposed = stream.Position;
                }
            }

            return roundTripped;
        }

        // RoundTrip with all verification
        public static T RoundTrip_NullSettings<T>(T value, TreeFormat format) where T : ITreeSerializable, new()
        {
            T roundTripped = new T();
            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream())
            {
                using (ITreeWriter writer = Writer(format, stream, null))
                {
                    value.Write(writer);
                }

                // Get bytes to read back from with reader
                buffer = stream.ToArray();
            }

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (ITreeReader reader = Reader(format, stream, null))
                {
                    roundTripped.Read(reader);

                    // Verify everything read back
                    Assert.Equal(buffer.Length, stream.Position);
                }
            }

            return roundTripped;
        }

        private static string StreamString(Stream stream)
        {
            long position = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            string text = null;

            using (StreamReader reader = new StreamReader(stream, leaveOpen: true))
            {
                text = reader.ReadToEnd();
            }

            stream.Seek(position, SeekOrigin.Begin);

            return text;
        }
    }
}
