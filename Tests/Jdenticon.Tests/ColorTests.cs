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

        [TestMethod]
        public void Color_ParseOfficiallySupported()
        {
            Assert.AreEqual("#00000000", Color.Parse("transparent").ToString("#rrggbbaa"));
            Assert.AreEqual("#66cdaaff", Color.Parse("mediumaquaMarine").ToString("#rrggbbaa"));
            Assert.AreEqual("#aabbccff", Color.Parse("#abc").ToString("#rrggbbaa"));
            Assert.AreEqual("#aabbccdd", Color.Parse("#abcd").ToString("#rrggbbaa")); 
            Assert.AreEqual("#aabbccff", Color.Parse("#aabbcc").ToString("#rrggbbaa"));
            Assert.AreEqual("#abcdef21", Color.Parse("#abcdef21").ToString("#rrggbbaa"));
            Assert.AreEqual("#fffefdff", Color.Parse("rgb(255, 254, 253)").ToString("#rrggbbaa"));
            Assert.AreEqual("#ff7f00ff", Color.Parse("rgb(100%, 50%, 0%)").ToString("#rrggbbaa"));
            Assert.AreEqual("#fffefd7f", Color.Parse("rgba(255, 254, 253, 0.5)").ToString("#rrggbbaa"));
            Assert.AreEqual("#ff7f007f", Color.Parse("rgba(100%, 50%, 0%, 0.5)").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(123, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#bcbcbcff", Color.Parse("hsl(123, 0%, 74% )").ToString("#rrggbbaa"));
        }

        [TestMethod]
        public void Color_ParseNotOfficiallySupported()
        {
            Assert.AreEqual("#fffefd7f", Color.Parse("rgb(255, 254, 253, 0.5)").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(123deg, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(123.000001deg, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbae7f", Color.Parse("hsl(123.000001deg, 23%, 74% , 0.5)").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbae7f", Color.Parse("hsla(123.000001deg, 23%, 74% , 0.5)").ToString("#rrggbbaa"));
            Assert.AreEqual("#000000ff", Color.Parse("hsl(123deg, 23%, 0% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#ffffffff", Color.Parse("hsl(123.000001deg, 23%, 100% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(2.146755rad, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(0.3416667turn, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#adcbaeff", Color.Parse("hsl(136.66667grad, 23%, 74% )").ToString("#rrggbbaa"));
            Assert.AreEqual("#00bfffff", Color.Parse("hwb(195, 0%, 0%)").ToString("#rrggbbaa"));
            Assert.AreEqual("#00bfffb2", Color.Parse("hwb(195, 0%, 0%, 0.7)").ToString("#rrggbbaa"));
            Assert.AreEqual("#9c9c9cff", Color.Parse("hwb(5, 91%, 57%)").ToString("#rrggbbaa"));
            Assert.AreEqual("#bfbfe5b2", Color.Parse("hwb(239.0000deg, 75%, 10%, 0.7)").ToString("#rrggbbaa"));
            Assert.AreEqual("#000000ff", Color.Parse("hwb(5, 0%, 100%)").ToString("#rrggbbaa"));
            Assert.AreEqual("#ffffffff", Color.Parse("hwb(5, 100%, 0%)").ToString("#rrggbbaa"));
        }
    }
}
