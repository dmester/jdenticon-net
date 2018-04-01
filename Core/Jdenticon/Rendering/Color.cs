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
using System.ComponentModel;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Represents a 24-bit color with a 8-bit alpha channel.
    /// </summary>
    public partial struct Color : IFormattable, IEquatable<Color>
    {
        #region Fields

        // Stored as RGBA
        private uint value;

        #endregion

        #region Constructors

        // Users of the struct should use the static factory methods to create Color value.

        private Color(int a, int r, int g, int b)
        {
            value =
                (((uint)r) << 24) |
                (((uint)g) << 16) |
                (((uint)b) << 8) |
                (((uint)a));
        }

        private Color(uint rgba)
        {
            value = rgba;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the red component of this color in the range [0,255].
        /// </summary>
        public int R
        {
            get { return (int)(value >> 24); }
        }

        /// <summary>
        /// Gets the green component of this color in the range [0,255].
        /// </summary>
        public int G
        {
            get { return (int)((value >> 16) & 0xff); }
        }

        /// <summary>
        /// Gets the blue component of this color in the range [0,255].
        /// </summary>
        public int B
        {
            get { return (int)((value >> 8) & 0xff); }
        }

        /// <summary>
        /// Gets the alpha channel value of this color in the range [0,255] where 0 is fully transparent.
        /// </summary>
        public int A
        {
            get { return (int)(value & 0xff); }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets the 32-bit RGBA value of this color.
        /// </summary>
        public uint ToRgba()
        {
            return value;
        }

        /// <summary>
        /// Gets a hexadecimal representation of this color on the format #rrggbbaa.
        /// </summary>
        public override string ToString()
        {
            return "#" + value.ToString("x8");
        }

        /// <summary>
        /// Gets a string representation of this color on the specified format. The following decimal
        /// placeholders are recognized: R, G, B, A. The following hexadecimal placeholders are
        /// recognized: RR, GG, BB, AA, rr, gg, bb, aa, where the lower case keywords produces 
        /// lower case hex strings.
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Gets a string representation of this color on the specified format. The following decimal
        /// placeholders are recognized: R, G, B, A. The following hexadecimal placeholders are
        /// recognized: RR, GG, BB, AA, rr, gg, bb, aa, where the lower case keywords produces 
        /// lower case hex strings.
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null) throw new ArgumentNullException(nameof(format));

            var sb = new StringBuilder(format.Length * 3);
            var keywords = new char[] { 'R', 'G', 'B', 'A', 'r', 'g', 'b', 'a' };

            var formatCursor = 0;

            while (formatCursor < format.Length)
            {
                var nextIndex = format.IndexOfAny(keywords, formatCursor);
                if (nextIndex >= 0)
                {
                    // Add substring preceding placeholder
                    sb.Append(format, formatCursor, nextIndex - formatCursor);
                    formatCursor = nextIndex + 1;

                    // Process placeholder
                    int value;
                    bool isUpperCase;

                    switch (format[nextIndex])
                    {
                        case 'R': value = R; isUpperCase = true; break;
                        case 'G': value = G; isUpperCase = true; break;
                        case 'B': value = B; isUpperCase = true; break;
                        case 'A': value = A; isUpperCase = true; break;
                        case 'r': value = R; isUpperCase = false; break;
                        case 'g': value = G; isUpperCase = false; break;
                        case 'b': value = B; isUpperCase = false; break;
                        case 'a': value = A; isUpperCase = false; break;
                        default: throw new Exception("Unknown placeholder."); // << this should not happen
                    }

                    var isHexPlaceholder =
                        nextIndex + 1 < format.Length &&
                        format[nextIndex] == format[nextIndex + 1];

                    if (isHexPlaceholder)
                    {
                        formatCursor++;
                        sb.Append(value.ToString(isUpperCase ? "X2" : "x2", formatProvider));
                    }
                    else if (isUpperCase)
                    {
                        sb.Append(value.ToString(formatProvider));
                    }
                    else
                    {
                        // Not a placeholder
                        sb.Append(format[nextIndex]);
                    }
                }
                else
                {
                    break;
                }
            }

            // End of string
            if (formatCursor < format.Length)
            {
                sb.Append(format, formatCursor, format.Length - formatCursor);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the RGBA value which serves as hash for this color.
        /// </summary>
        public override int GetHashCode()
        {
            return unchecked((int)value);
        }

        /// <summary>
        /// Checks if this color has the same RGBA value as another color.
        /// </summary>
        /// <param name="obj">The color to compare.</param>
        public override bool Equals(object obj)
        {
            return obj is Color && Equals((Color)obj);
        }

        /// <summary>
        /// Checks if this color has the same RGBA value as another color.
        /// </summary>
        /// <param name="other">The color to compare.</param>
        public bool Equals(Color other)
        {
            return other.value == value;
        }

        /// <summary>
        /// Checks if the two <see cref="Color"/> have the same RGBA value.
        /// </summary>
        /// <param name="a">The first color to compare.</param>
        /// <param name="b">The second color to compare.</param>
        public static bool operator ==(Color a, Color b)
        {
            return a.value == b.value;
        }

        /// <summary>
        /// Checks if the two <see cref="Color"/> have different RGBA value.
        /// </summary>
        /// <param name="a">The first color to compare.</param>
        /// <param name="b">The second color to compare.</param>
        public static bool operator !=(Color a, Color b)
        {
            return a.value != b.value;
        }

        #endregion
    }
}
