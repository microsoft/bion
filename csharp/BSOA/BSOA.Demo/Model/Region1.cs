using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace ScaleDemo
{
    /// <summary>
    ///  Region1 is a copy of the Sarif SDK Region class fields only, so they're easy to see. =)
    /// </summary>
    public class Region1
    {
        // Size: Region ref (8) + Region overhead (16) + fields (32 + 40) = 96 bytes.
        // In SDK, TagsCollection is constructed, adding another 24 bytes.
        // With List<Region>, total is ~120 MB for 1M

        // 8 ints = 32 bytes
        public int StartLine { get; set; }
        public int StartColumn { get; set; }
        public int EndLine { get; set; }
        public int EndColumn { get; set; }
        public int ByteOffset { get; set; }
        public int ByteLength { get; set; }
        public int CharOffset { get; set; }
        public int CharLength { get; set; }

        // 5 references = 40 bytes
        public ArtifactContent Snippet { get; set; }
        public Message Message { get; set; }
        public string SourceLanguage { get; set; }
        public TagsCollection Tags { get; set; }
        public IDictionary<string, SerializedPropertyInfo> Properties { get; set; }
    }
}
