using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jdenticon.Tests
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void Range_Equals()
        {
            var range1 = Range.Create(0.4, 0.6);
            var range2 = Range.Create(0.4, 0.6);
            var range3 = Range.Create(0.4, 0.7);
            var range4 = Range.Create(0.3, 0.6);

            Assert.IsTrue(range1.Equals((object)range2));
            Assert.IsTrue(range1.Equals(range2));
            Assert.IsTrue(range1 == range2);
            Assert.IsFalse(range1 != range2);

            Assert.IsFalse(range1.Equals((object)range3));
            Assert.IsFalse(range1.Equals(range3));
            Assert.IsFalse(range1 == range3);
            Assert.IsTrue(range1 != range3);

            Assert.IsFalse(range4.Equals((object)range2));
            Assert.IsFalse(range4.Equals(range2));
            Assert.IsFalse(range4 == range2);
            Assert.IsTrue(range4 != range2);
        }
    }
}
