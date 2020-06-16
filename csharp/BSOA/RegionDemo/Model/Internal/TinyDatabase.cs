// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Database for 'TinyLog'
    /// </summary>
    internal partial class TinyDatabase : Database
    {
        internal static TinyDatabase Current { get; private set; }
        
        internal ArtifactContentTable ArtifactContent { get; }
        internal MessageTable Message { get; }
        internal RegionTable Region { get; }
        internal TinyLogTable TinyLog { get; }

        public TinyDatabase()
        {
            Current = this;

            ArtifactContent = AddTable(nameof(ArtifactContent), new ArtifactContentTable(this));
            Message = AddTable(nameof(Message), new MessageTable(this));
            Region = AddTable(nameof(Region), new RegionTable(this));
            TinyLog = AddTable(nameof(TinyLog), new TinyLogTable(this));
        }
    }
}
