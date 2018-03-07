using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.Drawing.Rasterization;
using Jdenticon.Rendering;

namespace Jdenticon.Tests
{
    [TestClass]
    public class EdgeTableTests
    {
        [TestMethod]
        public void EdgeTable_VerticalUnaligned()
        {
            var table = new EdgeTable(10, 10);
            table.Add(new Edge(1, new PointF(2.5f, 0), new PointF(2.5f, 10), Color.Red));

            var edges = table[5];
            Assert.AreEqual(1, edges.Count);
            Assert.AreEqual(2, edges[0].FromX);
            Assert.AreEqual(1, edges[0].Width);
        }

        [TestMethod]
        public void EdgeTable_VerticalAligned()
        {
            var table = new EdgeTable(10, 10);
            table.Add(new Edge(1, new PointF(2f, 0), new PointF(2f, 10), Color.Red));

            var edges = table[5];
            Assert.AreEqual(1, edges.Count);
            Assert.AreEqual(2, edges[0].FromX);
            Assert.AreEqual(0, edges[0].Width);
        }

        [TestMethod]
        public void EdgeTable_Diagonal()
        {
            var table = new EdgeTable(10, 10);
            table.Add(new Edge(1, new PointF(0, 0), new PointF(10, 5), Color.Red));

            var edges = table[0];
            Assert.AreEqual(1, edges.Count);
            Assert.AreEqual(0, edges[0].FromX);
            Assert.AreEqual(2, edges[0].Width);
        }
    }
}
