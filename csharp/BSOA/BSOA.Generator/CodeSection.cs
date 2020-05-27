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

            return $"(^[ \t]*//[ \t]+<{markerName}>[^\n]*\n)(?<Content>.*?)([ \t]*//[ \t]+</{markerName}>[^\n]*\n([ \t\r]*\n)?)";
        }

        public static Dictionary<string, string> AllTemplates(string code)
        {
            Dictionary<string, string> templates = new Dictionary<string, string>();

            foreach(string templateName in AllTemplateNames(code))
            {
                templates[templateName] = Extract(code, templateName);
            }

            return templates;
        }

        public static List<string> AllTemplateNames(string code)
        {
            List<string> results = new List<string>();

            foreach (Match match in Regex.Matches(code, $"^[ \t]*//[ \t]+<([^ />]+)>[^\n]$", Options))
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
    }
}
