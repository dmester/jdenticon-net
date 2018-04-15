using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jdenticon.AspNet.Mvc;
using Jdenticon.Rendering;
using System.IO;
using System.Collections.Generic;

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
                    Padding = 0.3f,
                    BackColor = Color.Bisque,
                    ColorLightness = Range.Create(0.25f, 0.75f),
                    GrayscaleLightness = Range.Create(0, 1f),
                    ColorSaturation = 0.4f,
                    GrayscaleSaturation = 0.1f,
                    Hues = new HueCollection { 1.5f, -0.25f, 0.8f }
                }
            };

            var text = url1.ToString();
            Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));

            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToRgba(), url2.Style.BackColor.ToRgba());
            AssertAreAlmostEqual(url1.Style.Padding, url2.Style.Padding);
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

#pragma warning disable 0618
            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.Saturation);
#pragma warning restore 0618

            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.ColorSaturation);
            AssertAreAlmostEqual(url1.Style.GrayscaleSaturation, url2.Style.GrayscaleSaturation);
            Assert.AreEqual(3, url2.Style.Hues.Count);
            AssertAreAlmostEqual(0.5f, url2.Style.Hues[0]);
            AssertAreAlmostEqual(0.75f, url2.Style.Hues[1]);
            AssertAreAlmostEqual(0.8f, url2.Style.Hues[2]);

            Assert.AreEqual(url1.Size, url2.Size);
        }

        [TestMethod]
        public void IdenticonRequest_DefaultStyle()
        {
            var url1 = new IdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 741,
                Style = new IdenticonStyle
                {
                    Padding = 0.3f
                }
            };

            try
            {
                var text = url1.ToString();
                Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));

                AssertAreAlmostEqual(0.3f, url2.Style.Padding);
                Identicon.DefaultStyle.Padding = 0.1f;
                AssertAreAlmostEqual(0.3f, url2.Style.Padding);

                Identicon.DefaultStyle.Padding = 0.3f;

                text = url1.ToString();
                Assert.IsTrue(IdenticonRequest.TryParse(text, out var url3));

                AssertAreAlmostEqual(0.3f, url3.Style.Padding);
                Identicon.DefaultStyle.Padding = 0.2f;

                Assert.IsTrue(IdenticonRequest.TryParse(text, out var url4));
                AssertAreAlmostEqual(0.2f, url4.Style.Padding);
            }
            finally
            {
                // Restore default style so that other tests are not affected
                Identicon.DefaultStyle = null;
            }
        }

        [TestMethod]
        public void IdenticonRequest_v200BackwardCompatiblity()
        {
            var url1 = new IdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 741,
                Style = new IdenticonStyle
                {
                    Padding = 0.3f,
                    BackColor = Color.Bisque,
                    ColorLightness = Range.Create(0.25f, 0.75f),
                    GrayscaleLightness = Range.Create(0, 1f),
                    ColorSaturation = 0.4f,
                    GrayscaleSaturation = 0.1f,
                    Hues = new HueCollection { 1.5f, -0.25f, 0.8f }
                }
            };
            
            var text = url1.ToString();
            Assert.IsTrue(OldIdenticonRequest.TryParse(text, out var url2));

            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToRgba(), url2.Style.BackColor.ToRgba());
            AssertAreAlmostEqual(url1.Style.Padding, url2.Style.Padding);
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

#pragma warning disable 0618
            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.Saturation);
#pragma warning restore 0618

            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.ColorSaturation);
            AssertAreAlmostEqual(IdenticonStyle.DefaultGrayscaleSaturation, url2.Style.GrayscaleSaturation);
            Assert.AreEqual(0, url2.Style.Hues.Count);
            
            Assert.AreEqual(url1.Size, url2.Size);
        }

        [TestMethod]
        public void IdenticonRequest_v200ForwardCompatiblity()
        {
            var url1 = new OldIdenticonRequest
            {
                Hash = HashGenerator.ComputeHash("Hello", "SHA1"),
                Size = 741,
                Style = new IdenticonStyle
                {
                    Padding = 0.3f,
                    BackColor = Color.Bisque,
                    ColorLightness = Range.Create(0.25f, 0.75f),
                    GrayscaleLightness = Range.Create(0, 1f),
                    ColorSaturation = 0.4f,
                    GrayscaleSaturation = 0.1f
                }
            };

            var text = url1.ToString();
            Assert.IsTrue(IdenticonRequest.TryParse(text, out var url2));
            
            Assert.AreEqual(url1.Format, url2.Format);
            Assert.AreEqual(url1.Style.BackColor.ToRgba(), url2.Style.BackColor.ToRgba());
            AssertAreAlmostEqual(url1.Style.Padding, url2.Style.Padding);
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

#pragma warning disable 0618
            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.Saturation);
