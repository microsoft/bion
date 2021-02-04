// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace BSOA.Benchmarks.NonBsoaModel
{
    public class Rule
    {
        public string Id { get; set; }
        public string Guid { get; set; }
        public Uri HelpUri { get; set; }
        public List<Rule> RelatedRules { get; set; }
    }
}
