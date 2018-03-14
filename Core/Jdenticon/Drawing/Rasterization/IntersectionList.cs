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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    internal struct IntersectionList 
    {
        private Intersection[] intersections;
        private int count;

        public ref Intersection this[int index] => ref intersections[index];

        public int Count => count;

        public void Initialize(int capacity)
        {
            if (intersections == null ||
                intersections.Length < capacity)
            {
                intersections = new Intersection[capacity];
            }
        }

        public void Clear()
        {
            count = 0;
        }

        public void Add(Edge edge, float x)
        {
            for (var insertIndex = count - 1; insertIndex >= 0; insertIndex--)
            {
                if (intersections[insertIndex].X < x)
                {
                    ArrayUtils.ShiftRight(intersections, insertIndex + 1, count - insertIndex - 1);
                    intersections[insertIndex + 1] = new Intersection(edge, x);
                    count++;
                    return;
                }
            }

            ArrayUtils.ShiftRight(intersections, 0, count);
            intersections[0] = new Intersection(edge, x);
            count++;
        }
    }
}
