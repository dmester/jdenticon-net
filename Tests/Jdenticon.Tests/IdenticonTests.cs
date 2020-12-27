using System;
using System.IO;
using System.Linq;
using Jdenticon.Rendering;
using Jdenticon.Tests.Icons;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jdenticon.Tests
{
    [TestClass]
    public class IdenticonTests
    {
        // These tests should match the tests in Jdenticon-js and Jdenticon-php.
        // Failed tests might not be incorrect, but ensure the icon are looking the
        // same to a human eye.

        [TestMethod]
        public void Identicon_Icon39()
        {
            Test(39, new IdenticonStyle
            {
                Hues = new HueCollection {
                    { 134, HueUnit.Degrees },
                    { 0, HueUnit.Degrees },
                    { 60, HueUnit.Degrees },
                },
                BackColor = Color.FromRgba(255, 255, 255, 255),
                ColorLightness = Range.Create(0.66f, 0.86f),
                GrayscaleLightness = Range.Create(0.00f, 1.00f),
                ColorSaturation = 0.35f,
                GrayscaleSaturation = 0.10f
            });
        }

        [TestMethod]
        public void Identicon_Icon76()
        {
            Test(76, new IdenticonStyle
            {
                Hues = new HueCollection {
                    { 134, HueUnit.Degrees },
                    { 0, HueUnit.Degrees },
                    { 60, HueUnit.Degrees },
                },
                BackColor = Color.FromRgba(0, 0, 0, 42),
                ColorLightness = Range.Create(0.29f, 0.53f),
                GrayscaleLightness = Range.Create(0.19f, 0.40f),
                ColorSaturation = 0.45f,
                GrayscaleSaturation = 0.72f
            });
        }

        [TestMethod]
        public void Identicon_Icon50()
        {
            Test(50, null);
        }

        [TestMethod]
        public void Identicon_Icon73()
        {
            Test(73, null);
        }

        private static Stream GetExpectedIcon(string name)
        {
            var type = typeof(IconResources);
            return type.Assembly.GetManifestResourceStream(type, name);
        }

        private static void Test(int iconNumber, IdenticonStyle style)
        {
            var icon = Identicon.FromValue(iconNumber, 50);
            if (style != null)
            {
                icon.Style = style;
            }

            using (var actualStream = new MemoryStream())
            {
                icon.SaveAsPng(actualStream);

                var actualBytes = actualStream.ToArray();

                using (var expectedStream = GetExpectedIcon($"{iconNumber}.png"))
                {
                    // +1 to be able to detect a too short actual icon
                    var expectedBytes = new byte[actualBytes.Length + 1];
                    var expectedByteCount = expectedStream.Read(expectedBytes, 0, actualBytes.Length);

                    if (expectedByteCount != actualBytes.Length ||
                        expectedBytes.Take(expectedByteCount).SequenceEqual(actualBytes) == false)
                    {
                        Assert.Fail("Icon '{0}' failed PNG rendering test.", iconNumber);
                    }
                }
            }

            var actualSvg = icon.ToSvg();

            using (var expectedStream = GetExpectedIcon($"{iconNumber}.svg"))
            {
                using (var reader = new StreamReader(expectedStream))
                {
                    var expectedSvg = reader.ReadToEnd();

                    if (actualSvg != expectedSvg)
                    {
                        Assert.Fail("Icon '{0}' failed SVG rendering test. Actual: {1}", iconNumber, actualSvg);
                    }
                }
            }
        }
    }
}
