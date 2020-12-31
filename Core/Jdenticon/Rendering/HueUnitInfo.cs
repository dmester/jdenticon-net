#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2020
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
using System.Text;

namespace Jdenticon.Rendering
{
    internal class HueUnitInfo
    {
        public HueUnitInfo(HueUnit key, string suffix, float period)
        {
            Key = key;
            Suffix = suffix;
            Period = period;
        }

        /// <summary>
        /// Hue unit.
        /// </summary>
        public HueUnit Key { get; }

        /// <summary>
        /// Textual suffix that can be specified after the hue value, e.g. 23rad, to specify the unit.
        /// </summary>
        public string Suffix { get; }

        /// <summary>
        /// The maximum value of the unit, until it starts over.
        /// </summary>
        public float Period { get; }

        /// <summary>
        /// Information about known units.
        /// </summary>
        public static HueUnitInfo[] Values { get; } = new[]
        {
            new HueUnitInfo(HueUnit.Turns, "turn", 1f),
            new HueUnitInfo(HueUnit.Gradians, "grad", 400f),
            new HueUnitInfo(HueUnit.Degrees, "deg", 360f),
            new HueUnitInfo(HueUnit.Radians, "rad", (float)(2 * Math.PI)),
        };

        /// <summary>
        /// Gets information about a specific unit.
        /// </summary>
        /// <exception cref="ArgumentException">Unknown <paramref name="hueUnit"/>.</exception>
        public static HueUnitInfo Get(HueUnit hueUnit)
        {
            foreach (var unitInfo in Values)
            {
                if (unitInfo.Key == hueUnit)
                {
                    return unitInfo;
                }
            }

            throw new ArgumentException($"Unknown hue unit {hueUnit}.", nameof(hueUnit));
        }
    }
}
