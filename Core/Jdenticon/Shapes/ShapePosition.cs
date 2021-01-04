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
using System.Text;

#nullable enable

namespace Jdenticon.Shapes
{
    /// <summary>
    /// Specifies in which cell a shape will be rendered.
    /// </summary>
    public struct ShapePosition : IEquatable<ShapePosition>
    {
        int x, y;

        /// <summary>
        /// Creates a new <see cref="ShapePosition"/> instance.
        /// </summary>
        /// <param name="x">The x-coordinate of the containing cell.</param>
        /// <param name="y">The y-coordinate of the containing cell.</param>
        public ShapePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// The x-coordinate of the containing cell.
        /// </summary>
        public int X
        {
            get { return x; }
        }

        /// <summary>
        /// The y-coordinate of the containing cell.
        /// </summary>
        public int Y
        {
            get { return y; }
        }

        /// <summary>
        /// Gets a string representation of the <see cref="ShapePosition"/> instance.
        /// </summary>
        public override string ToString()
        {
            return "{ " + x + ", " + y + " }";
        }

        /// <summary>
        /// Gets a hash code for this <see cref="ShapePosition"/> instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x ^ y;
        }

        /// <summary>
        /// Checks if this position is equal to another position.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        public override bool Equals(object? obj)
        {
            return obj is ShapePosition && Equals((ShapePosition)obj);
        }

        /// <summary>
        /// Checks if this position is equal to another position.
        /// </summary>
        /// <param name="other">The object to compare with.</param>
        public bool Equals(ShapePosition other)
        {
            return other.x == x && other.y == y;
        }

        /// <summary>
        /// Checks if two <see cref="ShapePosition"/> instances are equal.
        /// </summary>
        /// <param name="a">The first <see cref="ShapePosition"/> instance to compare.</param>
        /// <param name="b">The second <see cref="ShapePosition"/> instance to compare.</param>
        public static bool operator ==(ShapePosition a, ShapePosition b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Checks if two <see cref="ShapePosition"/> instances are not equal.
        /// </summary>
        /// <param name="a">The first <see cref="ShapePosition"/> instance to compare.</param>
        /// <param name="b">The second <see cref="ShapePosition"/> instance to compare.</param>
        public static bool operator !=(ShapePosition a, ShapePosition b)
        {
            return !a.Equals(b);
        }
    }
}
