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
        internal IColumn<string> Protocol;
        internal IColumn<string> Version;
        internal IColumn<string> Target;
        internal IColumn<string> Method;
        internal IColumn<IDictionary<string, string>> Headers;
        internal IColumn<IDictionary<string, string>> Parameters;
        internal RefColumn Body;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal WebRequestTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Protocol = AddColumn(nameof(Protocol), ColumnFactory.Build<string>(default(string)));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<string>(default(string)));
            Target = AddColumn(nameof(Target), ColumnFactory.Build<string>(default(string)));
            Method = AddColumn(nameof(Method), ColumnFactory.Build<string>(default(string)));
            Headers = AddColumn(nameof(Headers), ColumnFactory.Build<IDictionary<string, string>>(default(IDictionary<string, string>)));
            Parameters = AddColumn(nameof(Parameters), ColumnFactory.Build<IDictionary<string, string>>(default(IDictionary<string, string>)));
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override WebRequest Get(int index)
        {
            return (index == -1 ? null : new WebRequest(this, index));
        }
    }
}
