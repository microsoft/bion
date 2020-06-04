using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: Root Database properties
    /// </summary>
    public partial class SarifLogBsoa : Database
    {
        public IList<Run> Runs
        {
            get => Root[0].Runs;
            set => Root[0].Runs = value;
        }
    }
}
