// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Person'
    /// </summary>
    internal partial class PersonTable : Table<Person>
    {
        internal PersonDatabase Database;

        internal IColumn<byte> Age;
        internal IColumn<string> Name;

        internal PersonTable(PersonDatabase database) : base()
        {
            Database = database;

            Age = AddColumn(nameof(Age), ColumnFactory.Build<byte>(default(byte)));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(default(string)));
        }

        public override Person Get(int index)
        {
            return (index == -1 ? null : new Person(this, index));
        }
    }
}
