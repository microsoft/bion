using BSOA.Column;
using BSOA.Model;

using System;

namespace BSOA.Benchmarks.Model
{ 
    internal partial class RunDatabase
    {
        public override IColumn BuildColumn(string tableName, string columnName, Type type, object defaultValue = null)
        {
            //if (type == typeof(string))
            //{
            //    return new DistinctColumn<string>(new StringColumn());
            //}
            //else
            {
                return base.BuildColumn(tableName, columnName, type, defaultValue);
            }
        }
    }
}
