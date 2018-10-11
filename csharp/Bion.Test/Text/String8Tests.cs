using Bion.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Text
{
    [TestClass]
    public class String8Tests
    {
        [TestMethod]
        public void String8_Basics()
        {
            String8 one = new String8("one");
            String8 two = new String8("two");
            String8 one2 = new String8("one");

            Assert.IsFalse(one.Equals(two));
            Assert.AreNotEqual(one.GetHashCode(), two.GetHashCode());

            Assert.IsTrue(one.Equals(one2));
            Assert.AreEqual(one.GetHashCode(), one2.GetHashCode());
        }
    }
}
