using BSOA.Generator.Extensions;
using BSOA.Generator.Generation;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

            // Replace type, default, table name first to avoid string.Replace errors 
            // if the column name or type is contained within those values.
            string populated = template;

            if (columnInTemplate.UnderlyingType != null)
            {
                populated = populated.Replace(columnInTemplate.UnderlyingType, column.UnderlyingType);
            }

            if (columnInTemplate.Default != null)
            {
                populated = populated.Replace(columnInTemplate.Default, column.Default ?? "");
            }

            if (columnInTemplate.ReferencedTableName != null)
            {
                populated = populated.Replace(columnInTemplate.ReferencedTableName, column.ReferencedTableName);
            }

            populated = populated
                .Replace(columnInTemplate.Type, column.Type)
                .Replace(columnInTemplate.Name, column.Name)
                .Replace(columnInTemplate.Name.ToCamelCase(), column.Name.ToCamelCase());

            return populated;
        }

        public static string MakeReplacements(string code, Dictionary<string, string> replacements)
        {
            if (replacements?.Count > 0)
            {
                foreach (KeyValuePair<string, string> replacement in replacements)
                {
                    code = Regex.Replace(code, replacement.Key, replacement.Value, Options);
                }
            }

            return code;
        }
    }
}
