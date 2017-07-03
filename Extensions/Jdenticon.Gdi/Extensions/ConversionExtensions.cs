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

namespace Jdenticon.Gdi.Extensions
{
    /// <summary>
    /// Extension methods for converting structs between GDI and Jdenticon.
    /// </summary>
    internal static class ConversionExtensions
    {
        public static Rendering.Rectangle ToJdenticon(this System.Drawing.Rectangle rect)
        {
            return new Rendering.Rectangle(
                rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rendering.PointF ToJdenticon(this System.Drawing.PointF point)
        {
            return new Rendering.PointF(point.X, point.Y);
        }

        public static System.Drawing.PointF ToGdi(this Rendering.PointF point)
        {
            return new System.Drawing.PointF(point.X, point.Y);
        }

        public static System.Drawing.PointF[] ToGdi(this Rendering.PointF[] points)
        {
            var translated = new System.Drawing.PointF[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                translated[i] = points[i].ToGdi();
            }
            return translated;
        }

        public static System.Drawing.Color ToGdi(this Rendering.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
