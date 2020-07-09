// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using BSOA.Generator.Extensions;
using BSOA.Generator.Schema;

namespace BSOA.Generator
{
    /// <summary>
    ///  CodeSection provides code template manipulation, using marker comments to identify
    ///  example segments and insertion points.
    /// </summary>
    public static class CodeSection
    {
        private const RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        private static string CodeSectionRegex(string markerName)
        {
            // Match:
            //  - Line with "       // <MarkerName>"
            //  - Lines between
            //  - Line with "       // </MarkerName>"
            //  - Subsequent whitespace only line, if present

            return $"(^[ \t]*//[ \t]+<{markerName}>[^\n]*\n)(?<Content>.*?)([ \t]*//[ \t]+</{markerName}>[^\n]*\n)";
        }

        public static Dictionary<string, string> AllTemplates(string code)
        {
            Dictionary<string, string> templates = new Dictionary<string, string>();

            foreach (string templateName in AllTemplateNames(code))
            {
                templates[templateName] = Extract(code, templateName);
            }

            return templates;
        }

        public static List<string> AllTemplateNames(string code)
        {
            List<string> results = new List<string>();

            foreach (Match match in Regex.Matches(code, $"^[ \t]*//[ \t]+<([^ />]+)>[^\n]*$", Options))
            {
                results.Add(match.Groups[1].Value);
            }

            return results;
        }

        private static Match Match(string code, string markerName)
        {
            Match m = Regex.Match(code, CodeSectionRegex(markerName), Options);
            if (!m.Success) { throw new ArgumentException($"Unable to find Code Marker {markerName} in:\r\n{code}"); }
            return m;
        }

        public static string Extract(string code, string markerName)
        {
            Match m = Match(code, markerName);
            return m.Groups["Content"].Value;
        }

        public static string Replace(string code, string markerName, string replacement)
        {
            return Regex.Replace(code, CodeSectionRegex(markerName), replacement, Options);
        }

        public static string Remove(string code, string markerName)
        {
            return Replace(code, markerName, "");
        }

        public static string Populate(Dictionary<string, string> templates, string templateName, Schema.Column column)
        {
            string template;
            Schema.Column columnInTemplate;

            // Use a category-specific template, if found
            if (templates.TryGetValue($"{column.Category}{templateName}", out template))
            {
                if (!TemplateDefaults.Columns.TryGetValue(column.Category, out columnInTemplate))
                {
                    throw new NotImplementedException($"Populate not implemented with defaults to replace for category {column.Category} ({column.Name})");
                }

                return Populate(template, columnInTemplate, column);
            }

            // Otherwise, use a non-specific template (using the Simple column defaults)
            if (templates.TryGetValue(templateName, out template))
            {
                return Populate(template, TemplateDefaults.Columns[Schema.ColumnTypeCategory.Simple], column);
            }

            throw new NotSupportedException($"Could not find template '{column.Category}{templateName}' or '{templateName}' in collection: ({string.Join(", ", templates.Keys)}");
        }

        public static string Populate(string template, Schema.Column columnInTemplate, Schema.Column column)
        {
            // Populate by replacing default values.
            List<Tuple<string, string>> replacements = new List<Tuple<string, string>>();

            if (columnInTemplate.UnderlyingType != null)
            {
                replacements.Add(new Tuple<string, string>(columnInTemplate.UnderlyingType, column.UnderlyingType));
            }

            if (columnInTemplate.Default != null)
            {
                replacements.Add(new Tuple<string, string>(columnInTemplate.Default, column.Default ?? "default"));
            }

            if (columnInTemplate.ReferencedTableName != null)
            {
                replacements.Add(new Tuple<string, string>(columnInTemplate.ReferencedTableName, column.ReferencedTableName));
            }

            replacements.Add(new Tuple<string, string>(columnInTemplate.Type, column.Type));
            replacements.Add(new Tuple<string, string>(columnInTemplate.Type.ToPascalCase(), column.Type.ToPascalCase()));
            replacements.Add(new Tuple<string, string>(columnInTemplate.Name, column.Name));
            replacements.Add(new Tuple<string, string>(columnInTemplate.Name.ToCamelCase(), column.Name.ToCamelCase()));

            // Sort by length descending
            replacements.Sort((left, right) => -left.Item1.Length.CompareTo(right.Item1.Length));

            // Turn template into a format string
            string escaped = template.Replace("{", "{{").Replace("}", "}}");
            for (int i = 0; i < replacements.Count; ++i)
            {
                escaped = escaped.Replace(replacements[i].Item1, $"{{{i}}}");
            }

            string populated = string.Format(escaped, replacements.Select(tuple => tuple.Item2).ToArray());

            return populated;
        }
    }
}
