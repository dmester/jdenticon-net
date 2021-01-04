using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Jdenticon.Tests
{
    [TestClass]
    public class NumericListTests
    {
        [TestMethod]
        [DataRow("sv-SE", ';')]
        [DataRow("en-US", ',')]
        public void GetSeparator(string culture, char separator)
        {
            Assert.AreEqual(separator, NumericList.GetSeparator(CultureInfo.GetCultureInfo(culture)));
        }

        [TestMethod]
        [DataRow("en-US", " 2.4 5.6, 6  ,  7  ", "2.4", "5.6", "6", "7")]
        [DataRow("sv-SE", " 2.4 5.6; 6  ;  7  ", "2.4", "5.6", "6", "7")]
        public void Split(string culture, string input, params string[] expectedOutput)
        {
            var output = NumericList.Parse(input, CultureInfo.GetCultureInfo(culture));
            CollectionAssert.AreEquivalent(expectedOutput, output);
        }

        [TestMethod]
        [DataRow("en-US", "1.1, 2.2, 3.3")]
        [DataRow("sv-SE", "1,1; 2,2; 3,3")]
        public void Join(string culture, string expectedOutput)
        {
            var actual = NumericList.Join(new[] { 1.1, 2.2, 3.3 }, CultureInfo.GetCultureInfo(culture));
            Assert.AreEqual(expectedOutput, actual);
        }
    }
}
