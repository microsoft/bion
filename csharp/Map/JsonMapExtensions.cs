using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.Sarif.Map;
using Newtonsoft.Json;

namespace Map
{
    public static class JsonMapExtensions
    {
        public static JsonMapNode FindByPath(this JsonMapNode node, string jsonPath, bool throwIfNotFound = true)
        {
            return FindByPath(node, SplitJsonPath(jsonPath), throwIfNotFound);
        }

        public static JsonMapNode FindByPath(this JsonMapNode node, IReadOnlyList<string> jsonPath, bool throwIfNotFound = true)
        {
            JsonMapNode current = node;

            for (int i = 0; i < jsonPath.Count; ++i)
            {
                if (current.Nodes == null)
                {
                    if (throwIfNotFound)
                    {
                        throw new ArgumentException($"Under \"{BuildJsonPath(jsonPath, i - 1)}\", no nodes were included in map to find \"{jsonPath[i]}\"");
                    }

                    return null;
                }

                if (!current.Nodes.TryGetValue(jsonPath[i], out JsonMapNode child))
                {
                    if (throwIfNotFound)
                    {
                        throw new ArgumentException($"Under \"{BuildJsonPath(jsonPath, i - 1)}\", node \"{jsonPath[i]}\" was not found in map. Available Nodes: \r\n{String.Join("\r\n", current.Nodes.Keys)}");
                    }

                    return null;
                }

                current = child;
            }

            return current;
        }

        public static List<string> SplitJsonPath(string path)
        {
            List<string> parts = new List<string>();

            int currentPartStart = 0;
            int i = 0;
            while(i < path.Length)
            {
                if (path[i] == '.')
                {
                    if (currentPartStart != i)
                    {
                        parts.Add(path.Substring(currentPartStart, i - currentPartStart));
                    }

                    i++;
                    currentPartStart = i;
                }
                else if (path[i] == '[')
                {
                    if (currentPartStart != i)
                    {
                        parts.Add(path.Substring(currentPartStart, i - currentPartStart));
                    }

                    currentPartStart = i + 1;

                    // Open Brace, whitespace?
                    Require(path, '[', ref i);
                    ParseWhitespace(path, ref i);

                    if(path[i] == '\'' || path[i] == '"')
                    {
                        parts.Add(ParseString(path, ref i));
                    }
                    else
                    {
                        int closeBrace = path.IndexOf(']', i);
                        if (closeBrace == -1) { throw new ArgumentException($"Unclosed brace in path \"{path}\" at {i}."); }

                        parts.Add(path.Substring(i, closeBrace - i));
                        i = closeBrace;
                    }

                    // Whitespace?, Close Brace
                    ParseWhitespace(path, ref i);
                    Require(path, ']', ref i);

                    currentPartStart = i;
                }
                else
                {
                    i++;
                }
            }

            if (currentPartStart < path.Length)
            {
                parts.Add(path.Substring(currentPartStart, path.Length - currentPartStart));
            }

            return parts;
        }

        private static string ParseString(string path, ref int i)
        {
            char quoteType = path[i];
            if (quoteType != '\'' && quoteType != '"') { throw new InvalidOperationException($"ParseString called when not on a quote."); }

            int startQuote = i;
            int endQuote = i;

            // Find the first unescaped end quote
            while (true)
            {
                endQuote = path.IndexOf(quoteType, endQuote + 1);
                if (endQuote == -1) { throw new ArgumentException($"Unclosed string in path \"{path}\" at {i}."); }

                int backslashes = 0;
                while (path[endQuote - 1 - backslashes] == '\\') { backslashes++; }
                if (backslashes % 2 == 0) { break; }
            }

            // Parse value including quotes to unescape and continue after end quote
            i = endQuote + 1;
            return Unescape(path.Substring(startQuote, endQuote + 1 - startQuote));
        }

        private static void ParseWhitespace(string path, ref int i)
        {
            while (i < path.Length && path[i] == ' ')
            {
                i++;
            }
        }

        private static void Require(string path, char value, ref int i)
        {
            if (i >= path.Length) { throw new ArgumentException($"JsonPath missing required '{value}' at {i}"); }
            if (path[i] != value) { throw new ArgumentException($"JsonPath expected '{value}' at {i} but found '{path[i]}'."); }

            i++;
        }

        private static string BuildJsonPath(IReadOnlyList<string> parts, int lastIncludedIndex)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i <= lastIncludedIndex; ++i)
            {
                if (long.TryParse(parts[i], out long unused))
                {
                    result.Append($"[{parts[i]}]");
                }
                else if (parts[i].IndexOf('.') == -1)
                {
                    if (result.Length > 0) { result.Append("."); }
                    result.Append(parts[i]);
                }
                else
                {
                    result.Append($"[{JsonConvert.ToString(parts[i])}]");
                }
            }

            return result.ToString();
        }

        private static string EscapeForQuotes(string value)
        {
            return JsonConvert.ToString(value);
        }

        private static string Unescape(string value)
        {
            return JsonConvert.DeserializeObject<string>(value);
        }
    }
}
