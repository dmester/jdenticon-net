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

        public int Count => edges.Count;

        public Edge this[int index] => edges[index];

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
