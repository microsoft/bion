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

            AbsoluteAddress = AddColumn(nameof(AbsoluteAddress), database.BuildColumn<int>(nameof(Address), nameof(AbsoluteAddress), -1));
            RelativeAddress = AddColumn(nameof(RelativeAddress), database.BuildColumn<int>(nameof(Address), nameof(RelativeAddress), default));
            Length = AddColumn(nameof(Length), database.BuildColumn<int>(nameof(Address), nameof(Length), default));
            Kind = AddColumn(nameof(Kind), database.BuildColumn<String>(nameof(Address), nameof(Kind), default));
            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(Address), nameof(Name), default));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), database.BuildColumn<String>(nameof(Address), nameof(FullyQualifiedName), default));
            OffsetFromParent = AddColumn(nameof(OffsetFromParent), database.BuildColumn<int>(nameof(Address), nameof(OffsetFromParent), default));
            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(Address), nameof(Index), -1));
            ParentIndex = AddColumn(nameof(ParentIndex), database.BuildColumn<int>(nameof(Address), nameof(ParentIndex), -1));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Address), nameof(Properties), default));
        }

        public override Address Get(int index)
        {
            return (index == -1 ? null : new Address(this, index));
        }
    }
}
