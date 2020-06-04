using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Test.Model.V1
{
    /// <summary>
    ///  GENERATED: Root Database properties
    /// </summary>
    public partial class PersonDatabase : Database
    {
        public IList<Person> People
        {
            get => Root[0].People;
            set => Root[0].People = value;
        }
    }
}
