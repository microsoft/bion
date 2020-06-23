// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Extensions;
using BSOA.IO;

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Xunit;

namespace BSOA.Test.Extensions
{
    public class BinaryReaderWriterExtensionsTests
    {
        [Fact]
        public static void BinaryReaderWriterExtensions_Strings()
        {
            byte[] buffer = null;
            string sample = "Hello";

            RoundTrip((writer) =>
            {
                writer.WriteString(TreeToken.String, null, ref buffer);
                writer.WriteString(TreeToken.String, "", ref buffer);
                writer.WriteString(TreeToken.String, sample, ref buffer);
                writer.WriteString(TreeToken.PropertyName, sample, ref buffer);
            }, (reader) =>
            {
                byte hint;
                string roundTripped;

                // Null strings roundtrip as empty
                hint = (byte)(reader.ReadByte() >> 4);
                roundTripped = reader.ReadString(hint, ref buffer);
                Assert.Equal("", roundTripped);

                hint = (byte)(reader.ReadByte() >> 4);
                roundTripped = reader.ReadString(hint, ref buffer);
                Assert.Equal("", roundTripped);

                hint = (byte)(reader.ReadByte() >> 4);
                roundTripped = reader.ReadString(hint, ref buffer);
                Assert.Equal(sample, roundTripped);

                hint = (byte)(reader.ReadByte() >> 4);
                roundTripped = reader.ReadString(hint, ref buffer);
                Assert.Equal(sample, roundTripped);
            });
        }

        [Fact]
        public static void BinaryReaderWriterExtensions_BlockArrays()
        {
            int[] sample = new int[] { 1, 2, 3 };
            byte[] buffer = null;

            // Verify arrays skip properly
            RoundTrip((writer) =>
            {
                writer.WriteBlockArray<int>(sample, ref buffer);
                writer.Write(true);
            }, (reader) =>
            {
                byte hint = (byte)(reader.ReadByte() >> 4);
                reader.SkipBlockArray(hint);

                bool guardBoolean = reader.ReadBoolean();
                Assert.True(guardBoolean);
            });

            // Verify null arrays roundtrip properly (to empty array)
            RoundTrip((writer) =>
            {
                writer.WriteBlockArray<int>(null, ref buffer);
            }, (reader) =>
            {
                int[] roundTripped = reader.ReadBlockArray<int>(ref buffer);

                // Ensure array is empty (not null)
                Assert.NotNull(roundTripped);
                Assert.Empty(roundTripped);
            });

            // Verify buffers are allocated when null or too small
            RoundTrip((writer) =>
            {
                buffer = null;
                writer.WriteBlockArray<int>(sample, ref buffer);
                Assert.True(buffer?.Length >= 12);

                buffer = new byte[11];
                writer.WriteBlockArray<int>(sample, ref buffer);
                Assert.True(buffer?.Length >= 12);

            }, (reader) =>
            {
                buffer = null;
                int[] roundTripped = reader.ReadBlockArray<int>(ref buffer);
                Assert.True(buffer?.Length >= 12);

                buffer = new byte[11];
                roundTripped = reader.ReadBlockArray<int>(ref buffer);
                Assert.True(buffer?.Length >= 12);

                Assert.Equal(sample, roundTripped);
            });

            // Verify char size reported properly (Marshal.SizeOf reports one because it defaults to marshalling to ASCII)
            Assert.Equal(2, BinaryReaderWriterExtensions.SizeOf(typeof(char)));

            if (!Debugger.IsAttached)
            {
                Assert.Throws<NotSupportedException>(() => BinaryReaderWriterExtensions.SizeOf(typeof(Guid)));

                // WriteMarker bounds verification
                Assert.Throws<ArgumentException>(() => RoundTrip((writer) => writer.WriteMarker(TreeToken.String, 16), null));
                Assert.Throws<ArgumentException>(() => RoundTrip((writer) => writer.WriteMarker(TreeToken.String, -1), null));
            }
        }

        internal static void RoundTrip(Action<BinaryWriter> write, Action<BinaryReader> read)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    write(writer);
                }

                long bytesWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                {
                    read(reader);
                }

                // Ensure everything written is read back
                Assert.Equal(bytesWritten, stream.Position);
            }
        }
    }
}
