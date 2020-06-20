// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'PropertyBag'
    /// </summary>
    internal partial class PropertyBagTable : Table<PropertyBag>
    {
        internal SarifLogDatabase Database;

        internal IColumn<IList<String>> Tags;

        internal PropertyBagTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tags = AddColumn(nameof(Tags), database.BuildColumn<IList<String>>(nameof(PropertyBag), nameof(Tags), default));
        }

        public override PropertyBag Get(int index)
        {
            return (index == -1 ? null : new PropertyBag(this, index));
        }
    }
}
