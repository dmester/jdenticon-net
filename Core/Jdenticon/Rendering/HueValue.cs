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
using System.Globalization;
using System.Text;

namespace Jdenticon.Rendering
{
    internal class HueValue : IEquatable<HueValue>, IFormattable
    {
        private HueValue(string value, IFormatProvider formatProvider)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var suffix = "";
            var period = 1f;

            foreach (var unit in HueUnitInfo.Values)
            {
                if (value.EndsWith(unit.Suffix, StringComparison.OrdinalIgnoreCase))
                {
                    suffix = unit.Suffix;
                    period = unit.Period;
                    value = value.Substring(0, value.Length - unit.Suffix.Length);
                    break;
                }
            }

            var parsedValue = float.Parse(value, NumberStyles.Number, formatProvider);
            
            Suffix = suffix;
            Value = Normalize(parsedValue, period);
            Turns = Value / period;
        }

        public HueValue(float hue, HueUnit hueUnit)
        {
            var info = HueUnitInfo.Get(hueUnit);
            var normalized = Normalize(hue, info.Period);

            Suffix = info.Suffix;
            Turns = normalized / info.Period;
            Value = normalized;
        }

        public HueValue(float turns)
        {
            Value = Turns = Normalize(turns, 1f);
            Suffix = "";
        }

        /// <summary>
        /// Value in the local unit, specified by <see cref="Suffix"/>.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Value in turns [0, 1).
        /// </summary>
        public float Turns { get; }

        /// <summary>
        /// Value unit as displayed to developer.
        /// </summary>
        public string Suffix { get; }

        public static HueValue Parse(string value, IFormatProvider formatProvider)
        {
            return new HueValue(value, formatProvider);
        }

        private static float Normalize(float value, float period)
        {
            value = value % period;
            if (value < 0) value += period;
            return value;
        }

        public override int GetHashCode() => unchecked((int)(1000 * Turns));

        public override bool Equals(object obj) => Equals(obj as HueValue);
        public bool Equals(HueValue other) => other != null && other.Turns == Turns;

        public override string ToString() => ToString(CultureInfo.InvariantCulture);

        public string ToString(IFormatProvider formatProvider) => Value.ToString("0.##", formatProvider) + Suffix;

        string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(formatProvider);
    }
}
