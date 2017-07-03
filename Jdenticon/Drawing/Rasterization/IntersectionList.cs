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
