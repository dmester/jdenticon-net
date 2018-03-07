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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    /// <summary>
    /// Holds all intersecting edges for each scanline in the rendered viewport.
    /// </summary>
    internal class EdgeTable
    {
        private List<EdgeIntersectionRange>[] scanlines;
        private int width;

        public EdgeTable(int width, int height)
        {
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            scanlines = new List<EdgeIntersectionRange>[height];
            this.width = width;
        }

        class EdgeComparer : IComparer<EdgeIntersectionRange>
        {
            public int Compare(EdgeIntersectionRange x, EdgeIntersectionRange y)
            {
                if (x.FromX < y.FromX)
                {
                    return -1;
                }
                if (x.FromX > y.FromX)
                {
                    return 1;
                }
                return 0;
            }
        }
        
        public void Add(Edge edge)
        {
            int minY, maxY;

            if (edge.From.Y == edge.To.Y)
            {
                // Skip horizontal lines
                return;
            }

            if (edge.From.X >= width && edge.To.X >= width)
            {
                // Skip edges entirely to the right of the viewport
                return;
            }

            // Edges crossing the right side of the viewport need to
            // be clipped.
            if ((edge.From.X > width) ^ (edge.To.X > width))
            {
                var intersectingY = edge.From.Y +
                    (edge.To.Y - edge.From.Y) * (width - edge.From.X) / (edge.To.X - edge.From.X);

                if (edge.From.X > width)
                {
                    // Keep To-point
                    edge = new Edge(edge.PolygonID,
                        new PointF(width, intersectingY), edge.To,
                        edge.Color);
                }
                else
                {
                    // Keep From-point
                    edge = new Edge(edge.PolygonID,
                        edge.From, new PointF(width, intersectingY),
                        edge.Color);
                }
            }

            if (edge.From.Y < 0 && edge.To.Y < 0)
            {
                // Skip edges entirely above the viewport
                return;
            }

            if (edge.From.Y >= scanlines.Length && edge.To.Y >= scanlines.Length)
            {
                // Skip edges entirely below the viewport
                return;
            }

            // Determine lower and upper vertical bounds 
            if (edge.From.Y < edge.To.Y)
            {
                minY = (int)edge.From.Y;
                maxY = (int)(edge.To.Y + 0.996f /* 1/255 */);
            }
            else
            {
                minY = (int)edge.To.Y;
                maxY = (int)(edge.From.Y + 0.996f /* 1/255 */);
            }

            // We only need to add the edge to the visible scanlines
            if (minY < 0)
            {
                minY = 0;
            }
            if (maxY > scanlines.Length)
            {
                maxY = scanlines.Length;
            }

            var y = minY;
            var x1 = edge.Intersection(y);

            while (y < maxY)
            {
                var x2 = edge.Intersection(y + 1);

                int fromX, superSampleWidth;
                if (x1 < x2)
                {
                    fromX = (int)x1;
                    superSampleWidth = (int)(x2 + 0.9999f) - fromX;
                }
                else
                {
                    fromX = (int)x2;
                    superSampleWidth = (int)(x1 + 0.9999f) - fromX;
                }

                if (fromX < 0)
                {
                    superSampleWidth += fromX;
                    fromX = 0;

                    if (superSampleWidth < 0)
                    {
                        superSampleWidth = 0;
                    }
                }

                if (fromX + superSampleWidth > width)
                {
                    superSampleWidth = width - fromX;
                }

                var scanline = scanlines[y];
                if (scanline == null)
                {
                    scanlines[y] = scanline = new List<EdgeIntersectionRange>();
                }
                scanline.Add(new EdgeIntersectionRange
                {
                    FromX = fromX,
                    Width = superSampleWidth,
                    Edge = edge
                });

                x1 = x2;
                y++;
            }
        }

        public void Sort()
        {
            var comparer = new EdgeComparer();

            for (var i = 0; i < scanlines.Length; i++)
            {
                scanlines[i]?.Sort(comparer);
            }
        }

        public IList<EdgeIntersectionRange> this[int y]
        {
            get { return scanlines[y] ?? (IList<EdgeIntersectionRange>)new EdgeIntersectionRange[0]; }
        }
    }
}
