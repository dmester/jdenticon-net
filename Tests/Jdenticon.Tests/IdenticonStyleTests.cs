using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.Rendering;

namespace Jdenticon.Tests
{
    [TestClass]
    public class IdenticonStyleTests
    {
        [TestMethod]
        public void IdenticonStyle_Clone()
        {
            var original = new IdenticonStyle
            {
                BackColor = Color.FromRgba(1, 2, 3, 4),
                ColorLightness = Range.Create(0.1f, 0.5f),
                GrayscaleLightness = Range.Create(0.3f, 0.7f),
                ColorSaturation = 0.6f,
                GrayscaleSaturation = 0.9f,
                Hues = new HueCollection { 555.1f, 0.9f, 0.75f, -0.8f, -2f, 1f },
                Padding = 0.3f
            };

            var clone = original.Clone();

            Assert.AreEqual(original.BackColor, clone.BackColor);
            Assert.AreEqual(original.ColorLightness, clone.ColorLightness);
            Assert.AreEqual(original.GrayscaleLightness, clone.GrayscaleLightness);
            Assert.AreEqual(original.ColorSaturation, clone.ColorSaturation);
            Assert.AreEqual(original.GrayscaleSaturation, clone.GrayscaleSaturation);
            Assert.AreEqual(original.Hues, clone.Hues);
            Assert.AreEqual(original.Padding, clone.Padding);

            clone.Hues.Add(7f);

            Assert.AreNotEqual(original.Hues, clone.Hues);
        }
    }
}
