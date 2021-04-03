using Jdenticon.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Jdenticon.Tests
{
    [TestClass]
    public class HueCollectionTests
    {
        [TestMethod]
        public void Remove()
        {
            var collection = new HueCollection();

            collection.Add(180, HueUnit.Degrees);

            Assert.AreEqual(1, collection.Count);

            collection.Remove(0.57f);
            collection.Remove(0.57f, HueUnit.Turns);

            Assert.AreEqual(1, collection.Count);

            collection.Remove(0.5f, HueUnit.Turns);

            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        [DataRow(0.25f, "0.25", -2.75f, null)]
        [DataRow(0.25f, "0.25", 0.25f, null)]
        [DataRow(0.25f, "0.25", 2.25f, null)]

        [DataRow(0.25f, "0.25turn", -2.75f, HueUnit.Turns)]
        [DataRow(0.25f, "0.25turn", 0.25f, HueUnit.Turns)]
        [DataRow(0.25f, "0.25turn", 2.25f, HueUnit.Turns)]

        [DataRow(0.25f, "90deg", -270f, HueUnit.Degrees)]
        [DataRow(0.25f, "90deg", 90f, HueUnit.Degrees)]
        [DataRow(0.25f, "90deg", 450f, HueUnit.Degrees)]

        [DataRow(0.25f, "100grad", -300f, HueUnit.Gradians)]
        [DataRow(0.25f, "100grad", 100f, HueUnit.Gradians)]
        [DataRow(0.25f, "100grad", 500f, HueUnit.Gradians)]

        [DataRow(0.25f, "1.57rad", -1.5f * (float)Math.PI, HueUnit.Radians)]
        [DataRow(0.25f, "1.57rad", 0.5f * (float)Math.PI, HueUnit.Radians)]
        [DataRow(0.25f, "1.57rad", 2.5f * (float)Math.PI, HueUnit.Radians)]
        public void Normalize(float expectedTurns, string expectedString, float value, HueUnit? unit)
        {
            var collection = new HueCollection();
            if (unit == null)
            {
                collection.Add(value);
            }
            else
            {
                collection.Add(value, unit.Value);
            }

            Assert.AreEqual(expectedTurns, collection[0], 0.01f);
            Assert.AreEqual(expectedString, collection.ToString());
        }

        [TestMethod]
        public void Stringify()
        {
            using (new CustomCulture())
            {
                var collection = new HueCollection
                {
                    { 14.5f, HueUnit.Degrees },
                    { 15.5f, HueUnit.Degrees },
                };

                Assert.AreEqual("14.5deg, 15.5deg", collection.ToString());
                Assert.AreEqual("14/5deg, 15/5deg", collection.ToString(null));
                Assert.AreEqual("14,5deg; 15,5deg", collection.ToString(new CultureInfo("sv-se")));
            }
        }
    }
}
