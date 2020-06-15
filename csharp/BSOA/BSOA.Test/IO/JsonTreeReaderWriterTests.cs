// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test.IO
{
    public class JsonTreeReaderWriterTests
    {
        [Fact]
        public void JsonTreeReaderWriter_Basics()
        {
            // Run ITreeSerializable suite on JsonTreeReader and JsonTreeWriter
            TreeSerializer.Basics(TreeFormat.Json);
        }
    }
}
