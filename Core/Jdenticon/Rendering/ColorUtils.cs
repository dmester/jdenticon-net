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

namespace Jdenticon.Rendering
{
    internal static class ColorUtils
    {
        private static readonly float[] LightnessCompensations = new[]
        {
            0.55f, 0.5f, 0.5f, 0.46f, 0.6f, 0.55f, 0.55f
        };

        /// <summary>
        /// Generates a hexadecimal color string on the format #rrggbb
        /// </summary>
        public static string ToHexString(Color color)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}",
                color.R, color.G, color.B);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HSL color parameters and will compensate 
        /// the lightness for hues that appear to be darker than others.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1]</param>
        /// <param name="saturation">Saturation in the range [0, 1]</param>
        /// <param name="lightness">Lightness in the range [0, 1]</param>
        public static Color FromHslCompensated(float hue, float saturation, float lightness)
        {
            if (hue < 0) throw new ArgumentOutOfRangeException("hue");
            if (hue > 1) throw new ArgumentOutOfRangeException("hue");

            var lightnessCompensation = LightnessCompensations[(int)(hue * 6 + 0.5f)];
            
            // Adjust the input lightness relative to the compensation
            lightness = lightness < 0.5f ?
                lightness * lightnessCompensation * 2f : 
                lightnessCompensation + (lightness - 0.5f) * (1 - lightnessCompensation) * 2f;

            return Color.FromHsl(hue, saturation, lightness);
        }
    }
}
