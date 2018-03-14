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
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Specifies the colors to be used in an identicon.
    /// </summary>
    public class ColorTheme
    {
        /// <summary>
        /// Creates an instance of <see cref="ColorTheme"/>.
        /// </summary>
        /// <param name="hue">The hue of the colored shapes.</param>
        /// <param name="style">The style that specifies the lightness and saturation of the icon.</param>
        public ColorTheme(float hue, IdenticonStyle style)
        {
            DarkGray = Color.FromHsl(0, 0, style.GrayscaleLightness.From);
            MidColor = ColorUtils.FromHslCompensated(hue, style.Saturation, (style.ColorLightness.From + style.ColorLightness.To) / 2);
            LightGray = Color.FromHsl(0, 0, style.GrayscaleLightness.To);
            LightColor = ColorUtils.FromHslCompensated(hue, style.Saturation, style.ColorLightness.To);
            DarkColor = ColorUtils.FromHslCompensated(hue, style.Saturation, style.ColorLightness.From);
        }

        /// <summary>
        /// Gets or sets the dark gray color of the icon.
        /// </summary>
        public Color DarkGray { get; set; }

        /// <summary>
        /// Gets or sets the mid-lightness color of the icon.
        /// </summary>
        public Color MidColor { get; set; }

        /// <summary>
        /// Gets or sets the light gray color of the icon.
        /// </summary>
        public Color LightGray { get; set; }

        /// <summary>
        /// Gets or sets the high-lightness color of the icon.
        /// </summary>
        public Color LightColor { get; set; }

        /// <summary>
        /// Gets or sets the low-lightness color of the icon.
        /// </summary>
        public Color DarkColor { get; set; }

        /// <summary>
        /// Gets a color from this color theme by index.
        /// </summary>
        public Color this[int index]
        {
            get
            {
                if (index == 0) return DarkGray;
                if (index == 1) return MidColor;
                if (index == 2) return LightGray;
                if (index == 3) return LightColor;
                if (index == 4) return DarkColor;
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Gets the number of colors in this theme.
        /// </summary>
        public int Count
        {
            get { return 5; }
        }
    }
}
