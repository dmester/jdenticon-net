using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.AspNet.Mvc;
using Jdenticon.Rendering;
using Jdenticon.Cryptography;

namespace Jdenticon.Tests
{
    [TestClass]
    public class IdenticonRequestTests
    {
        private static void AssertAreAlmostEqual(float a, float b)
        {
            if (a == b)
            {
                return;
            }

            if (a != 0)
            {
                var diff = a / b;
                if (diff > 0.9f && diff < 1.1f)
                {
                    return;
                }
            }

            Assert.AreEqual(a, b);
        }

        [TestMethod]
        public void IdenticonRequest_Extended()
        {
            var url1 = new IdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 741,
                Style = new IdenticonStyle
                {
                    BackColor = Color.Bisque,
                    ColorLightness = Range.Create(0.25f, 0.75f),
                    GrayscaleLightness = Range.Create(0, 1f),
                    Saturation = 0.5f
                }
            };

            var text = url1.ToString();
            Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));

            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToArgb(), url2.Style.BackColor.ToArgb());
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

            Assert.AreEqual(url1.Size, url2.Size);
        }

        [TestMethod]
        public void IdenticonRequest_Compact_DefaultSize()
        {
            var url1 = new IdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 64
            };

            var text = url1.ToString();
            Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));

            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToArgb(), url2.Style.BackColor.ToArgb());
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

            Assert.AreEqual(url1.Size, url2.Size);
        }

        [TestMethod]
        public void IdenticonRequest_Compact_SizeMultipleOf5()
        {
            var url1 = new IdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 65
            };

            var text = url1.ToString();
            Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));

            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToArgb(), url2.Style.BackColor.ToArgb());
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

            Assert.AreEqual(url1.Size, url2.Size);
        }
    }
}
