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
using System.Globalization;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Represents a SVG path element.
    /// </summary>
    internal class SvgPath
    {
        private readonly List<string> dataString = new List<string>();

        /// <summary>
        /// Adds a circle to the SVG.
        /// </summary>
        public void AddCircle(PointF location, float diameter, bool counterClockwise)
        {
            var sweepFlag = counterClockwise ? '0' : '1';
            var radius = diameter / 2;
            var invariant = CultureInfo.InvariantCulture;
            var radiusAsString = radius.ToString(invariant);

            dataString.Add(
                "M" + (location.X).ToString(invariant) + " " + (location.Y + radius).ToString(invariant) +
                "a" + radiusAsString + "," + radiusAsString + " 0 1," + sweepFlag + " " + diameter.ToString(invariant) + ",0" +
                "a" + radiusAsString + "," + radiusAsString + " 0 1," + sweepFlag + " " + (-diameter).ToString(invariant) + ",0");
        }

        /// <summary>
        /// Adds a polygon to the SVG.
        /// </summary>
        public void AddPolygon(PointF[] points)
        {
            var invariant = CultureInfo.InvariantCulture;

            dataString.Add("M" + points[0].X.ToString(invariant) + " " + points[0].Y.ToString(invariant));

            for (var i = 1; i < points.Length; i++)
            {
                dataString.Add("L" + points[i].X.ToString(invariant) + " " + points[i].Y.ToString(invariant));
            }

            dataString.Add("Z");
        }

        /// <summary>
        /// Gets the path as a SVG path string.
        /// </summary>
        public override string ToString()
        {
            return string.Concat(dataString);
        }
    }
}
