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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon
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

            return FromHsl(hue, saturation, lightness);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HSL color parameters.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1]</param>
        /// <param name="saturation">Saturation in the range [0, 1]</param>
        /// <param name="lightness">Lightness in the range [0, 1]</param>
        public static Color FromHsl(float hue, float saturation, float lightness)
        {
            if (hue < 0) throw new ArgumentOutOfRangeException("hue");
            else if (hue > 1) throw new ArgumentOutOfRangeException("hue");

            if (saturation < 0) throw new ArgumentOutOfRangeException("saturation");
            else if (saturation > 1) throw new ArgumentOutOfRangeException("saturation");

            if (lightness < 0) throw new ArgumentOutOfRangeException("lightness");
            else if (lightness > 1) throw new ArgumentOutOfRangeException("lightness");
            
            // Based on http://www.w3.org/TR/2011/REC-css3-color-20110607/#hsl-color
            if (saturation == 0)
            {
                var value = (int)(lightness * 255);
                return Color.FromArgb(255, value, value, value);
            }
            else
            {
                var m2 = lightness <= 0.5f ? lightness * (saturation + 1) : lightness + saturation - lightness * saturation;
                var m1 = lightness * 2 - m2;

                return Color.FromArgb(255,
                    HueToRgb(m1, m2, hue * 6 + 2),
                    HueToRgb(m1, m2, hue * 6),
                    HueToRgb(m1, m2, hue * 6 - 2));
            }
        }
        
        // Helper method for FromHsl
        private static int HueToRgb(float m1, float m2, float h)
        {
            h = h < 0 ? h + 6 : h > 6 ? h - 6 : h;
            return (int)(255 * (
                h < 1 ? m1 + (m2 - m1) * h :
                h < 3 ? m2 :
                h < 4 ? m1 + (m2 - m1) * (4 - h) :
                m1));
        }
    }
}
