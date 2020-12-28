using System;
using Jdenticon.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jdenticon.Tests
{
    [TestClass]
    public class ImmutableHueCollectionTests
    {
        [TestMethod]
        public void Parse()
        {
            var collection = ImmutableHueCollection.Parse("");
            Assert.AreEqual(0, collection.Count);

            collection = ImmutableHueCollection.Parse("42deg, 2.5, 2.5turn, 1rad, 500grad");
            Assert.AreEqual(5, collection.Count);
            Assert.AreEqual(42f / 360, collection[0]);
            Assert.AreEqual(0.5f, collection[1]);
            Assert.AreEqual(0.5f, collection[2]);
            Assert.AreEqual(0.15915494309189533576888376337251f, collection[3]);
            Assert.AreEqual(0.25f, collection[4]);

            Assert.AreEqual("42deg, 0.5, 0.5turn, 1rad, 100grad", collection.ToString());
        }
    }
}
