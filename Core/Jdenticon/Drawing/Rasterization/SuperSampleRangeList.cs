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
                superSampleRangeCount = 0;
                return;
            }

            var rangeIndex = 0;
            var superSampleRangeIndex = 0;

            // Initialize empty list
            if (superSampleRanges == null)
            {
                superSampleRanges = new SuperSampleRange[10];

                for (var i = 0; i < superSampleRanges.Length; i++)
                {
                    superSampleRanges[i].Initialize();
                }
            }

            do
            {
                // Ensure capacity
                if (superSampleRangeIndex >= superSampleRanges.Length)
                {
                    var oldSuperSampleRanges = superSampleRanges;
                    superSampleRanges = new SuperSampleRange[oldSuperSampleRanges.Length * 2];

                    Array.Copy(oldSuperSampleRanges, superSampleRanges, oldSuperSampleRanges.Length);

                    for (var i = oldSuperSampleRanges.Length; i < superSampleRanges.Length; i++)
                    {
                        superSampleRanges[i].Initialize();
                    }
                }

                ref var superSampleRange = ref superSampleRanges[superSampleRangeIndex];
                superSampleRange.Clear();
                superSampleRangeIndex++;

                superSampleRange.Add(ranges[rangeIndex]);
                rangeIndex++;

                for (var i = rangeIndex; i < ranges.Count; i++)
                {
                    // Don't merge adjacent non-overlapping ranges. This improves the chances
                    // that we can use the integration algorithm during the rasterization,
                    // which performs better than supersampling.
                    if (ranges[i].FromX < superSampleRange.ToXExcl)
                    {
                        superSampleRange.Add(ranges[i]);
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
