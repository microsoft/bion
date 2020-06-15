// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using Microsoft.CodeAnalysis.Sarif;

namespace ScaleDemo
{
    // Put infrequent properties into a single container (which will usually be one null pointer)
    internal class RegionEx
    {
        public int ByteOffset { get; set; }
        public int ByteLength { get; set; }
        public int CharOffset { get; set; }
        public int CharLength { get; set; }

        public Message Message { get; set; }
        public string SourceLanguage { get; set; }

        public TagsCollection Tags { get; set; }
        public IDictionary<string, SerializedPropertyInfo> Properties { get; set; }
    }
}
