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

namespace Jdenticon.Drawing.Rasterization
{
    internal static class PolygonFactory
    {
        public static PointF[] FromCircle(PointF center, float radius, bool clockwise)
        {
            const int ArcLengthPixels = 3;

            int sectors = (int)(((float)Math.PI * radius * 2) / ArcLengthPixels);
            if (sectors < 4)
            {
                sectors = 4;
            }

            var points = new PointF[sectors];

            var sectorAngle = Math.PI * 2 / sectors;
            if (clockwise)
            {
                sectorAngle = -sectorAngle;
            }

            for (var i = 0; i < sectors; i++)
            {
                var angle = sectorAngle * i;
                var dx = (float)Math.Sin(angle) * radius;
                var dy = (float)Math.Cos(angle) * radius;
                points[i] = new PointF(
                    center.X + dx,
                    center.Y + dy
                    );
            }
            
            return points;
        }
    }
}
