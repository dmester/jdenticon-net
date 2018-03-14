#region License
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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.Drawing.Png
{
    internal class PngEncoder
    {
        private PngPalette palette;
        private Stream output;
        
        /// <summary>
        /// The canvas to be encoded. Ranges cannot span multiple scanlines. Input is not validated.
        /// </summary>
        public ColorRange[] Canvas { get; set; }

        /// <summary>
        /// Bitmap width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Bitmap height.
        /// </summary>
        public int Height { get; set; }
        
        private void WriteSignature()
        {
            var signature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
            output.Write(signature, 0, signature.Length);
        }

        private void WriteImageHeader()
        {
            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.ImageHeader))
            {
                var colorType = palette.Count > 0 ? 
                    PngColorType.IndexedColour : PngColorType.TruecolourWithAlpha;

                chunk.WriteBigEndian(Width);
                chunk.WriteBigEndian(Height);
                chunk.WriteByte(8); // Bit depth
                chunk.WriteByte(colorType);
                chunk.WriteByte(0); // Compression
                chunk.WriteByte(0); // Filter
                chunk.WriteByte(0); // Interlace
            }
        }

        private void WriteTransparency()
        {
            if (palette.Count == 0 || !palette.HasAlphaChannel)
            {
                return;
            }

            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.Transparency))
            {
                for (var i = 0; i < palette.Count; i++)
                {
                    chunk.WriteByte((byte)palette[i].A);
                }
            }
        }

        private void WritePalette()
        {
            if (palette.Count == 0)
            {
                return;
            }

            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.Palette))
            {
                for (var i = 0; i < palette.Count; i++)
                {
                    chunk.WriteByte((byte)palette[i].R);
                    chunk.WriteByte((byte)palette[i].G);
                    chunk.WriteByte((byte)palette[i].B);
                }
            }
        }
        
        private void WriteImageGamma()
        {
            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.ImageGamma))
            {
                chunk.WriteBigEndian(45455);
            }
        }
        
        private  void WriteTextualData(string key, string value)
        {
            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.TextualData))
            {
                chunk.Write(key);
                chunk.WriteByte(0);
                chunk.Write(value);
            }
        }

        private void WriteTrueColorWithAlpha(Stream idat)
        {
            var buf = new byte[(Width * 4 + 1) * Height];
            var outputCursor = 0;
            var canvasCursor = 0;

            for (var y = 0; y < Height; y++)
            {
                buf[outputCursor++] = 0; // No filtering

                for (var x = 0; x < Width; canvasCursor++)
                {
                    var r = Canvas[canvasCursor];

                    var red = (byte)r.Color.R;
                    var green = (byte)r.Color.G;
                    var blue = (byte)r.Color.B;
                    var alpha = (byte)r.Color.A;

                    for (var i = 0; i < r.Count; i++)
                    {
                        buf[outputCursor + i * 4 + 0] = red;
                        buf[outputCursor + i * 4 + 1] = green;
                        buf[outputCursor + i * 4 + 2] = blue;
                        buf[outputCursor + i * 4 + 3] = alpha;
                    }

                    outputCursor += r.Count * 4;
                    x += r.Count;
                }
            }

            idat.Write(buf, 0, outputCursor);
        }

        private void WriteIndexed(Stream idat)
        {
            var buf = new byte[(Width + 1) * Height];
            var outputCursor = 0;
            var canvasCursor = 0;

            var paletteLookup = new Dictionary<Color, byte>();

            for (var i = 0; i < palette.Count; i++)
            {
                paletteLookup[palette[i]] = (byte)i;
            }

            for (var y = 0; y < Height; y++)
            {
                buf[outputCursor++] = 0; // No filtering

                for (var x = 0; x < Width; canvasCursor++)
                {
                    var r = Canvas[canvasCursor];
                    var paletteIndex = paletteLookup[r.Color];

                    for (var i = 0; i < r.Count; i++)
                    {
                        buf[outputCursor + i] = paletteIndex;
                    }

                    x += r.Count;
                    outputCursor += r.Count;
                }
            }

            idat.Write(buf, 0, outputCursor);
        }

        private void WriteImageData()
        {
            using (var chunk = new PngChunkStream(output, PngChunkIdentifier.ImageData))
            {
                using (var deflate = new ZlibStream(chunk))
                {
                    if (palette.Count > 0)
                    {
                        WriteIndexed(deflate);
                    }
                    else
                    {
                        WriteTrueColorWithAlpha(deflate);
                    }
                }
            }
        }

        private void WriteImageEnd()
        {
            using (new PngChunkStream(output, PngChunkIdentifier.ImageEnd))
            {
            }
        }

        public void Save(Stream output)
        {
            this.output = output;

            palette.Populate(Canvas);

            WriteSignature();
            WriteImageHeader();

            WriteImageGamma();
            WritePalette();
            WriteTransparency();
            WriteTextualData(PngKeyword.Software, "Jdenticon");
            
            WriteImageData();

            WriteImageEnd();

            this.output = null;
        }
    }
}
