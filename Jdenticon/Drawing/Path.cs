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

using Jdenticon.Drawing.Rasterization;
using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing
{
    /// <summary>
    /// A set of polygons and circles that will be rendered as a single entity.
    /// </summary>
    public class Path
    {
        private Buffer<PointF[]> polygons = new Buffer<PointF[]>(10);

        /// <summary>
        /// Adds a polygon to the path.
        /// </summary>
        /// <param name="points">The location of all corners of the polygon. The polygon is rendered using the non-zero winding rule.</param>
        public void AddPolygon(PointF[] points)
        {
            polygons.Add(points);
        }

        /// <summary>
        /// Adds a circle to the path.
        /// </summary>
        /// <param name="center">The location of the center of the circle.</param>
        /// <param name="radius">The radius of the circle in pixels.</param>
        /// <param name="clockwise">Specifies if the circle will be drawn clockwise. The direction might affect the result since the shapes are rendered using the non-zero winding rule.</param>
        public void AddCircle(PointF center, float radius, bool clockwise)
        {
            polygons.Add(PolygonFactory.FromCircle(center, radius, clockwise));
        }

        internal IEnumerable<PointF[]> Polygons => polygons;
    }
}
