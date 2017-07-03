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
