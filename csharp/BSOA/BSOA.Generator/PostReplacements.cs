using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BSOA.Generator
{
    public class PostReplacement
    {
        [JsonIgnore]
        public const RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        /// <summary>
        ///  Regex or String to be replaced
        /// </summary>
        public string Replace { get; set; }

        /// <summary>
        ///  Regex or String to insert as replacement; use $n to refer to groups.
        /// </summary>
        public string With { get; set; }

        /// <summary>
        ///  Regex identifying files to replace; null or empty will apply to all files
        /// </summary>
        public string Files { get; set; }

        /// <summary>
        ///  Optional description to explain the Replacement purpose
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Whether the Replace and With values are plain text rather than Regexes
        /// </summary>
        public bool ArePlainText { get; set; }

        public PostReplacement(string replace, string with, string files = null, bool arePlainText = false)
        {
            Replace = replace;
            With = with;
            Files = files;
            ArePlainText = arePlainText;
        }

        public string Apply(string targetFilePath, string code)
        {
            if (String.IsNullOrEmpty(Files) || Regex.IsMatch(targetFilePath, Files, Options))
            {
                if (ArePlainText)
                {
                    code = code.Replace(Replace, With, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    code = Regex.Replace(code, Replace, With, Options);
                }
            }

            return code;
        }
    }

    public class PostReplacements
    {
        public RegexOptions Options { get; set; }
        public List<PostReplacement> Replacements { get; set; }

        public PostReplacements()
        {
            Options = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            Replacements = new List<PostReplacement>();
        }

        public string Apply(string targetFilePath, string code)
        {
            foreach (PostReplacement r in Replacements)
            {
                code = r.Apply(targetFilePath, code);
            }

            return code;
        }
    }
}
