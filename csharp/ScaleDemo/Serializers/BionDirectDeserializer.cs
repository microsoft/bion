using Bion;
using Bion.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScaleDemo
{
    public class BionDirectDeserializer
    {
        private Dictionary<String8, Func<BionReader, Region3, Region3>> structFieldParsers = new Dictionary<String8, Func<BionReader, Region3, Region3>>();
        private Dictionary<String8, Action<BionReader, Region2>> classFieldParsers = new Dictionary<String8, Action<BionReader, Region2>>();

        public BionDirectDeserializer()
        {
            structFieldParsers = new Dictionary<String8, Func<BionReader, Region3, Region3>>();
            classFieldParsers = new Dictionary<String8, Action<BionReader, Region2>>();
            
            BuildStructParsers();
            BuildClassParsers();
        }

        public List<Region3> DeserializeRegion3s(BionReader reader)
        {
            List<Region3> result = new List<Region3>();
            Expect(reader, BionToken.StartArray, "List<Region3>");

            while (reader.TokenType != BionToken.EndArray)
            {
                result.Add(DeserializeRegion3(reader));
            }

            Expect(reader, BionToken.EndArray, "List<Region3>");
            return result;
        }

        public Region3 DeserializeRegion3(BionReader reader)
        {
            Region3 result = new Region3();
            Expect(reader, BionToken.StartObject, "Region3");

            while (reader.TokenType == BionToken.PropertyName)
            {
                String8 propertyName = reader.CurrentString8();

                if (!structFieldParsers.TryGetValue(propertyName, out Func<BionReader, Region3, Region3> parser))
                {
                    throw new NotImplementedException($"Unknown property Region.{propertyName}. Stopping.");
                }

                reader.Read();
                result = parser(reader, result);
                reader.Read();
            }

            Expect(reader, BionToken.EndObject, "Region3");
            return result;
        }

        private void BuildStructParsers()
        {
            byte[] buffer = new byte[100];

            structFieldParsers[String8.CopyExpensive("startLine")] = (reader, region) => { region.StartLine = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("startColumn")] = (reader, region) => { region.StartColumn = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("endLine")] = (reader, region) => { region.EndLine = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("endColumn")] = (reader, region) => { region.EndColumn = (int)reader.CurrentInteger(); return region; };

            structFieldParsers[String8.CopyExpensive("byteLength")] = (reader, region) => { region.ByteLength = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("byteOffset")] = (reader, region) => { region.ByteOffset = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("charLength")] = (reader, region) => { region.CharLength = (int)reader.CurrentInteger(); return region; };
            structFieldParsers[String8.CopyExpensive("charOffset")] = (reader, region) => { region.CharOffset = (int)reader.CurrentInteger(); return region; };
        }

        // -----

        public List<Region2> DeserializeRegion2s(BionReader reader)
        {
            List<Region2> result = new List<Region2>();
            Expect(reader, BionToken.StartArray, "List<Region2>");

            while (reader.TokenType != BionToken.EndArray)
            {
                result.Add(DeserializeRegion2(reader));
            }

            Expect(reader, BionToken.EndArray, "List<Region2>");
            return result;
        }

        public Region2 DeserializeRegion2(BionReader reader)
        {
            Region2 result = new Region2();
            Expect(reader, BionToken.StartObject, "Region2");

            while (reader.TokenType == BionToken.PropertyName)
            {
                String8 propertyName = reader.CurrentString8();

                if (!classFieldParsers.TryGetValue(propertyName, out Action<BionReader, Region2> parser))
                {
                    throw new NotImplementedException($"Unknown property Region.{propertyName}. Stopping.");
                }

                reader.Read();
                parser(reader, result);
                reader.Read();
            }

            Expect(reader, BionToken.EndObject, "Region2");
            return result;
        }

        private void BuildClassParsers()
        {
            classFieldParsers[String8.CopyExpensive("startLine")] = (reader, region) => { region.StartLine = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("startColumn")] = (reader, region) => { region.StartColumn = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("endLine")] = (reader, region) => { region.EndLine = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("endColumn")] = (reader, region) => { region.EndColumn = (int)reader.CurrentInteger(); };

            classFieldParsers[String8.CopyExpensive("byteLength")] = (reader, region) => { region.ByteLength = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("byteOffset")] = (reader, region) => { region.ByteOffset = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("charLength")] = (reader, region) => { region.CharLength = (int)reader.CurrentInteger(); };
            classFieldParsers[String8.CopyExpensive("charOffset")] = (reader, region) => { region.CharOffset = (int)reader.CurrentInteger(); };
        }

        private static void Expect(BionReader reader, BionToken requiredToken, string parseContext)
        {
            if (reader.TokenType == BionToken.None) { reader.Read(); }

            if (reader.TokenType != requiredToken)
            {
                throw new IOException($"Reader found invalid token type '{requiredToken}' while parsing {parseContext}.");
            }

            reader.Read();
        }
    }
}
