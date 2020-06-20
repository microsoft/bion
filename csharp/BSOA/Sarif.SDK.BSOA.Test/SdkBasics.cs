// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;

using Microsoft.CodeAnalysis.Sarif;

using Xunit;

namespace Sarif.SDK.BSOA.Test
{
    public class SdkBasics
    {
        [Fact]
        public void EmptySarifLog_Size()
        {
            string bsoaFilePath = "Empty.bsoa";
            string jsonFilePath = "Empty.sarif";
            long size;

            SarifLog log = new SarifLog();

            // Save as BSOA directly; ensure reload works
            log.Save(bsoaFilePath);
            log = SarifLog.Load(bsoaFilePath);

            // Verify under 1 KB
            size = new FileInfo(bsoaFilePath).Length;
            Assert.True(size < 1024);

            // Save as JSON; ensure reload works
            log.Save(jsonFilePath);
            log = SarifLog.Load(jsonFilePath);

            // Verify under 1 KB
            size = new FileInfo(jsonFilePath).Length;
            Assert.True(size < 1024);

            // Save as BSOA after Newtonsoft loading
            log.Save(bsoaFilePath);
            log = SarifLog.Load(bsoaFilePath);

            // Verify under 1 KB
            size = new FileInfo(bsoaFilePath).Length;
            Assert.True(size < 1024);
        }
    }
}
