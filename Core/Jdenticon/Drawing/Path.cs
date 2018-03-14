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
    internal class Path
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
