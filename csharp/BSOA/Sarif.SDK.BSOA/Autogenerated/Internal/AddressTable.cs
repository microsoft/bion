// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Address'
    /// </summary>
    internal partial class AddressTable : Table<Address>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> AbsoluteAddress;
        internal IColumn<int> RelativeAddress;
        internal IColumn<int> Length;
        internal IColumn<String> Kind;
        internal IColumn<String> Name;
        internal IColumn<String> FullyQualifiedName;
        internal IColumn<int> OffsetFromParent;
        internal IColumn<int> Index;
        internal IColumn<int> ParentIndex;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal AddressTable(SarifLogDatabase database) : base()
        {
            Database = database;

            AbsoluteAddress = AddColumn(nameof(AbsoluteAddress), ColumnFactory.Build<int>(-1));
            RelativeAddress = AddColumn(nameof(RelativeAddress), ColumnFactory.Build<int>(default));
            Length = AddColumn(nameof(Length), ColumnFactory.Build<int>(default));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<String>(default));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<String>(default));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), ColumnFactory.Build<String>(default));
            OffsetFromParent = AddColumn(nameof(OffsetFromParent), ColumnFactory.Build<int>(default));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Address Get(int index)
        {
            return (index == -1 ? null : new Address(this, index));
        }
    }
}
