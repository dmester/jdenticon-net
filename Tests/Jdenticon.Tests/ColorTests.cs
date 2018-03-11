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

        [TestMethod]
        public void Color_Mix()
        {
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), 0));
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), 1));
            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(0, 0, 0, 0), 0.5f));

            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), Color.Mix(Color.FromArgb(0, 255, 255, 0), Color.FromArgb(0, 255, 255, 0), 0.5f));

            Assert.AreEqual(Color.FromArgb(127, 128, 128, 128), Color.Mix(Color.FromArgb(0, 255, 0, 0), Color.FromArgb(255, 128, 128, 128), 0.5f));

            Assert.AreEqual(Color.FromArgb(0, 0, 0, 0), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0f));

            Assert.AreEqual(Color.FromArgb(119, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.47f));
            Assert.AreEqual(Color.FromArgb(122, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.48f));
            Assert.AreEqual(Color.FromArgb(124, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.49f));
            Assert.AreEqual(Color.FromArgb(127, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.50f));
            Assert.AreEqual(Color.FromArgb(130, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.51f));
            Assert.AreEqual(Color.FromArgb(132, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 0.52f));
            Assert.AreEqual(Color.FromArgb(255, 255, 255, 255), Color.Mix(Color.FromArgb(0, 0, 0, 0), Color.FromArgb(255, 255, 255, 255), 1f));
        }
    }
}
