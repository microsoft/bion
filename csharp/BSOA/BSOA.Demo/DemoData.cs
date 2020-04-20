using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace BSOA.Demo
{
    public class DemoData
    {
        public List<Region> Regions { get; set; }

        public DemoData()
        {
            Regions = new List<Region>();
        }
    }
}
