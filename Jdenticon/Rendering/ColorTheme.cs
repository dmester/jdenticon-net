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

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Specifies the colors to be used in an identicon.
    /// </summary>
    public class ColorTheme
    {
        public ColorTheme(float hue, IdenticonStyle style)
        {
            DarkGray = ColorUtils.FromHsl(0, 0, style.GrayscaleLightness.From);
            MidColor = ColorUtils.FromHslCompensated(hue, style.Saturation, (style.ColorLightness.From + style.ColorLightness.To) / 2);
            LightGray = ColorUtils.FromHsl(0, 0, style.GrayscaleLightness.To);
            LightColor = ColorUtils.FromHslCompensated(hue, style.Saturation, style.ColorLightness.To);
            DarkColor = ColorUtils.FromHslCompensated(hue, style.Saturation, style.ColorLightness.From);
        }

        public Color DarkGray { get; set; }

        public Color MidColor { get; set; }

        public Color LightGray { get; set; }

        public Color LightColor { get; set; }

        public Color DarkColor { get; set; }

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

        public int Count
        {
            get { return 5; }
        }
    }
}
