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
using System.Diagnostics;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    /// <summary>
    /// Specifies a segment of a scanline that will be rendered using super
    /// sampling.
    /// </summary>
    [DebuggerDisplay("{FromX} {Width}")]
    internal struct SuperSampleRange
    {
        private Buffer<Edge> edges;
        private int fromX;
        private int toXExcl;

        public int FromX => fromX;
        public int Width => toXExcl - fromX;
        public int ToXExcl => toXExcl;

        public void Add(EdgeIntersectionRange range)
        {
            if (edges.Count == 0)
            {
                fromX = range.FromX;
            }

            var rangeToXExcl = range.FromX + range.Width;
            if (toXExcl < rangeToXExcl)
            {
                toXExcl = rangeToXExcl;
            }

            edges.Add(range.Edge);
        }

        public void Initialize()
        {
            edges.Initialize();
        }

        public void Clear()
        {
            toXExcl = 0;
            edges.Clear();
        }

        public void GetIntersections(ref IntersectionList intersections, float y)
        {
            intersections.Clear();

            for (var i = 0; i < edges.Count; i++)
            {
                ref var edge = ref edges[i];

                if (edge.From.Y < y && edge.To.Y >= y ||
                    edge.From.Y >= y && edge.To.Y < y)
                {
                    var x = edge.From.X +
                        (edge.To.X - edge.From.X) * (y - edge.From.Y) /
                        (edge.To.Y - edge.From.Y);

                    intersections.Add(edge, x);
                }
            }
        }
    }
}
