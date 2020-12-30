﻿#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using Jdenticon.IO;
using Jdenticon.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon
{
    /// <summary>
    /// Holds the information needed to render a requested icon, and supports serializing 
    /// to and from <see cref="string"/>.
    /// </summary>
    public class IdenticonRequest
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
                    style = Identicon.DefaultStyle.Clone();
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
        public static bool TryParse(string requestString, out IdenticonRequest request)
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

            request = new IdenticonRequest();

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

                            var colorSaturation = (float)Math.Round(
                                reader.ReadByte() / 255f, DecimalPrecision);

                            // Hash
                            request.Hash = reader.ReadBytes(10);

                            // GrayscaleSaturation was added in version 2.1.0 so it might not be available.
                            var grayscaleSaturation =
                                dataStream.Position < dataStream.Length ?
                                (float)Math.Round(reader.ReadByte() / 255f, DecimalPrecision) :
                                IdenticonStyle.DefaultGrayscaleSaturation;

                            request.style = new IdenticonStyle
                            {
                                Padding = padding,
                                BackColor = Color.FromArgb(a, r, g, b),
                                ColorLightness = Range.Create(colorLightnessFrom, colorLightnessTo),
                                GrayscaleLightness = Range.Create(grayscaleLightnessFrom, grayscaleLightnessTo),
                                ColorSaturation = colorSaturation,
                                GrayscaleSaturation = grayscaleSaturation
                            };

                            // Hues
                            var hueCount = dataStream.Position < dataStream.Length ? reader.ReadByte() : 0;
                            for (var i = 0; i < hueCount; i++)
                            {
                                var hue = (float)Math.Round(reader.ReadByte() / 255f, DecimalPrecision);
                                request.style.Hues.Add(hue);
                            }
                        }
                        else
                        {
                            // Hash
                            request.Hash = reader.ReadBytes(10);
                        }
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
                    var explicitStyle = style != null && !style.Equals(Identicon.DefaultStyle);
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

                        writer.Write((byte)(style.ColorSaturation * 255));
                    }

                    // Hash
                    if (Hash.Length != 10)
                    {
                        writer.Write(Hash, 0, 6);
                        writer.Write(Hash, Hash.Length - 4, 4);
                    }
                    else
                    {
                        writer.Write(Hash);
                    }

                    // Continued style (added in v2.1.0)
                    if (explicitStyle)
                    {
                        writer.Write((byte)(style.GrayscaleSaturation * 255));

                        var hueCount = style.Hues.Count;
                        writer.Write((byte)hueCount);
                        for (var i = 0; i < hueCount; i++)
                        {
                            writer.Write((byte)(style.Hues[i] * 255));
                        }
                    }

                    writer.Flush();

#if HAVE_MEMORYSTREAM_GETBUFFER
                    var buffer = dataStream.GetBuffer();
#else
                    var buffer = dataStream.ToArray();
#endif

                    var bufferLength = (int)dataStream.Length;

                    var checksum = ComputeChecksum(buffer, 1, bufferLength - 1);
                    buffer[0] = checksum;

                    var base64 = new char[bufferLength * 2];
                    var base64Length = Convert.ToBase64CharArray(buffer, 0, bufferLength, base64, 0);

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
