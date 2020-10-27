// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BaselineState = BSOA.Test.Model.Log.BaselineState;

namespace BSOA.Benchmarks.NonBsoaModel
{
    public class Result
    {
        public string Guid { get; set; }
        public string RuleId { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; }
        public int StartLine { get; set; }
        public DateTime WhenDetectedUtc { get; set; }
        public BaselineState BaselineState { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public IList<int> Tags { get; set; }
    }
}
