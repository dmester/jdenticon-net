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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable enable

namespace Jdenticon.Rendering
{
    partial struct Color
    {
        #region Methods

        /// <summary>
        /// Creates a <see cref="Color"/> from ARGB color components.
        /// </summary>
        /// <param name="alpha">Alpha channel value in the range [0,255].</param>
        /// <param name="red">Red component in the range [0,255].</param>
        /// <param name="green">Green component in the range [0,255].</param>
        /// <param name="blue">Blue component in the range [0,255].</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was less than 0 or greater than 255.</exception>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            return FromRgba(red, green, blue, alpha);
        }

        /// <summary>
        /// Creates a fully opaque <see cref="Color"/> from RGB color components.
        /// </summary>
        /// <param name="red">Red component in the range [0,255].</param>
        /// <param name="green">Green component in the range [0,255].</param>
        /// <param name="blue">Blue component in the range [0,255].</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was less than 0 or greater than 255.</exception>
        public static Color FromRgb(int red, int green, int blue)
        {
            return FromRgba(red, green, blue, 255);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> from RGBA color components.
        /// </summary>
        /// <param name="red">Red component in the range [0,255].</param>
        /// <param name="green">Green component in the range [0,255].</param>
        /// <param name="blue">Blue component in the range [0,255].</param>
        /// <param name="alpha">Alpha channel value in the range [0,255].</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was less than 0 or greater than 255.</exception>
        public static Color FromRgba(int red, int green, int blue, int alpha)
        {
            if (red < 0 || red > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(red), $"Value {red} is not a valid value of {nameof(red)}. Allowed values are in the range [0, 255].");
            }
            if (green < 0 || green > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(green), $"Value {green} is not a valid value of {nameof(green)}. Allowed values are in the range [0, 255].");
            }
            if (blue < 0 || blue > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(blue), $"Value {blue} is not a valid value of {nameof(blue)}. Allowed values are in the range [0, 255].");
            }
            if (alpha < 0 || alpha > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), $"Value {alpha} is not a valid value of {nameof(alpha)}. Allowed values are in the range [0, 255].");
            }

            return new Color(alpha, red, green, blue);
        }
        
        /// <summary>
        /// Creates a <see cref="Color"/> from a 32-bit RGBA value.
        /// </summary>
        /// <param name="rgba">A 0xRRGGBBAA color value.</param>
        public static Color FromRgba(uint rgba)
        {
            return new Color(rgba);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HSL (Hue-Saturation-Lightness) color parameters.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1]</param>
        /// <param name="saturation">Saturation in the range [0, 1]</param>
        /// <param name="lightness">Lightness in the range [0, 1]</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was <see cref="float.NaN"/>, less than 0f or greater than 1f.</exception>
        public static Color FromHsl(float hue, float saturation, float lightness)
        {
            return FromHsl(hue, saturation, lightness, 1f);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HSL (Hue-Saturation-Lightness) color parameters and an alpha value.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1]</param>
        /// <param name="saturation">Saturation in the range [0, 1]</param>
        /// <param name="lightness">Lightness in the range [0, 1]</param>
        /// <param name="alpha">Alpha in the range [0, 1]</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was <see cref="float.NaN"/>, less than 0f or greater than 1f.</exception>
        public static Color FromHsl(float hue, float saturation, float lightness, float alpha)
        {
            if (float.IsNaN(hue)) throw new ArgumentOutOfRangeException(nameof(hue), $"NaN is not a valid {nameof(hue)}. Allowed values are in the range [0, 1].");
            if (hue < 0 || hue > 1) throw new ArgumentOutOfRangeException(nameof(hue), $"Value {hue} is not a valid {nameof(hue)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(saturation)) throw new ArgumentOutOfRangeException(nameof(saturation), $"NaN is not a valid {nameof(saturation)}. Allowed values are in the range [0, 1].");
            if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException(nameof(saturation), $"Value {saturation} is not a valid {nameof(saturation)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(lightness)) throw new ArgumentOutOfRangeException(nameof(lightness), $"NaN is not a valid {nameof(lightness)}. Allowed values are in the range [0, 1].");
            if (lightness < 0 || lightness > 1) throw new ArgumentOutOfRangeException(nameof(lightness), $"Value {lightness} is not a valid {nameof(lightness)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(alpha)) throw new ArgumentOutOfRangeException(nameof(alpha), $"NaN is not a valid value of {nameof(alpha)}. Allowed values are in the range [0, 1].");
            if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException(nameof(alpha), $"Value {alpha} is not a valid value of {nameof(alpha)}. Allowed values are in the range [0, 1].");
            
            // Based on http://www.w3.org/TR/2011/REC-css3-color-20110607/#hsl-color
            if (saturation == 0)
            {
                var value = (int)(lightness * 255);
                return new Color((int)(255 * alpha), value, value, value);
            }
            else
            {
                var m2 = lightness <= 0.5f ? lightness * (saturation + 1) : lightness + saturation - lightness * saturation;
                var m1 = lightness * 2 - m2;
                
                return new Color(
                    (int)(255 * alpha),
                    HueToRgb(m1, m2, hue * 6 + 2),
                    HueToRgb(m1, m2, hue * 6),
                    HueToRgb(m1, m2, hue * 6 - 2));
            }
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HWB (Hue-Whiteness-Blackness) color parameters.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1].</param>
        /// <param name="whiteness">Whiteness in the range [0, 1].</param>
        /// <param name="blackness">Blackness in the range [0, 1].</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was <see cref="float.NaN"/>, less than 0f or greater than 1f.</exception>
        public static Color FromHwb(float hue, float whiteness, float blackness)
        {
            return FromHwb(hue, whiteness, blackness, 1f);
        }

        /// <summary>
        /// Creates a <see cref="Color"/> instance from HWB (Hue-Whiteness-Blackness) color parameters and an alpha value.
        /// </summary>
        /// <param name="hue">Hue in the range [0, 1].</param>
        /// <param name="whiteness">Whiteness in the range [0, 1].</param>
        /// <param name="blackness">Blackness in the range [0, 1].</param>
        /// <param name="alpha">Alpha in the range [0, 1].</param>
        /// <exception cref="ArgumentOutOfRangeException">One of the components was <see cref="float.NaN"/>, less than 0f or greater than 1f.</exception>
        public static Color FromHwb(float hue, float whiteness, float blackness, float alpha)
        {
            if (float.IsNaN(hue)) throw new ArgumentOutOfRangeException(nameof(hue), $"NaN is not a valid {nameof(hue)}. Allowed values are in the range [0, 1].");
            if (hue < 0 || hue > 1) throw new ArgumentOutOfRangeException(nameof(hue), $"Value {hue} is not a valid {nameof(hue)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(whiteness)) throw new ArgumentOutOfRangeException(nameof(whiteness), $"NaN is not a valid value of {nameof(whiteness)}. Allowed values are in the range [0, 1].");
            if (whiteness < 0 || whiteness > 1) throw new ArgumentOutOfRangeException(nameof(whiteness), $"Value {whiteness} is not a valid value of {nameof(whiteness)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(blackness)) throw new ArgumentOutOfRangeException(nameof(blackness), $"NaN is not a valid value of {nameof(blackness)}. Allowed values are in the range [0, 1].");
            if (blackness < 0 || blackness > 1) throw new ArgumentOutOfRangeException(nameof(blackness), $"Value {blackness} is not a valid value of {nameof(blackness)}. Allowed values are in the range [0, 1].");

            if (float.IsNaN(alpha)) throw new ArgumentOutOfRangeException(nameof(alpha), $"NaN is not a valid value of {nameof(alpha)}. Allowed values are in the range [0, 1].");
            if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException(nameof(alpha), $"Value {alpha} is not a valid value of {nameof(alpha)}. Allowed values are in the range [0, 1].");

            // Specification:
            // https://www.w3.org/TR/css-color-4/#hwb-to-rgb

            var rgb = FromHsl(hue, 1, .5f);

            // Sum of white and black must not exceed 100%
            if (whiteness + blackness > 1)
            {
                whiteness = whiteness / (whiteness + blackness);
                blackness = 1 - whiteness;
            }

            var m = 1 - whiteness - blackness;

            var r = (int)(rgb.R * m + whiteness * 255);
            var g = (int)(rgb.G * m + whiteness * 255);
            var b = (int)(rgb.B * m + whiteness * 255);

            return new Color(
                (int)(255 * alpha),
                r < 0 ? 0 : r > 255 ? 255 : r,
                g < 0 ? 0 : g > 255 ? 255 : g,
                b < 0 ? 0 : b > 255 ? 255 : b);
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
        
        #endregion
    }
}
