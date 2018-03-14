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
