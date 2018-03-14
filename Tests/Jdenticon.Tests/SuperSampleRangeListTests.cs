using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.Drawing.Rasterization;

namespace Jdenticon.Tests
{
    [TestClass]
    public class SuperSampleRangeListTests
    {
        [TestMethod]
        public void SuperSampleRangeList_Single()
        {
            var ssRanges = new SuperSampleRangeList();
            ssRanges.Populate(new[]
            {
                new EdgeIntersectionRange{FromX = 6, Width = 0 }
            });

            Assert.AreEqual(1, ssRanges.Count);
            Assert.AreEqual(6, ssRanges[0].FromX);
            Assert.AreEqual(0, ssRanges[0].Width);
        }

        [TestMethod]
        public void SuperSampleRangeList_Overlapping()
        {
            var ssRanges = new SuperSampleRangeList();
            ssRanges.Populate(new[]
            {
                new EdgeIntersectionRange{FromX = 5, Width = 10 },
                new EdgeIntersectionRange{FromX = 10, Width = 10 }
            });

            Assert.AreEqual(1, ssRanges.Count);
            Assert.AreEqual(5, ssRanges[0].FromX);
            Assert.AreEqual(15, ssRanges[0].Width);
        }

        [TestMethod]
        public void SuperSampleRangeList_Containing()
        {
            var ssRanges = new SuperSampleRangeList();
            ssRanges.Populate(new[]
            {
                new EdgeIntersectionRange{FromX = 10, Width = 20 },
                new EdgeIntersectionRange{FromX = 15, Width = 5 }
            });

            Assert.AreEqual(1, ssRanges.Count);
            Assert.AreEqual(10, ssRanges[0].FromX);
            Assert.AreEqual(20, ssRanges[0].Width);
        }

        [TestMethod]
        public void SuperSampleRangeList_Adjacent_NoDistance()
        {
            var ssRanges = new SuperSampleRangeList();
            ssRanges.Populate(new[]
            {
                new EdgeIntersectionRange{FromX = 5, Width = 10 },
                new EdgeIntersectionRange{FromX = 14, Width = 10 }
            });

            Assert.AreEqual(1, ssRanges.Count);
            Assert.AreEqual(5, ssRanges[0].FromX);
            Assert.AreEqual(19, ssRanges[0].Width);
        }
        
        [TestMethod]
        public void SuperSampleRangeList_Adjacent_2pxDistance()
        {
            var ssRanges = new SuperSampleRangeList();
            ssRanges.Populate(new[]
            {
                new EdgeIntersectionRange{FromX = 5, Width = 10 },
                new EdgeIntersectionRange{FromX = 17, Width = 10 }
            });

            Assert.AreEqual(2, ssRanges.Count);

            Assert.AreEqual(5, ssRanges[0].FromX);
            Assert.AreEqual(10, ssRanges[0].Width);

            Assert.AreEqual(17, ssRanges[1].FromX);
            Assert.AreEqual(10, ssRanges[1].Width);
        }
    }
}
