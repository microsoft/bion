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
        internal IColumn<string> Kind;
        internal IColumn<string> Name;
        internal IColumn<string> FullyQualifiedName;
        internal IColumn<int> OffsetFromParent;
        internal IColumn<int> Index;
        internal IColumn<int> ParentIndex;
        internal IColumn<IDictionary<string, string>> Properties;

        internal AddressTable(SarifLogDatabase database) : base()
        {
            Database = database;

            AbsoluteAddress = AddColumn(nameof(AbsoluteAddress), ColumnFactory.Build<int>(-1));
            RelativeAddress = AddColumn(nameof(RelativeAddress), ColumnFactory.Build<int>());
            Length = AddColumn(nameof(Length), ColumnFactory.Build<int>());
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<string>());
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>());
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), ColumnFactory.Build<string>());
            OffsetFromParent = AddColumn(nameof(OffsetFromParent), ColumnFactory.Build<int>());
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Address Get(int index)
        {
            return (index == -1 ? null : new Address(this, index));
        }
    }
}