#pragma warning restore 0618

            AssertAreAlmostEqual(url1.Style.ColorSaturation, url2.Style.ColorSaturation);
            AssertAreAlmostEqual(IdenticonStyle.DefaultGrayscaleSaturation, url2.Style.GrayscaleSaturation);
            AssertAreAlmostEqual(0, url2.Style.Hues.Count);

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
            Assert.AreEqual(url1.Style.BackColor.ToRgba(), url2.Style.BackColor.ToRgba());
            AssertAreAlmostEqual(url1.Style.Padding, url2.Style.Padding);
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
            Assert.AreEqual(url1.Style.BackColor.ToRgba(), url2.Style.BackColor.ToRgba());
            AssertAreAlmostEqual(url1.Style.Padding, url2.Style.Padding);
            AssertAreAlmostEqual(url1.Style.ColorLightness.From, url2.Style.ColorLightness.From);
            AssertAreAlmostEqual(url1.Style.ColorLightness.To, url2.Style.ColorLightness.To);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.From, url2.Style.GrayscaleLightness.From);
            AssertAreAlmostEqual(url1.Style.GrayscaleLightness.To, url2.Style.GrayscaleLightness.To);

            Assert.AreEqual(url1.Size, url2.Size);
        }

        /// <summary>
        /// Previous <see cref="IdenticonRequest"/> implementation used in v2.0.0 and earlier.
        /// </summary>
        class OldIdenticonRequest
        {
            private IdenticonStyle style;
            private int size;
            private static readonly int[] DefaultSizes = new[] { 16, 32, 48, 64, 128, 256, 512 };

            /// <summary>
            /// Gets or sets the hash that the requested icon will be based on.
            /// </summary>
            public byte[] Hash { get; set; }

            /// <summary>
            /// Gets or sets the file format of the requested icon.
            /// </summary>
            public ExportImageFormat Format { get; set; }

            /// <summary>
            /// Gets or sets the style of the requested icon. If no style is set 
            /// the default style is used.
            /// </summary>
            public IdenticonStyle Style
            {
                get
                {
                    if (style == null)
                    {
                        style = new IdenticonStyle();
                    }
                    return style;
                }
                set
                {
                    style = value;
                }
            }

            /// <summary>
            /// Gets or sets the size of the requested icon in pixels.
            /// </summary>
            public int Size
            {
                get { return size; }
                set { size = Math.Max(value, 1); }
            }

            private static byte ComputeChecksum(byte[] buffer, int offset, int count)
            {
                var checksum = (byte)0;
                unchecked
                {
                    for (var i = 0; i < count; i++)
                    {
                        checksum = (byte)(((checksum << 1) | (checksum >> 7)) ^ buffer[offset + i]);
                    }
                }
                return checksum;
            }

            /// <summary>
            /// Tries to parse a request string serialized by <see cref="ToString"/>.
            /// </summary>
            /// <param name="requestString">The request string.</param>
            /// <param name="request">The parsed request if succeeded.</param>
            public static bool TryParse(string requestString, out OldIdenticonRequest request)
            {
                if (requestString == null ||
                    requestString.Length == 0 ||
                    requestString.Length > 50)
                {
                    goto InvalidRequest;
                }

                var base64 = requestString.ToCharArray();
                var offset = base64[0] == '?' ? 1 : 0;

                for (var i = offset; i < base64.Length; i++)
                {
                    switch (base64[i])
                    {
                        case '-': base64[i] = '='; break;
                        case '_': base64[i] = '/'; break;
                        case '~': base64[i] = '+'; break;
                    }
                }

                byte[] data;
                try
                {
                    data = Convert.FromBase64CharArray(base64, offset, base64.Length - offset);
                }
                catch
                {
                    goto InvalidRequest;
                }

                var checksum = ComputeChecksum(data, 1, data.Length - 1);
                if (checksum != data[0])
                {
                    goto InvalidRequest;
                }

                request = new OldIdenticonRequest();

                try
                {
                    using (var dataStream = new MemoryStream(data, 1, data.Length - 1))
                    {
                        using (var reader = new BinaryReader(dataStream))
                        {
                            // Size
                            var size = reader.ReadByte();
                            request.size =
                                size == 255 ? reader.ReadUInt16() :
                                size < DefaultSizes.Length ? DefaultSizes[size] :
                                (size - DefaultSizes.Length) * 5;

                            // Flags:
                            // Bit 0: explicit style (bool)
                            // Bit 1-3: image format (int)
                            var flags = reader.ReadByte();

                            // Format
                            request.Format = (ExportImageFormat)((flags >> 1) & 0b111);

                            // Style
                            var explicitStyle = (flags & 0b1) == 0b1;
                            if (explicitStyle)
                            {
                                // By limiting the number of decimals we are limiting 
                                // the rounding error created by the transformation 
                                // from and to a byte.
                                const int DecimalPrecision = 3;

                                var padding = (float)Math.Round(
                                    reader.ReadByte() / 637f, DecimalPrecision);

                                var a = reader.ReadByte();
                                var r = reader.ReadByte();
                                var g = reader.ReadByte();
                                var b = reader.ReadByte();

                                var grayscaleLightnessFrom = (float)Math.Round(
                                    reader.ReadByte() / 255f, DecimalPrecision);
                                var grayscaleLightnessTo = (float)Math.Round(
                                    reader.ReadByte() / 255f, DecimalPrecision);

                                var colorLightnessFrom = (float)Math.Round(
                                    reader.ReadByte() / 255f, DecimalPrecision);
                                var colorLightnessTo = (float)Math.Round(
                                    reader.ReadByte() / 255f, DecimalPrecision);

                                var saturation = (float)Math.Round(
                                    reader.ReadByte() / 255f, DecimalPrecision);

                                request.style = new IdenticonStyle
                                {
                                    Padding = padding,
                                    BackColor = Color.FromArgb(a, r, g, b),
                                    ColorLightness = Range.Create(colorLightnessFrom, colorLightnessTo),
                                    GrayscaleLightness = Range.Create(grayscaleLightnessFrom, grayscaleLightnessTo),
#pragma warning disable 0618
                                    Saturation = saturation
#pragma warning restore 0618
                                };
                            }

                            // Hash
                            request.Hash = reader.ReadBytes(10);
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                    goto InvalidRequest;
                }

                // Protect against too large requests
                if (request.size > 1000)
                {
                    goto InvalidRequest;
                }

                return true;

                InvalidRequest:
                request = null;
                return false;
            }

            /// <summary>
            /// Serializes this request to a string that can be deserialized using <see cref="TryParse(string, out IdenticonRequest)"/>
            /// </summary>
            public override string ToString()
            {
                using (var dataStream = new MemoryStream(50))
                {
                    var buffer = dataStream.GetBuffer();

                    using (var writer = new BinaryWriter(dataStream))
                    {
                        // Checksum placeholder
                        writer.Write((byte)0);

                        // Size
                        var defaultSizeIndex = ((IList<int>)DefaultSizes).IndexOf(size);
                        if (defaultSizeIndex >= 0)
                        {
                            writer.Write((byte)defaultSizeIndex);
                        }
                        else if ((size % 5) == 0 && size <= (254 - DefaultSizes.Length) * 5)
                        {
                            writer.Write((byte)(DefaultSizes.Length + size / 5));
                        }
                        else
                        {
                            writer.Write((byte)0xff);
                            writer.Write((ushort)size);
                        }

                        // Flags:
                        // Bit 0: explicit style (bool)
                        // Bit 1-3: image format (int)
                        var explicitStyle = style != null && !style.Equals(new IdenticonStyle());
                        var format = (int)Format;

                        writer.Write((byte)(
                            (format << 1) |
                            (explicitStyle ? 1 : 0)));

                        // Style
                        if (explicitStyle)
                        {
                            writer.Write((byte)(style.Padding * 637f));

                            writer.Write((byte)(style.BackColor.A));
                            writer.Write((byte)(style.BackColor.R));
                            writer.Write((byte)(style.BackColor.G));
                            writer.Write((byte)(style.BackColor.B));

                            writer.Write((byte)(style.GrayscaleLightness.From * 255));
                            writer.Write((byte)(style.GrayscaleLightness.To * 255));

                            writer.Write((byte)(style.ColorLightness.From * 255));
                            writer.Write((byte)(style.ColorLightness.To * 255));

#pragma warning disable 0618
                            writer.Write((byte)(style.Saturation * 255));
#pragma warning restore 0618
                        }

                        // Hash
                        if (Hash.Length > 10)
                        {
                            writer.Write(Hash, 0, 6);
                            writer.Write(Hash, Hash.Length - 4, 4);
                        }
                        else
                        {
                            writer.Write(Hash);
                        }

                        writer.Flush();

                        var checksum = ComputeChecksum(buffer, 1, (int)dataStream.Length - 1);
                        buffer[0] = checksum;

                        var base64 = new char[dataStream.Length * 2];
                        var base64Length = Convert.ToBase64CharArray(buffer, 0, (int)dataStream.Length, base64, 0);

                        // Replace /, = and + with characters that are not reserved characters in percent-encoding
                        for (var i = 0; i < base64Length; i++)
                        {
                            switch (base64[i])
                            {
                                case '=': base64[i] = '-'; break;
                                case '/': base64[i] = '_'; break;
                                case '+': base64[i] = '~'; break;
                            }
                        }

                        return new string(base64, 0, base64Length);
                    }
                }
            }
        }
    }
}
