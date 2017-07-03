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
using System.Text;

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

        public override string ToString()
        {
            return "{ " + x + ", " + y + " }";
        }

        public override int GetHashCode()
        {
            return x ^ y;
        }

        public override bool Equals(object obj)
        {
            return obj is ShapePosition && Equals((ShapePosition)obj);
        }

        public bool Equals(ShapePosition other)
        {
            return other.x == x && other.y == y;
        }

        public static bool operator ==(ShapePosition a, ShapePosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ShapePosition a, ShapePosition b)
        {
            return !a.Equals(b);
        }
    }
}
