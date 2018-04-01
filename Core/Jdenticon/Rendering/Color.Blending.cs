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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Jdenticon.Rendering
{
    partial struct Color
    {
        #region Methods
        
        /// <summary>
        /// Computes a mix of the two specified colors, with the proportion given by the specified weight.
        /// </summary>
        /// <param name="color1">First color to mix.</param>
        /// <param name="color2">Second color to mix.</param>
        /// <param name="weight">Weight in the range [0,1]. 0 gives <paramref name="color1"/>, 1 gives <paramref name="color2"/>.</param>
        public static Color Mix(Color color1, Color color2, float weight)
        {
            // Convert weight to an integer value to avoid rounding errors on the RGB components.
            // 8 bits goes to the RGB component, 8 bits to the multiplied alpha.
            // Use the remaining 16 bits of the integer, for the alpha multiplier to
            // maximize accuracy, but skip the first bit to avoid overflows to negative
            // integers.
            const int AlphaBits = 15;
            const int AlphaMultiplier = 1 << AlphaBits;

            var iweight = (int)(AlphaMultiplier * weight);

            if (iweight < 0)
            {
                iweight = 0;
            }
            else if (iweight > AlphaMultiplier)
            {
                iweight = AlphaMultiplier;
            }

            var alphaSum = color1.A * (AlphaMultiplier - iweight) + color2.A * iweight;
            if (alphaSum == 0)
            {
                return Transparent;
            }

            return new Color(
                alphaSum >> AlphaBits,
                (color1.R * color1.A * (AlphaMultiplier - iweight) + color2.R * color2.A * iweight) / alphaSum,
                (color1.G * color1.A * (AlphaMultiplier - iweight) + color2.G * color2.A * iweight) / alphaSum,
                (color1.B * color1.A * (AlphaMultiplier - iweight) + color2.B * color2.A * iweight) / alphaSum);
        }
        
        /// <summary>
        /// Blends this color with another color using the over blending operation.
        /// </summary>
        /// <param name="background">The background color.</param>
        public Color Over(Color background)
        {
            var foreA = A;

            if (foreA < 1)
            {
                return background;
            }
            else if (foreA > 254 || background.A < 1)
            {
                return this;
            }

            // Source: https://en.wikipedia.org/wiki/Alpha_compositing#Description
            var forePA = foreA * 255;
            var backPA = background.A * (255 - foreA);
            var alpha = (forePA + backPA);

            var b = (byte)(
                (forePA * B + backPA * background.B) /
                alpha);

            var g = (byte)(
                (forePA * G + backPA * background.G) /
                alpha);

            var r = (byte)(
                (forePA * R + backPA * background.R) /
                alpha);

            var a = (byte)(alpha / 255);

            return new Color(a, r, g, b);
        }
        
        #endregion
    }
}
