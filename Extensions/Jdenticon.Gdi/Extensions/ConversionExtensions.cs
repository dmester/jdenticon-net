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
