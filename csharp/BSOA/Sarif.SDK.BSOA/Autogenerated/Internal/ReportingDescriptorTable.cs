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

            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(ReportingDescriptor), nameof(Id), default));
            DeprecatedIds = AddColumn(nameof(DeprecatedIds), database.BuildColumn<IList<String>>(nameof(ReportingDescriptor), nameof(DeprecatedIds), default));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(ReportingDescriptor), nameof(Guid), default));
            DeprecatedGuids = AddColumn(nameof(DeprecatedGuids), database.BuildColumn<IList<String>>(nameof(ReportingDescriptor), nameof(DeprecatedGuids), default));
            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(ReportingDescriptor), nameof(Name), default));
            DeprecatedNames = AddColumn(nameof(DeprecatedNames), database.BuildColumn<IList<String>>(nameof(ReportingDescriptor), nameof(DeprecatedNames), default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            MessageStrings = AddColumn(nameof(MessageStrings), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(ReportingDescriptor), nameof(MessageStrings), default));
            DefaultConfiguration = AddColumn(nameof(DefaultConfiguration), new RefColumn(nameof(SarifLogDatabase.ReportingConfiguration)));
            HelpUri = AddColumn(nameof(HelpUri), database.BuildColumn<Uri>(nameof(ReportingDescriptor), nameof(HelpUri), default));
            Help = AddColumn(nameof(Help), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            Relationships = AddColumn(nameof(Relationships), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorRelationship)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ReportingDescriptor), nameof(Properties), default));
        }

        public override ReportingDescriptor Get(int index)
        {
            return (index == -1 ? null : new ReportingDescriptor(this, index));
        }
    }
}
