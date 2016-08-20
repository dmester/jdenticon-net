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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
