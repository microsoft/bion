// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Microsoft.CodeAnalysis.Sarif
{
    public partial class ReportingDescriptor
    {
        public string Format(string messageId, IEnumerable<string> arguments)
        {
            return string.Format(CultureInfo.CurrentCulture, this.MessageStrings[messageId].Text, arguments.ToArray());
        }

        public bool ShouldSerializeDeprecatedIds()
        {
            return this.DeprecatedIds.HasAtLeastOneNonNullValue();
        }

        public bool ShouldSerializeDeprecatedGuids()
        {
            return this.DeprecatedGuids.HasAtLeastOneNonNullValue();
        }

        public bool ShouldSerializeDeprecatedNames()
        {
            return this.DeprecatedNames.HasAtLeastOneNonNullValue();
        }

        public bool ShouldSerializeRelationships()
        {
            return this.Relationships.HasAtLeastOneNonDefaultValue(ReportingDescriptorRelationship.ValueComparer);
        }

        public bool ShouldSerializeDefaultConfiguration()
        {
            return this.DefaultConfiguration != null && !this.DefaultConfiguration.ValueEquals(ReportingConfiguration.Empty);
        }
    }
}
