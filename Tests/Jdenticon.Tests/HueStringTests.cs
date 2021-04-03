using Jdenticon.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;

namespace Jdenticon.Tests
{
    [TestClass]
    public class HueStringTests
    {
        [TestMethod]
        [DataRow("14.5deg, 15.5deg", "notspecified")]
        [DataRow("14/5deg, 15/5deg", null)]
        [DataRow("14,5deg; 15,5deg", "sv-se")]
        public void Stringify(string expectedSerialization, string culture)
        {
            using (new CustomCulture())
            {
                var collection = new HueCollection
                {
                    { 14.5f, HueUnit.Degrees },
                    { 15.5f, HueUnit.Degrees },
                };

                var hueStr = new HueString(collection);

                string serialized;

                if (culture == "notspecified")
                {
                    serialized = hueStr.ToString();
                }
                else if (culture == null)
                {
                    serialized = hueStr.ToString(null);
                }
                else
                {
                    serialized = hueStr.ToString(new CultureInfo(culture));
                }

                Assert.AreEqual(expectedSerialization, serialized);

                HueString deserialized;

                if (culture == "notspecified")
                {
                    deserialized = HueString.Parse(serialized);
                }
                else if (culture == null)
                {
                    deserialized = HueString.Parse(serialized, CultureInfo.CurrentCulture);
                }
                else
                {
                    deserialized = HueString.Parse(serialized, new CultureInfo(culture));
                }

                var deserializedCollection = deserialized.ToCollection();

                Assert.AreEqual(2, deserializedCollection.Count);

                Assert.AreEqual(14.5f, deserializedCollection.Values.ElementAt(0).Value);
                Assert.AreEqual(15.5f, deserializedCollection.Values.ElementAt(1).Value);
            }
        }
    }
}
