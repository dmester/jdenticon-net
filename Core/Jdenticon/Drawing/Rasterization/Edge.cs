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
    /// <summary>
    /// Specifies an edge of a polygon that is being rendered.
    /// </summary>
    internal struct Edge
    {
        public Edge(int polygonId, PointF from, PointF to, Color color)
        {
            PolygonID = polygonId;
            From = from;
            To = to;
            Color = color;
        }

        public PointF From { get; private set; }
        public PointF To { get; private set; }

        public int PolygonID { get; private set; }
        public Color Color { get; private set; }

        public override string ToString()
        {
            return PolygonID + ": " + From + "; " + To;
        }

        public float Intersection(float y)
        {
            var dx = (To.X - From.X) * (From.Y - y) / (From.Y - To.Y);
            return From.X + dx;
        }
    }
}
