using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public static class FileComparer
    {
        public static string JsonEqual(string expectedPath, string actualPath)
        {
            string error = null;

            using (JsonTextReader expectedReader = new JsonTextReader(new StreamReader(expectedPath)))
            using (JsonTextReader actualReader = new JsonTextReader(new StreamReader(actualPath)))
            {
                while (expectedReader.Read())
                {
                    if (!actualReader.Read())
                    {
                        return $"{expectedPath} reports more content @({expectedReader.LineNumber}, {expectedReader.LinePosition}) when {actualPath} is done.";
                    }

                    error = JsonEqual(expectedReader.TokenType, actualReader.TokenType, "Token Type", expectedReader, actualReader);
                    if (error != null) { return error; }

                    error = JsonEqual(expectedReader.Value, actualReader.Value, "Value", expectedReader, actualReader);
                    if (error != null) { return error; }
                }

                if (actualReader.Read())
                {
                    return $"{actualPath} reports more content @({actualReader.LineNumber}, {actualReader.LinePosition}) when {expectedPath} is done.";
                }
            }

            return error;
        }

        private static string JsonEqual(object expected, object actual, string category, JsonTextReader expectedReader, JsonTextReader actualReader)
        {
            if (expected == null && actual == null) { return null; }
            if (expected != null && expected.Equals(actual)) { return null; }

            return $"{category} different\r\nexpect: {expected ?? "<null>"} @({expectedReader.LineNumber}, {expectedReader.LinePosition})\r\nactual: {actual ?? "<null>"} @({actualReader.LineNumber}, {actualReader.LinePosition})";
        }

        public static string BinaryEqual(string expectedPath, string actualPath)
        {
            Span<byte> expected = new byte[64 * 1024];
            Span<byte> actual = new byte[64 * 1024];

            long position = 0;
            using (FileStream expectedReader = File.OpenRead(expectedPath))
            using (FileStream actualReader = File.OpenRead(actualPath))
            {
                while (true)
                {
                    int expectedLength = expectedReader.Read(expected);
                    int actualLength = actualReader.Read(actual);
                    if (expectedLength != actualLength)
                    {
                        return $"@{position:n0}, Read length\r\n expected: {expectedLength:n0} bytes from {expectedPath}\r\nactual: {actualLength:n0}bytes from {actualPath}.";
                    }

                    for (int i = 0; i < expectedLength; ++i)
                    {
                        if (expected[i] != actual[i])
                        {
                            return $"@{position + i:n0};\r\nexpect: {expected[i]} from {expectedPath},\r\nactual: {actual[i]} from {actualPath}.";
                        }
                    }

                    position += expectedLength;
                    if (expectedLength < expected.Length) break;
                }
            }

            return null;
        }
    }
}
