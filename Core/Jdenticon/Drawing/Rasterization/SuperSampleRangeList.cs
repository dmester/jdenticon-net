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

namespace Jdenticon.Drawing.Rasterization
{
    internal struct SuperSampleRangeList
    {
        private SuperSampleRange[] superSampleRanges;
        private int superSampleRangeCount;

        public int Count => superSampleRangeCount;

        public ref SuperSampleRange this[int index] => ref superSampleRanges[index];


        public void Populate(IList<EdgeIntersectionRange> ranges)
        {
            if (ranges.Count == 0)
            {
                return;
            }

            const int AllowedRangeDistance = 1;
            var rangeIndex = 0;
            var superSampleRangeIndex = 0;

            do
            {
                if (superSampleRanges == null)
                {
                    superSampleRanges = new SuperSampleRange[10];

                    for (var i = 0; i < superSampleRanges.Length; i++)
                    {
                        superSampleRanges[i].Initialize();
                    }
                }
                else if (superSampleRangeIndex >= superSampleRanges.Length)
                {
                    var oldSuperSampleRanges = superSampleRanges;
                    superSampleRanges = new SuperSampleRange[oldSuperSampleRanges.Length * 2];

                    Array.Copy(oldSuperSampleRanges, superSampleRanges, oldSuperSampleRanges.Length);

                    for (var i = oldSuperSampleRanges.Length; i < superSampleRanges.Length; i++)
                    {
                        superSampleRanges[i].Initialize();
                    }
                }

                ref var msr = ref superSampleRanges[superSampleRangeIndex];
                msr.Clear();
                superSampleRangeIndex++;

                msr.Add(ranges[rangeIndex]);
                rangeIndex++;

                for (var i = rangeIndex; i < ranges.Count; i++)
                {
                    if (ranges[i].FromX <= msr.ToXExcl + AllowedRangeDistance)
                    {
                        msr.Add(ranges[i]);
                        rangeIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            while (rangeIndex < ranges.Count);

            superSampleRangeCount = superSampleRangeIndex;
        }
    }
}
