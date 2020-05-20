using System;
using System.Text.RegularExpressions;

namespace BSOA.Generator
{
    /// <summary>
    ///  CodeSection provides code template manipulation, using marker comments to identify
    ///  example segments and insertion points.
    /// </summary>
    public static class CodeSection
    {
        private const RegexOptions Options = RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        private static string CodeSectionRegex(string markerName)
        {
            // Match:
            //  - Line with "       // <MarkerName>"
            //  - Lines between
            //  - Line with "       // </MarkerName>"
            //  - Subsequent whitespace only line, if present

            return $"([ \t]*//[ ]+<{markerName}>[^\n]*\n)(?<Content>.*?)([ \t]*//[ ]+</{markerName}>[^\n]*\n([ \t\r]*\n)?)";
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
            Match m = Match(code, markerName);
            return code.Substring(0, m.Index) + replacement + code.Substring(m.Index + m.Length);
        }

        public static string Remove(string code, string markerName)
        {
            return Replace(code, markerName, "");
        }
    }
}
