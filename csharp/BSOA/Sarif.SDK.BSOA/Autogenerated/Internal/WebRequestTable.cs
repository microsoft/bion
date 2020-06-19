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
            Protocol = AddColumn(nameof(Protocol), ColumnFactory.Build<string>(default));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<string>(default));
            Target = AddColumn(nameof(Target), ColumnFactory.Build<string>(default));
            Method = AddColumn(nameof(Method), ColumnFactory.Build<string>(default));
            Headers = AddColumn(nameof(Headers), ColumnFactory.Build<IDictionary<string, string>>(default));
            Parameters = AddColumn(nameof(Parameters), ColumnFactory.Build<IDictionary<string, string>>(default));
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override WebRequest Get(int index)
        {
            return (index == -1 ? null : new WebRequest(this, index));
        }
    }
}
