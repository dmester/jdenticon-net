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

        private static string FormatCoordinate(float coordinate)
        {
            // Round to a single decimal. Done for two reasons:
            // * Smaller output. For some icons the SVG is as much as 15% smaller with rounding.
            // * More deterministic output. Without specifying a format string, the produced output could
            //   vary on different machines.
            return coordinate.ToString("0.#", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Adds a circle to the SVG.
        /// </summary>
        public void AddCircle(PointF location, float diameter, bool counterClockwise)
        {
            var sweepFlag = counterClockwise ? '0' : '1';
            var radius = diameter / 2;
            var radiusAsString = FormatCoordinate(radius);

            dataString.Add(
                "M" + FormatCoordinate(location.X) + " " + FormatCoordinate(location.Y + radius) +
                "a" + radiusAsString + "," + radiusAsString + " 0 1," + sweepFlag + " " + FormatCoordinate(diameter) + ",0" +
                "a" + radiusAsString + "," + radiusAsString + " 0 1," + sweepFlag + " " + FormatCoordinate(-diameter) + ",0");
        }

        /// <summary>
        /// Adds a polygon to the SVG.
        /// </summary>
        public void AddPolygon(PointF[] points)
        {
            dataString.Add("M" + FormatCoordinate(points[0].X) + " " + FormatCoordinate(points[0].Y));

            for (var i = 1; i < points.Length; i++)
            {
                dataString.Add("L" + FormatCoordinate(points[i].X) + " " + FormatCoordinate(points[i].Y));
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
