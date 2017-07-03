using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jdenticon.Tests
{
    [TestClass]
    public class HexStringTests
    {
        [TestMethod]
        public void HexString_ToStringAndBack()
        {
            var array = new byte[512];
            var expected = "";
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = (byte)(i % byte.MaxValue);
                expected += array[i].ToString("x2");
            }

            var str = HexString.ToString(array);
            var array2 = HexString.ToArray(str);
            var str2 = HexString.ToString(array2);

            Assert.AreEqual(expected, str);
            Assert.AreEqual(expected, str2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HexString_ToString_Null()
        {
            HexString.ToString(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HexString_ToArray_Null()
        {
            HexString.ToArray(null);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void HexString_ToArray_InvalidChars()
        {
            HexString.ToArray("01234k123");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void HexString_ToArray_InvalidLength()
        {
            HexString.ToArray("12345");
        }

        [TestMethod]
        public void HexString_ToArray_Empty()
        {
            var arr = HexString.ToArray("");
            Assert.AreEqual(0, arr.Length);
        }

    }
}
