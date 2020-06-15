// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Column;

using Xunit;

namespace BSOA.Test
{
    public class UriColumnTests
    {
        [Fact]
        public void UriColumn_Basics()
        {
            Column.Basics<Uri>(
                () => new UriColumn(),
                null,
                new Uri("http://github.com/Microsoft/sarif-sdk/src/Program.cs"),
                (i) => new Uri($"src/{i}.cs", UriKind.Relative)
            );
        }
    }
}
