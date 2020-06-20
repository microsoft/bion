// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ReportingDescriptor'
    /// </summary>
    internal partial class ReportingDescriptorTable : Table<ReportingDescriptor>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Id;
        internal IColumn<IList<String>> DeprecatedIds;
        internal IColumn<String> Guid;
        internal IColumn<IList<String>> DeprecatedGuids;
        internal IColumn<String> Name;
        internal IColumn<IList<String>> DeprecatedNames;
        internal RefColumn ShortDescription;
        internal RefColumn FullDescription;
        internal IColumn<IDictionary<String, MultiformatMessageString>> MessageStrings;
        internal RefColumn DefaultConfiguration;
        internal IColumn<Uri> HelpUri;
        internal RefColumn Help;
        internal RefListColumn Relationships;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ReportingDescriptorTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<String>(default));
            DeprecatedIds = AddColumn(nameof(DeprecatedIds), ColumnFactory.Build<IList<String>>(default));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<String>(default));
            DeprecatedGuids = AddColumn(nameof(DeprecatedGuids), ColumnFactory.Build<IList<String>>(default));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<String>(default));
            DeprecatedNames = AddColumn(nameof(DeprecatedNames), ColumnFactory.Build<IList<String>>(default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            MessageStrings = AddColumn(nameof(MessageStrings), new DictionaryColumn<String, MultiformatMessageString>(new DistinctColumn<string>(new StringColumn()), new MultiformatMessageStringColumn(this.Database)));
            DefaultConfiguration = AddColumn(nameof(DefaultConfiguration), new RefColumn(nameof(SarifLogDatabase.ReportingConfiguration)));
            HelpUri = AddColumn(nameof(HelpUri), ColumnFactory.Build<Uri>(default));
            Help = AddColumn(nameof(Help), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            Relationships = AddColumn(nameof(Relationships), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorRelationship)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ReportingDescriptor Get(int index)
        {
            return (index == -1 ? null : new ReportingDescriptor(this, index));
        }
    }
}
