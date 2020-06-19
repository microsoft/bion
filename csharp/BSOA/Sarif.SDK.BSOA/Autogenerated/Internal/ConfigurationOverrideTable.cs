// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ConfigurationOverride'
    /// </summary>
    internal partial class ConfigurationOverrideTable : Table<ConfigurationOverride>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Configuration;
        internal RefColumn Descriptor;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ConfigurationOverrideTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Configuration = AddColumn(nameof(Configuration), new RefColumn(nameof(SarifLogDatabase.ReportingConfiguration)));
            Descriptor = AddColumn(nameof(Descriptor), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ConfigurationOverride Get(int index)
        {
            return (index == -1 ? null : new ConfigurationOverride(this, index));
        }
    }
}
