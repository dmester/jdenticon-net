using Jdenticon.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Jdenticon.Tests
{
    [TestClass]
    public class HueTests
    {
        [TestMethod]
        [DataRow("en-us", "0.25000001", "0.25", 0.25f)]
        [DataRow("en-us", "0.25000001turn", "0.25turn", 0.25f)]
        [DataRow("en-us", "90.00000000deg", "90deg", 0.25f)]
        [DataRow("en-us", "450.00000000deg", "90deg", 0.25f)]
        [DataRow("en-us", "100GRAD", "100grad", 0.25f)]
        [DataRow("en-us", "1.5707963267948966192313216916398rad", "1.57rad", 0.25f)]
        [DataRow("sv-se", "1,5707963267948966192313216916398rad", "1,57rad", 0.25f)]
        public void Parse(string culture, string input, string output, float turns)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            var value = HueValue.Parse(input, cultureInfo);
            Assert.AreEqual(turns, value.Turns, 0.01);
            Assert.AreEqual(output, value.ToString(cultureInfo));
        }
    }
}
