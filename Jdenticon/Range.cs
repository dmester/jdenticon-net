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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon
{
    /// <summary>
    /// Provides a static class for creating <see cref="Range{TValue}"/> instances.
    /// </summary>
    public static class Range
    {
        /// <summary>
        /// Creates a new <see cref="Range{TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the range bounds.</typeparam>
        /// <param name="from">The lower bound of the range.</param>
        /// <param name="to">The upper bound of the range.</param>
        public static Range<TValue> Create<TValue>(TValue from, TValue to)
        {
            return new Range<TValue>(from, to);
        }
    }

    /// <summary>
    /// Represents a range between two values.
    /// </summary>
    /// <typeparam name="TValue">The type of the range bounds.</typeparam>
    public struct Range<TValue>
    {
        private TValue from;
        private TValue to;

        /// <summary>
        /// Creates a new <see cref="Range{TValue}"/> instance.
        /// </summary>
        /// <param name="from">The lower bound of the range.</param>
        /// <param name="to">The upper bound of the range.</param>
        public Range(TValue from, TValue to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Gets the lower bound of the range.
        /// </summary>
        public TValue From
        {
            get { return from; }
        }

        /// <summary>
        /// Gets the upper bound of the range.
        /// </summary>
        public TValue To
        {
            get { return to; }
        }

        /// <summary>
        /// Creates a string representation of this range.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", from, to);
        }
    }
}
