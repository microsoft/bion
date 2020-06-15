// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using Bion.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test
{
    [TestClass]
    public class BionTokenRoundTripTests
    {
        [TestMethod]
        public void Literals()
        {
            RoundTrip((bool?)null);
            RoundTrip(true);
            RoundTrip(false);
        }

        [TestMethod]
        public void Strings()
        {
            RoundTrip((string)null);
            RoundTrip("");
            RoundTrip("Simple");
            RoundTrip("Normally\t\r\nRequires\"Escaping\"");

            RoundTrip("\u00BC");                // Two Byte  [¼]
            RoundTrip("\u16A0");                // Three Byte  [ᚠ]
            RoundTrip("\U00010908");            // Four Byte  [𐤈] (should look like circle with X inscribed)

            RoundTrip("\u1F3AF");               // 'Direct Hit' (dart board bullseye)
            RoundTrip("\u1F937");               // Person Shrugging
            RoundTrip("\u263A\uFE0F");          // Smiley Face
            RoundTrip("\u2714\uFE0F");          // Check Mark")

            RoundTrip(new string('S', ushort.MaxValue));
        }

        [TestMethod]
        public void Integers()
        {
            RoundTrip(0);       // Inline
            RoundTrip(15);      // Inline limit
            RoundTrip(16);      // Non-Inline min
            RoundTrip(127);     // 1b Max
            RoundTrip(128);     // 2b Min
            RoundTrip(14383);   // 2b Max
            RoundTrip(14384);   // 3b Min

            RoundTrip(short.MaxValue);
            RoundTrip(int.MaxValue);
            RoundTrip(long.MaxValue);

            RoundTrip(-1);      // Negative (1b)
            RoundTrip(-127);    // 1b Max
            RoundTrip(-128);    // 2b Min
            RoundTrip(-14383);  // 2b Max
            RoundTrip(-14384);  // 3b Min

            RoundTrip(short.MinValue);
            RoundTrip(int.MinValue);
            RoundTrip(long.MinValue);
        }

        [TestMethod]
        public void Doubles()
        {
            RoundTrip(0.0);
            RoundTrip(0.3333);
            RoundTrip(0.5);
            RoundTrip(1.0);

            RoundTrip(-0.5);
            RoundTrip(-1.0);
            
            RoundTrip((double)int.MaxValue);

            RoundTrip(float.MinValue);
            RoundTrip(float.MaxValue);
            RoundTrip(double.MinValue);
            RoundTrip(double.MaxValue);
        }

        private static void RoundTrip(Action<BionWriter> write, Action<BionReader> readAndVerify)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Write desired value(s) without closing stream
                using (BionWriter writer = new BionWriter(new BufferedWriter(stream) { CloseStream = false }))
                {
                    write(writer);
                }

                // Track position and seek back
                long length = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                // Read, verify, validate position and no more content
                using (BionReader reader = new BionReader(new BufferedReader(stream) { CloseStream = false }))
                {
                    readAndVerify(reader);

                    Assert.AreEqual(length, reader.BytesRead);
                    Assert.IsFalse(reader.Read());
                }
            }
        }

        private static void RoundTrip(bool? value)
        {
            RoundTrip(
                (writer) =>
                {
                    writer.WriteValue(value);
                },
                (reader) =>
                {
                    Assert.IsTrue(reader.Read());

                    if (value == null)
                    {
                        Assert.AreEqual(BionToken.Null, reader.TokenType);
                    }
                    else if (value == true)
                    {
                        Assert.AreEqual(BionToken.True, reader.TokenType);
                        Assert.AreEqual(true, reader.CurrentBool());
                    }
                    else
                    {
                        Assert.AreEqual(BionToken.False, reader.TokenType);
                        Assert.AreEqual(false, reader.CurrentBool());
                    }
                }
            );
        }

        private static void RoundTrip(long value)
        {
            RoundTrip(
                (writer) =>
                {
                    writer.WriteValue(value);
                },
                (reader) =>
                {
                    Assert.IsTrue(reader.Read());
                    Assert.AreEqual(BionToken.Integer, reader.TokenType);

                    long valueRead = reader.CurrentInteger();
                    Assert.AreEqual(value, valueRead);
                }
            );
        }

        private static void RoundTrip(double value)
        {
            RoundTrip(
                (writer) =>
                {
                    writer.WriteValue(value);
                },
                (reader) =>
                {
                    Assert.IsTrue(reader.Read());
                    Assert.AreEqual(BionToken.Float, reader.TokenType);

                    double valueRead = reader.CurrentFloat();
                    Assert.AreEqual(value, valueRead);
                }
            );
        }

        private static void RoundTrip(string value)
        {
            RoundTrip(
                (writer) =>
                {
                    // Write twice as both string containers
                    writer.WriteStartObject();
                    writer.WritePropertyName(value);
                    writer.WriteValue(value);
                    writer.WriteEndObject();
                },
                (reader) =>
                {
                    // StartObject
                    Assert.IsTrue(reader.Read());

                    // Read and validate in both forms
                    Assert.IsTrue(reader.Read());
                    Assert.AreEqual((value != null ? BionToken.PropertyName : BionToken.Null), reader.TokenType);
                    string readAsPropertyName = reader.CurrentString();
                    Assert.AreEqual(value, readAsPropertyName);

                    Assert.IsTrue(reader.Read());
                    Assert.AreEqual((value != null ? BionToken.String : BionToken.Null), reader.TokenType);
                    string readAsString = reader.CurrentString();
                    Assert.AreEqual(value, readAsString);

                    // EndObject
                    Assert.IsTrue(reader.Read());
                }
            );
        }
    }
}
