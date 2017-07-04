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
using System.Text;

namespace Jdenticon
{
    /// <summary>
    /// Provides helper methods for creating <see cref="Range{TValue}"/> values.
    /// </summary>
    /// <example>
    /// This class is used to create <see cref="Range{TValue}"/> values without explicitly 
    /// specifying the type of the range. The following examples are equivalent.
    /// <code language="cs">
    /// Range.Create(0f, 1f);
    /// new Range&lt;float&gt;(0f, 1f);
    /// </code>
    /// </example>
    public static class Range
    {
        /// <summary>
        /// Creates a new <see cref="Range{TValue}"/> with the specified values.
        /// </summary>
        /// <typeparam name="TValue">The type of the range bounds.</typeparam>
        /// <param name="from">The lower bound of the range.</param>
        /// <param name="to">The upper bound of the range.</param>
        public static Range<TValue> Create<TValue>(TValue from, TValue to) where TValue : struct
        {
            return new Range<TValue>(from, to);
        }
    }

    /// <summary>
    /// Represents a range between two values.
    /// </summary>
    /// <typeparam name="TValue">The type of the range bounds.</typeparam>
    public struct Range<TValue> : IEquatable<Range<TValue>> where TValue : struct
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
        /// Gets a hash code for this range.
        /// </summary>
        public override int GetHashCode()
        {
            return from.GetHashCode() ^ to.GetHashCode();
        }

        /// <summary>
        /// Checks if this is range is equal to another range.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        public override bool Equals(object obj)
        {
            return obj is Range<TValue> && Equals((Range<TValue>)obj);
        }

        /// <summary>
        /// Checks if this is range is equal to another range.
        /// </summary>
        /// <param name="other">The object to compare.</param>
        public bool Equals(Range<TValue> other)
        {
            var comparer = EqualityComparer<TValue>.Default;
            return
                comparer.Equals(other.from, from) &&
                comparer.Equals(other.to, to);
        }

        /// <summary>
        /// Checks if two ranges are equal.
        /// </summary>
        /// <param name="a">The first range to compare.</param>
        /// <param name="b">The second range to compare.</param>
        public static bool operator ==(Range<TValue> a, Range<TValue> b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Checks if two ranges are not equal.
        /// </summary>
        /// <param name="a">The first range to compare.</param>
        /// <param name="b">The second range to compare.</param>
        public static bool operator !=(Range<TValue> a, Range<TValue> b)
        {
            return !a.Equals(b);
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
