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
    /// Translates and rotates a point before being rendered.
    /// </summary>
    internal class Transform
    {
        private int x;
        private int y;
        private int size;
        private int rotation;

        private static readonly Transform empty = new Transform(0, 0, 0, 0);

        /// <summary>
        /// Creates a new <see cref="Transform"/> instance.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper left corner of the transformed rectangle.</param>
        /// <param name="y">The y-coordinate of the upper left corner of the transformed rectangle.</param>
        /// <param name="size">The size of the transformed rectangle.</param>
        /// <param name="rotation">Rotation specified as 0 = 0 rad, 1 = 0.5π rad, 2 = π rad, 3 = 1.5π rad.</param>
        public Transform(int x, int y, int size, int rotation)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.rotation = rotation;
        }

        /// <summary>
        /// Gets a <see cref="Transform"/> instance that does no transform.
        /// </summary>
        public static Transform Empty
        {
            get { return empty; }
        }

        /// <summary>
        /// Transforms the specified point based on the translation and rotation specification for this Transform.
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="width">The width of the transformed rectangle. If greater than 0, this will ensure the returned point is of the upper left corner of the transformed rectangle.</param>
        /// <param name="height">The height of the transformed rectangle. If greater than 0, this will ensure the returned point is of the upper left corner of the transformed rectangle.</param>
        /// <returns></returns>
        public PointF TransformPoint(float x, float y, float width = 0, float height = 0)
        {
            var right = this.x + this.size;
            var bottom = this.y + this.size;

            return this.rotation == 1 ? new PointF(right - y - height, this.y + x) :
                   this.rotation == 2 ? new PointF(right - x - width, bottom - y - height) :
                   this.rotation == 3 ? new PointF(this.x + y, bottom - x - width) :
                   new PointF(this.x + x, this.y + y);
        }
    }
}
