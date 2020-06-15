// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Xunit;

namespace Map.Test
{
    public class JsonMapExtensionsTests
    {
        [Fact]
        public void JsonMapExtensionsTests_Basics()
        {
            ParseAndVerify("", new string[] { });
            ParseAndVerify("Root", new[] { "Root" });
            ParseAndVerify("Root.Subpart", new[] { "Root", "Subpart" });
            ParseAndVerify("Root[5]", new[] { "Root", "5" });
            ParseAndVerify("Root[5].Subpart", new[] { "Root", "5", "Subpart" });
            ParseAndVerify("Root[5][15]", new[] { "Root", "5", "15" });
            ParseAndVerify("[5][15]", new[] { "5", "15" });
            ParseAndVerify("runs[0].results[1593].codeFlows[2].threadFlows[5]", new[] { "runs", "0", "results", "1593", "codeFlows", "2", "threadFlows", "5" });

            ParseAndVerify("runs[0].rules['In[]']", new[] { "runs", "0", "rules", "In[]" });

        }

        private static void ParseAndVerify(string jsonPath, IReadOnlyList<string> expectedParts)
        {
            List<string> actualParts = JsonMapExtensions.SplitJsonPath(jsonPath);
            Assert.Equal(expectedParts, actualParts.ToArray());
        }
    }
}
