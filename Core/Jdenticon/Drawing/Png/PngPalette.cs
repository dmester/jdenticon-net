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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Png
{
    internal struct PngPalette
    {
        private Color[] colors;
        private int count;
        private bool hasAlphaChannel;

        public Color this[int index] => colors[index];

        public int Count => count;

        public bool HasAlphaChannel => hasAlphaChannel;

        public bool Populate(ColorRange[] canvas)
        {
            var addedColors = new Dictionary<Color, bool>();
            
            var colors = new Color[256];
            var hasAlphaChannel = false;
            var count = 0;

            for (var i = 0; i < canvas.Length; i++)
            {
                if (canvas[i].Count <= 0)
                {
                    break;
                }

                var color = canvas[i].Color;
                
                if (!addedColors.ContainsKey(color))
                {
                    addedColors[color] = true;

                    if (count >= colors.Length)
                    {
                        return false;
                    }

                    colors[count++] = color;

                    if (color.A != 255)
                    {
                        hasAlphaChannel = true;
                    }
                }
            }

            this.colors = colors;
            this.hasAlphaChannel = hasAlphaChannel;
            this.count = count;
            return true;
        }
    }
}
