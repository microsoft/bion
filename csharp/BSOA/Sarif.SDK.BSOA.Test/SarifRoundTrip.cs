using BSOA.IO;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using System.IO;

using Xunit;

namespace Sarif.SDK.BSOA.Test
{
    public class SarifRoundTrip
    {
        [Fact]
        public void SarifRoundTrip_Basic()
        {
            Run(@"C:\Download\Demo\V2\Inputs\elfie-arriba.sarif");
        }

        internal static void Run(string sarifFilePath)
        {
            // Load via JSON
            SarifLog log = SarifLog.Load(sarifFilePath);

            // Sarif as BSOA
            Directory.CreateDirectory("RoundTrip");
            string fileName = Path.GetFileNameWithoutExtension(sarifFilePath); // Guid.NewGuid().ToString();
            string bsoaFilePath = $"RoundTrip\\{fileName}.bsoa";
            log.Save(bsoaFilePath);

            // Reload from BSOA
            log = SarifLog.Load(bsoaFilePath);

            using (StreamWriter writer = File.CreateText(Path.ChangeExtension(bsoaFilePath, ".bsoa.diag.txt")))
            {
                TreeDiagnostics diagnostics = Diagnostics(bsoaFilePath);
                diagnostics.Write(writer, 3);
            }

            // Save back to JSON
            string jsonFilePath = $"RoundTrip\\{fileName}.sarif";
            log.Save(jsonFilePath);

            VerifyJsonIdentical(sarifFilePath, jsonFilePath);
        }

        internal static void VerifyJsonIdentical(string expectedJsonFilePath, string actualJsonFilePath)
        {
            using (StreamReader eR = File.OpenText(expectedJsonFilePath))
            using (JsonTextReader expected = new JsonTextReader(eR))
            using (StreamReader aR = File.OpenText(actualJsonFilePath))
            using (JsonTextReader actual = new JsonTextReader(aR))
            {
                while (expected.Read())
                {
                    if (!actual.Read())
                    {
                        Assert.True(false, $"At {Path.GetFileName(expectedJsonFilePath)} ({expected.LineNumber}, {expected.LinePosition}), {Path.GetFullPath(actualJsonFilePath)} ran out of content.");
                    }
                    else if (expected.TokenType != actual.TokenType)
                    {
                        Assert.True(false, $"At {Path.GetFileName(expectedJsonFilePath)} ({expected.LineNumber}, {expected.LinePosition}), {Path.GetFullPath(actualJsonFilePath)} token types didn't match.\r\n  Expect:  [{expected.TokenType}]\r\n Actual:  [{actual.TokenType}]");
                    }
                    else if (!object.Equals(expected.Value, actual.Value))
                    {
                        Assert.True(false, $"At {Path.GetFileName(expectedJsonFilePath)} ({expected.LineNumber}, {expected.LinePosition}), {Path.GetFullPath(actualJsonFilePath)} values didn't match.\r\n  Expect: [{expected.Value?.ToString() ?? "null"}]\r\n Actual:  [{actual.Value?.ToString() ?? "null"}]");
                    }
                }

                if (actual.Read())
                {
                    Assert.True(false, $"At {Path.GetFullPath(actualJsonFilePath)} ({actual.LineNumber}, {actual.LinePosition}), content after end of {Path.GetFileName(expectedJsonFilePath)}.");
                }
            }
        }

        internal static TreeDiagnostics Diagnostics(string bsoaFilePath)
        {
            using (Stream stream = File.OpenRead(bsoaFilePath))
            using (BinaryTreeReader btr = new BinaryTreeReader(stream))
            using (TreeDiagnosticsReader reader = new TreeDiagnosticsReader(btr))
            {
                SarifLog log = new SarifLog();
                log.DB.Read(reader);

                return reader.Tree;
            }
        }
    }
}
