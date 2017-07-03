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
using System.Linq;
using System.Text;
using JdenticonColor = Jdenticon.Rendering.Color;
using WpfColor = System.Windows.Media.Color;

namespace Jdenticon.Rendering.Wpf
{
    internal static class WpfExtensions
    {
        public static WpfColor ToWpfColor(this JdenticonColor color)
        {
            return WpfColor.FromArgb((byte)color.A, (byte)color.R, (byte)color.G, (byte)color.B);
        }

        public static Rectangle ToJdenticon(this System.Windows.Rect rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}
