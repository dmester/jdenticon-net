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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Rendering.Svg
{
    /// <summary>
    /// Represents a SVG path element.
    /// </summary>
    public class SvgPath
    {
        List<string> dataString = new List<string>();

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

        public override string ToString()
        {
            return string.Concat(dataString);
        }
    }
}
