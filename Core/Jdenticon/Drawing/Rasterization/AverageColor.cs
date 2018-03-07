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
