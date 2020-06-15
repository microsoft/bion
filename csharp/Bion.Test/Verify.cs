// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.IO;

using Bion.IO;
using Bion.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test
{
    public static class Verify
    {
        public static void JsonEqual(string expectedPath, string actualPath)
        {
            string error = FileComparer.JsonEqual(expectedPath, actualPath);
            if (error != null) { Assert.Fail(error); }
        }

        public static void FilesEqual(string expectedPath, string actualPath)
        {
            string error = FileComparer.BinaryEqual(expectedPath, actualPath);
            if (error != null) { Assert.Fail(error); }
        }

        public static bool SizeRatioUnder(string originalFile, string actualFile, float maximumSizePercentage)
        {
            long originalSize = new FileInfo(originalFile).Length;
            long actualSize = new FileInfo(actualFile).Length;
            Trace.WriteLine($"{originalFile} => {FileLength.MB(originalSize)}");
            Trace.WriteLine($"{actualFile} => {FileLength.MB(actualSize)}");

            bool isSmallEnough = actualSize <= originalSize * maximumSizePercentage;
            Assert.IsTrue(isSmallEnough, $"{actualFile} was {FileLength.MB(actualSize)}, which was over {maximumSizePercentage:p0} of {originalFile} size {FileLength.MB(originalSize)}.");
            return isSmallEnough;
        }
    }
}
