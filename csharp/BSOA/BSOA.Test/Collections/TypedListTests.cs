using BSOA.Collections;
using BSOA.Column;

using System.Collections.Generic;

using Xunit;

namespace BSOA.Test.Collections
{
    public class TypedListTests
    {
        [Fact]
        public void TypeList_Basics()
        {
            List<string> strings = new List<string> { "One", "Two", "Three" };

            NumberListColumn<int> column = new NumberListColumn<int>();
            TypedList<string> list = new TypedList<string>(column[0], (i) => strings[i], (text) => strings.IndexOf(text));

            Assert.Empty(list);

            list.Add("One");
            Assert.Single(list);
            Assert.Equal(0, column[0][0]);

            list.Add("Two");
            Assert.True(list.Equals(new string[] { "One", "Two" }));
            Assert.Equal(1, column[0][1]);

            // SetTo self works properly
            list.SetTo(list);
            Assert.True(list.Equals(new string[] { "One", "Two" }));

            // SetTo other works
            list.SetTo(new List<string>() { "One" });
            Assert.True(list.Equals(new string[] { "One" }));

            // SetTo empty works
            list.SetTo(new string[0]);
            Assert.Empty(list);

            // SetTo null works
            list.SetTo(null);
            Assert.Empty(list);
        }
    }
}
