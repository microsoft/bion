// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'WebResponse'
    /// </summary>
    internal partial class WebResponseTable : Table<WebResponse>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Index;
        internal IColumn<string> Protocol;
        internal IColumn<string> Version;
        internal IColumn<int> StatusCode;
        internal IColumn<string> ReasonPhrase;
        internal IColumn<IDictionary<string, string>> Headers;
        internal RefColumn Body;
        internal IColumn<bool> NoResponseReceived;
        internal IColumn<IDictionary<string, string>> Properties;

        internal WebResponseTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Protocol = AddColumn(nameof(Protocol), ColumnFactory.Build<string>());
            Version = AddColumn(nameof(Version), ColumnFactory.Build<string>());
            StatusCode = AddColumn(nameof(StatusCode), ColumnFactory.Build<int>());
            ReasonPhrase = AddColumn(nameof(ReasonPhrase), ColumnFactory.Build<string>());
            Headers = AddColumn(nameof(Headers), ColumnFactory.Build<IDictionary<string, string>>());
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            NoResponseReceived = AddColumn(nameof(NoResponseReceived), ColumnFactory.Build<bool>(false));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override WebResponse Get(int index)
        {
            return (index == -1 ? null : new WebResponse(this, index));
        }
    }
}
