#region License
//
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// This software is provided 'as-is', without any express or implied
// warranty.In no event will the authors be held liable for any damages
// arising from the use of this software.
// 
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software.If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
//
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
