using Microsoft.CodeAnalysis.Sarif;

using System.IO;

using Xunit;

namespace Sarif.SDK.BSOA.Test
{
    public class SdkBasics
    {
        //[Fact]
        //public void EmptySarifLog_Size()
        //{
        //    string bsoaFilePath = "Empty.bsoa";
        //    string jsonFilePath = "Empty.sarif";
        //    long size;

        //    SarifLog log = new SarifLog();
            
        //    // Save as BSOA directly; ensure reload works
        //    log.Save(bsoaFilePath);
        //    log = SarifLog.Load(bsoaFilePath);

        //    // Verify under 1 KB
        //    size = new FileInfo(bsoaFilePath).Length;
        //    Assert.True(size < 1024);

        //    // Save as JSON; ensure reload works
        //    log.Save(jsonFilePath);
        //    log = SarifLog.Load(jsonFilePath);

        //    // Verify under 1 KB
        //    size = new FileInfo(jsonFilePath).Length;
        //    Assert.True(size < 1024);

        //    // Save as BSOA after Newtonsoft loading
        //    log.Save(bsoaFilePath);
        //    log = SarifLog.Load(bsoaFilePath);

        //    // Verify under 1 KB
        //    size = new FileInfo(bsoaFilePath).Length;
        //    Assert.True(size < 1024);
        //}
    }
}
