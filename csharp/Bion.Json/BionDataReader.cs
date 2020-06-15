// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using Newtonsoft.Json;

namespace Bion.Json
{
    public class BionDataReader : JsonReader
    {
        private BionReader _reader;

        public BionDataReader(BionReader reader)
        {
            _reader = reader;
        }

        public BionDataReader(Stream stream)
        {
            _reader = new BionReader(stream);
        }

        public override bool Read()
        {
            bool success = _reader.Read();

            switch (_reader.TokenType)
            {
                case BionToken.EndArray:
                    SetToken(JsonToken.EndArray);
                    break;

                case BionToken.EndObject:
                    SetToken(JsonToken.EndObject);
                    break;

                case BionToken.False:
                    SetToken(JsonToken.Boolean, false);
                    break;

                case BionToken.Float:
                    SetToken(JsonToken.Float, _reader.CurrentFloat());
                    break;

                case BionToken.Integer:
                    SetToken(JsonToken.Integer, (int)_reader.CurrentInteger());
                    break;

                case BionToken.None:
                    SetToken(JsonToken.None);
                    break;

                case BionToken.Null:
                    SetToken(JsonToken.Null, null);
                    break;

                case BionToken.PropertyName:
                    SetToken(JsonToken.PropertyName, _reader.CurrentString());
                    break;

                case BionToken.StartArray:
                    SetToken(JsonToken.StartArray);
                    break;

                case BionToken.StartObject:
                    SetToken(JsonToken.StartObject);
                    break;

                case BionToken.String:
                    SetToken(JsonToken.String, _reader.CurrentString());
                    break;

                case BionToken.True:
                    SetToken(JsonToken.Boolean, true);
                    break;

                default:
                    throw new NotImplementedException($"BionDataReader.Convert not implemented for '{_reader.TokenType}'.");
            }

            return success;
        }

        public override void Close()
        {
            _reader?.Dispose();
            _reader = null;
        }
    }
}
