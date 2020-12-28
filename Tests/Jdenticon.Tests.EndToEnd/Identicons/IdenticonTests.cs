using System;
using System.IO;
using System.Linq;
using Jdenticon.Rendering;
using Jdenticon.Tests.EndToEnd.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jdenticon.Tests.EndToEnd.Identicons
{
    [TestClass]
    public class IdenticonTests
    {
        private static TestBed testBed;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testBed = TestBed.Create<IdenticonTests>(context);
        }

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

                actualStream.Position = 0;
                testBed.AssertEqual(actualStream, $"{iconNumber}.png");
            }

            var actualSvg = icon.ToSvg();
            testBed.AssertEqual(actualSvg, $"{iconNumber}.svg");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testBed?.Dispose();
        }
    }
}
