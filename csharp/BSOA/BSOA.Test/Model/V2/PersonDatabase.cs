using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V2
{
    /// <summary>
    ///  GENERATED: BSOA Database
    /// </summary>
    public partial class PersonDatabase : Database
    {
        internal static PersonDatabase Current { get; private set; }
        
        internal PersonTable Person { get; }
        internal RootTable Root { get; }

        public PersonDatabase()
        {
            Current = this;

            Person = AddTable(nameof(Person), new PersonTable(this));
            Root = AddTable(nameof(Root), new RootTable(this));
        }
    }
}
