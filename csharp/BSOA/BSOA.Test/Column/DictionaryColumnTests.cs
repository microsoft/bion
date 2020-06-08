using BSOA.Collections;
using BSOA.Column;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test
{
    public class DictionaryColumnTests
    {

        public static IDictionary<string, string> SampleRow()
        {
            DictionaryColumn<string, string> column = new DictionaryColumn<string, string>(
                new DistinctColumn<string>(new StringColumn(), null),
                new StringColumn());

            return column[0];
        }

        //[Fact]
        //public void DictionaryColumn_Basics()
        //{
        //    DictionaryColumn<string, string> other = new DictionaryColumn<string, string>(
        //        new StringColumn(), new StringColumn());

        //    ColumnDictionary<string, string> defaultValue = ColumnDictionary<string, string>.Empty;

        //    other[1] = new Dictionary<string, string>()
        //    {
        //        ["Name"] = "Scott",
        //        ["City"] = "Redmond"
        //    };

        //    IDictionary<string, string> otherValue = other[1];

        //    Column.Basics<IDictionary<string, string>>(
        //        () => new DictionaryColumn<string, string>(
        //            new DistinctColumn<string>(new StringColumn(), null),
        //            new StringColumn()),
        //        defaultValue,
        //        otherValue,
        //        (i) => {
        //            other[0].Clear();
        //            other[0][(i % 10).ToString()] = i.ToString();
        //            other[0][(i % 20).ToString()] = i.ToString();
        //            return other[0];
        //        }
        //    );
        //}
    }
}
