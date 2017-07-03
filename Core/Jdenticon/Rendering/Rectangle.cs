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

namespace Jdenticon.Rendering
{
    /// <summary>
    /// The coordinates of a rectangle.
    /// </summary>
    public struct Rectangle
    {
        private readonly int x;
        private readonly int y;
        private readonly int width;
        private readonly int height;

        /// <summary>
        /// Creates an instance of <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="x">The X coordinate of the left edge of the rectangle.</param>
        /// <param name="y">The Y coordinate of the top edge of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets the X coordinate of the left edge of the rectangle.
        /// </summary>
        public int X => x;

        /// <summary>
        /// Gets the Y coordinate of the top edge of the rectangle.
        /// </summary>
        public int Y => y;

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public int Height => height;
    }
}
