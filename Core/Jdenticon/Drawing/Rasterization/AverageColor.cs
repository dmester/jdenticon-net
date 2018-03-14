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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    /// <summary>
    /// Computes an average color of a set of color samples.
    /// </summary>
    internal struct AverageColor
    {
        ulong argbc;

        public const int MaxSampleCount = 63;
        
        /// <summary>
        /// Removes all samples from this <see cref="AverageColor"/>.
        /// </summary>
        public void Clear()
        {
            argbc = 0;
        }

        /// <summary>
        /// Gets the number of colors in this <see cref="AverageColor"/>.
        /// </summary>
        public int Count
        {
            get { return (int)(argbc & 0x3fLu); }
        }

        /// <summary>
        /// Adds a color sample to this <see cref="AverageColor"/>.
        /// </summary>
        public void Add(Color color)
        {
#if DEBUG
            if (Count == MaxSampleCount)
            {
                throw new InvalidOperationException("Too many colors added to this AverageColor");
            }
#endif

            argbc +=
                ((ulong)(uint)color.A << 48) |
                ((ulong)(uint)((color.R * color.A) / 255) << 34) |
                ((ulong)(uint)((color.G * color.A) / 255) << 20) |
                ((ulong)(uint)((color.B * color.A) / 255) << 6) |
                0x1Lu;
        }

        /// <summary>
        /// Adds one or more samples of the same color to this <see cref="AverageColor"/>.
        /// </summary>
        public void Add(Color color, int count)
        {
#if DEBUG
            if (Count + count > MaxSampleCount)
            {
                throw new InvalidOperationException("Too many colors added to this AverageColor");
            }
#endif

            argbc += (
                ((ulong)(uint)color.A << 48) |
                ((ulong)(uint)((color.R * color.A) / 255) << 34) |
                ((ulong)(uint)((color.G * color.A) / 255) << 20) |
                ((ulong)(uint)((color.B * color.A) / 255) << 6) |
                0x1Lu
                ) * (ulong)count;
        }
        
        /// <summary>
        /// Gets the average color of the samples in this <see cref="AverageColor"/>.
        /// </summary>
        public Color Color
        {
            get
            {
                var count = (int)(argbc & 0x3f);
                if (count == 0)
                {
                    return new Color();
                }

                var alphaSum = (int)(argbc >> 48);
                if (alphaSum == 0)
                {
                    return new Color();
                }
                
                return Color.FromArgb(
                    alphaSum / count,
                    ((int)((argbc >> 34) & 0x3fffLu) * 255) / alphaSum,
                    ((int)((argbc >> 20) & 0x3fffLu) * 255) / alphaSum,
                    ((int)((argbc >> 6) & 0x3fffLu) * 255) / alphaSum);
            }
        }
    }
}
