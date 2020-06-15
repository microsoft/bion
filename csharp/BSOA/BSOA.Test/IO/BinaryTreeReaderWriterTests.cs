// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test.IO
{
    public class BinaryTreeReaderWriterTests
    {
        [Fact]
        public void BinaryTreeReaderWriter_Basics()
        {
            // Run ITreeSerializable suite on BinaryTreeReader and BinaryTreeWriter
            TreeSerializer.Basics(TreeFormat.Binary);
        }
    }
}
