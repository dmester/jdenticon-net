using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.Rendering;

namespace Jdenticon.Tests
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void Color_ToString()
        {
            var color = Color.FromArgb(10, 11, 12, 13);

            Assert.AreEqual("", color.ToString(""));

            Assert.AreEqual("#0a0b0c0d#", color.ToString("#aarrggbb#"));
            Assert.AreEqual("0a0b0c0d#", color.ToString("aarrggbb#"));

            Assert.AreEqual("#0A0B0C0D", color.ToString("#AARRGGBB"));
            Assert.AreEqual("#0A0B0C0D", color.ToString("#AARRGGBB"));

            Assert.AreEqual(" rgba(11, 12, 13, 10)", color.ToString(" rgba(R, G, B, A)"));
        }

        [TestMethod]
        public void Color_Equals()
        {
            var color1 = Color.FromArgb(1, 2, 3, 4);
            var color2 = Color.FromArgb(1, 2, 3, 4);
            var color3 = Color.FromArgb(4, 3, 2, 1);

            Assert.IsTrue(color1.Equals((object)color2));
            Assert.IsTrue(color1.Equals(color2));
            Assert.IsTrue(color1 == color2);
            Assert.IsFalse(color1 != color2);

            Assert.IsFalse(color1.Equals((object)color3));
            Assert.IsFalse(color1.Equals(color3));
            Assert.IsFalse(color1 == color3);
            Assert.IsTrue(color1 != color3);
        }
    }
}
