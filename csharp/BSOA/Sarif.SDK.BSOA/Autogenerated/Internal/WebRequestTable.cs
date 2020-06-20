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

            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Protocol = AddColumn(nameof(Protocol), ColumnFactory.Build<String>(default));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<String>(default));
            Target = AddColumn(nameof(Target), ColumnFactory.Build<String>(default));
            Method = AddColumn(nameof(Method), ColumnFactory.Build<String>(default));
            Headers = AddColumn(nameof(Headers), ColumnFactory.Build<IDictionary<String, String>>(default));
            Parameters = AddColumn(nameof(Parameters), ColumnFactory.Build<IDictionary<String, String>>(default));
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override WebRequest Get(int index)
        {
            return (index == -1 ? null : new WebRequest(this, index));
        }
    }
}
