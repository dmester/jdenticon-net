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
