#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2020
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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Jdenticon
{
    /// <summary>
    /// Helper class for constructing and parsing list of numbers with culture-specific formatting.
    /// </summary>
    internal static class NumericList
    {
        /// <summary>
        /// Gets a suitable list separator for a specified format provider.
        /// </summary>
        /// <param name="formatProvider">Format provider used for formatting the numbers.</param>
        public static char GetSeparator(IFormatProvider formatProvider)
        {
            var numberFormat = formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
            var decimalSeparator = numberFormat?.NumberDecimalSeparator;
            return decimalSeparator.Length > 0 && decimalSeparator[0] == ',' ? ';' : ',';
        }

        /// <summary>
        /// Joins an enumeration of numeric items, using the numeric list separator.
        /// </summary>
        public static string Join<T>(IEnumerable<T> items, IFormatProvider formatProvider)
        {
            var listSeparator = GetSeparator(formatProvider) + " ";
            var result = new StringBuilder();

            foreach (var item in items)
            {
                if (result.Length > 0) result.Append(listSeparator);
                result.Append(Convert.ToString(item, formatProvider));
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets the individual numbers from a list of numbers.
        /// </summary>
        /// <param name="str">String containing a list of numbers.</param>
        /// <param name="formatProvider">Format provider used for formatting the numbers.</param>
        /// <returns>The splitted numbers.</returns>
        public static string[] Split(string str, IFormatProvider formatProvider)
        {
            if (str != null)
            {
                str = str.Trim();

                if (str.Length > 0)
                {
                    var listSeparator = GetSeparator(formatProvider);
                    return Regex.Split(str.Trim(), "\\s*" + listSeparator + "\\s*|\\s+");
                }
            }

            return new string[0];
        }
    }
}
