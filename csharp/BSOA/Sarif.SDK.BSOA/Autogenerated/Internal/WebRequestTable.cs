// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'WebRequest'
    /// </summary>
    internal partial class WebRequestTable : Table<WebRequest>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Index;
        internal IColumn<String> Protocol;
        internal IColumn<String> Version;
        internal IColumn<String> Target;
        internal IColumn<String> Method;
        internal IColumn<IDictionary<String, String>> Headers;
        internal IColumn<IDictionary<String, String>> Parameters;
        internal RefColumn Body;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal WebRequestTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(WebRequest), nameof(Index), -1));
            Protocol = AddColumn(nameof(Protocol), database.BuildColumn<String>(nameof(WebRequest), nameof(Protocol), default));
            Version = AddColumn(nameof(Version), database.BuildColumn<String>(nameof(WebRequest), nameof(Version), default));
            Target = AddColumn(nameof(Target), database.BuildColumn<String>(nameof(WebRequest), nameof(Target), default));
            Method = AddColumn(nameof(Method), database.BuildColumn<String>(nameof(WebRequest), nameof(Method), default));
            Headers = AddColumn(nameof(Headers), database.BuildColumn<IDictionary<String, String>>(nameof(WebRequest), nameof(Headers), default));
            Parameters = AddColumn(nameof(Parameters), database.BuildColumn<IDictionary<String, String>>(nameof(WebRequest), nameof(Parameters), default));
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(WebRequest), nameof(Properties), default));
        }

        public override WebRequest Get(int index)
        {
            return (index == -1 ? null : new WebRequest(this, index));
        }
    }
}
