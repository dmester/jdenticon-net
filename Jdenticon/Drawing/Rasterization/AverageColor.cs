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
        ulong argbnc;
        
        /// <summary>
        /// Removes all samples from this <see cref="AverageColor"/>.
        /// </summary>
        public void Clear()
        {
            argbnc = 0;
        }

        /// <summary>
        /// Gets the number of colors in this <see cref="AverageColor"/>.
        /// </summary>
        public int Count
        {
            get { return (int)(argbnc & 0x3fLu); }
        }

        /// <summary>
        /// Adds a color sample to this <see cref="AverageColor"/>.
        /// </summary>
        public void Add(Color color)
        {
#if DEBUG
            if (Count == 32)
            {
                throw new InvalidOperationException("Too many colors added to this AverageColor");
            }
#endif

            if (color.A != 0)
            {
                argbnc +=
                    ((ulong)(uint)color.A << 51) |
                    ((ulong)(uint)color.R << 38) |
                    ((ulong)(uint)color.G << 25) |
                    ((ulong)(uint)color.B << 12) |
                    0x41Lu;
            }
            else
            {
                argbnc += 1;
            }
        }

        /// <summary>
        /// Adds one or more samples of the same color to this <see cref="AverageColor"/>.
        /// </summary>
        public void Add(Color color, int count)
        {
#if DEBUG
            if (Count == 32)
            {
                throw new InvalidOperationException("Too many colors added to this AverageColor");
            }
#endif

            if (color.A != 0)
            {
                argbnc += (
                    ((ulong)(uint)color.A << 51) |
                    ((ulong)(uint)color.R << 38) |
                    ((ulong)(uint)color.G << 25) |
                    ((ulong)(uint)color.B << 12) |
                    0x41Lu
                    ) * (ulong)count;
            }
            else
            {
                argbnc += (ulong)count;
            }
        }

        /// <summary>
        /// Gets the average color of the samples in this <see cref="AverageColor"/>.
        /// </summary>
        public Color Color
        {
            get
            {
                var nonEmptyCount = (int)((argbnc >> 6) & 0x3f);

                if (nonEmptyCount == 0)
                {
                    return new Color();
                }

                return Color.FromArgb(
                    (int)(argbnc >> 51) / (int)(argbnc & 0x3fLu),
                    (int)((argbnc >> 38) & 0x1fffLu) / nonEmptyCount,
                    (int)((argbnc >> 25) & 0x1fffLu) / nonEmptyCount,
                    (int)((argbnc >> 12) & 0x1fffLu) / nonEmptyCount);
            }
        }
    }
}
