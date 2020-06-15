// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using Microsoft.CodeAnalysis.Sarif;

namespace ScaleDemo
{
    /// <summary>
    ///  Region2 moves infrequently used properties into an 'Ex' class which is just one null pointer
    ///  when none of those properties are set, reducing RAM use.
    /// </summary>
    //[JsonConverter(typeof(Region2Converter))]
    public class Region2
    {
        // 4 ints = 32 bytes
        public int StartLine { get; set; }
        public int StartColumn { get; set; }
        public int EndLine { get; set; }
        public int EndColumn { get; set; }

        public ArtifactContent Snippet { get; set; }

        // Move rarely used properties to 'Ex' class
        private RegionEx Ex { get; set; }

        // Properties backed by Ex class return defaults when it is null and initialize it on set if needed
        public int ByteOffset
        {
            get => Ex?.ByteOffset ?? -1;
            set => (Ex ??= new RegionEx()).ByteOffset = value;
        }

        public int ByteLength
        {
            get => Ex?.ByteLength ?? -1;
            set => (Ex ??= new RegionEx()).ByteLength = value;
        }

        public int CharOffset
        {
            get => Ex?.CharOffset ?? -1;
            set => (Ex ??= new RegionEx()).CharOffset = value;
        }

        public int CharLength
        {
            get => Ex?.CharLength ?? -1;
            set => (Ex ??= new RegionEx()).CharLength = value;
        }

        public Message Message
        {
            get => Ex?.Message;
            set => (Ex ??= new RegionEx()).Message = value;
        }

        public string SourceLanguage
        {
            get => Ex?.SourceLanguage;
            set => (Ex ??= new RegionEx()).SourceLanguage = value;
        }

        public TagsCollection Tags
        {
            get => Ex?.Tags;
            set => (Ex ??= new RegionEx()).Tags = value;
        }

        public IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => Ex?.Properties;
            set => (Ex ??= new RegionEx()).Properties = value;
        }
    }
}
