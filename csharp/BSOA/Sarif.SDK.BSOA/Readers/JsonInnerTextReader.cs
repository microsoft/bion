// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif.Readers
{
    /// <summary>
    ///  JsonInnerTextReader is used by deferred collections when deserializing components. 
    ///  It's just a type used as a marker that we're already inside a deferred thing.
    /// </summary>
    internal class JsonInnerTextReader : JsonTextReader
    {
        public JsonInnerTextReader(TextReader reader) : base(reader)
        { }
    }
}
