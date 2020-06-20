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
        internal IColumn<String> Protocol;
        internal IColumn<String> Version;
        internal IColumn<int> StatusCode;
        internal IColumn<String> ReasonPhrase;
        internal IColumn<IDictionary<String, String>> Headers;
        internal RefColumn Body;
        internal IColumn<bool> NoResponseReceived;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal WebResponseTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(WebResponse), nameof(Index), -1));
            Protocol = AddColumn(nameof(Protocol), database.BuildColumn<String>(nameof(WebResponse), nameof(Protocol), default));
            Version = AddColumn(nameof(Version), database.BuildColumn<String>(nameof(WebResponse), nameof(Version), default));
            StatusCode = AddColumn(nameof(StatusCode), database.BuildColumn<int>(nameof(WebResponse), nameof(StatusCode), default));
            ReasonPhrase = AddColumn(nameof(ReasonPhrase), database.BuildColumn<String>(nameof(WebResponse), nameof(ReasonPhrase), default));
            Headers = AddColumn(nameof(Headers), database.BuildColumn<IDictionary<String, String>>(nameof(WebResponse), nameof(Headers), default));
            Body = AddColumn(nameof(Body), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            NoResponseReceived = AddColumn(nameof(NoResponseReceived), database.BuildColumn<bool>(nameof(WebResponse), nameof(NoResponseReceived), false));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(WebResponse), nameof(Properties), default));
        }

        public override WebResponse Get(int index)
        {
            return (index == -1 ? null : new WebResponse(this, index));
        }
    }
}
