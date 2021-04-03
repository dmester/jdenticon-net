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
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable enable

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Specifies a list of hue values as a string.
    /// </summary>
    /// <remarks>
    /// This class is available since version 3.0.0.
    /// </remarks>
#if HAVE_EXTENDED_COMPONENTMODEL
    [TypeConverter(typeof(HueStringConverter))]
#endif
    public class HueString : IFormattable, IEquatable<HueString?>
    {
        private readonly List<HueValue> hues = new List<HueValue>();
        private int? hashCode;

        private HueString() { }

        /// <summary>
        /// Creates a <see cref="HueString"/> instance containing the hue values from a specified <see cref="HueCollection"/>.
        /// </summary>
        /// <param name="collection">Hue values from this collection is added to the hue string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> was <c>null</c>.</exception>
        public HueString(HueCollection collection)
        {
            hues = new List<HueValue>(collection.Values ?? throw new ArgumentNullException(nameof(collection)));
        }

        /// <summary>
        /// Parses a list of hues specified as a string.
        /// </summary>
        /// <param name="value">Hue string to parse.</param>
        /// <remarks>
        /// <para>
        ///     This method can parse string representations of a <see cref="HueString"/> created with the 
        ///     <see cref="ToString()"/> method.
        /// </para>
        /// <para>
        ///     The value is a comma separated list of hues. The default unit is turns in the range [0, 1), but another unit can be 
        ///     specified using the suffixes below:
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>Unit</term>
        ///     <term>Suffix</term>
        ///     <term>Example</term>
        /// </listheader>
        /// <item>
        ///     <term>Degrees</term>
        ///     <term>deg</term>
        ///     <term>90deg</term>
        /// </item>
        /// <item>
        ///     <term>Radians</term>
        ///     <term>rad</term>
        ///     <term>1.4rad</term>
        /// </item>
        /// <item>
        ///     <term>Gradians</term>
        ///     <term>grad</term>
        ///     <term>100grad</term>
        /// </item>
        /// <item>
        ///     <term>Turns</term>
        ///     <term>turn</term>
        ///     <term>0.5turn</term>
        /// </item>
        /// </list>
        /// <para>
        ///     The values is specified using a period as decimal separator. Use the <see cref="HueString(string, IFormatProvider)"/> overload
        ///     to use a localized decimal separator.
        /// </para>
        /// </remarks>
        public HueString(string? value) : this(value, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        /// Parses a culture-specific list of hues specified as a string.
        /// </summary>
        /// <param name="value">Hue string to parse.</param>
        /// <param name="formatProvider">Format provider for parsing numbers.</param>
        /// <remarks>
        /// <para>
        ///     This method can parse string representations of a <see cref="HueString"/> created with the 
        ///     <see cref="ToString(IFormatProvider)"/> method.
        /// </para>
        /// <para>
        ///     Commas are used as value separators. If the specified culture uses comma as decimal separator, semicolons are used as value separators instead.
        ///     The default unit is turns in the range [0, 1), but another unit can be specified using the suffixes below:
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>Unit</term>
        ///     <term>Suffix</term>
        ///     <term>Example</term>
        /// </listheader>
        /// <item>
        ///     <term>Degrees</term>
        ///     <term>deg</term>
        ///     <term>90deg</term>
        /// </item>
        /// <item>
        ///     <term>Radians</term>
        ///     <term>rad</term>
        ///     <term>1.4rad</term>
        /// </item>
        /// <item>
        ///     <term>Gradians</term>
        ///     <term>grad</term>
        ///     <term>100grad</term>
        /// </item>
        /// <item>
        ///     <term>Turns</term>
        ///     <term>turn</term>
        ///     <term>0.5turn</term>
        /// </item>
        /// </list>
        /// </remarks>
        public HueString(string? value, IFormatProvider formatProvider)
        {
            foreach (var hueString in NumericList.Parse(value, formatProvider))
            {
                hues.Add(HueValue.Parse(hueString, formatProvider));
            }
        }

        /// <summary>
        /// Gets a <see cref="HueString"/> with no hue values.
        /// </summary>
        public static HueString Empty { get; } = new HueString();

        /// <summary>
        /// Parses a list of hues specified as a string.
        /// </summary>
        /// <param name="value">Hue string to parse.</param>
        /// <remarks>
        /// <para>
        ///     This method can parse string representations of a <see cref="HueString"/> created with the 
        ///     <see cref="ToString()"/> method.
        /// </para>
        /// <para>
        ///     The value is a comma separated list of hues. The default unit is turns in the range [0, 1), but another unit can be 
        ///     specified using the suffixes below:
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>Unit</term>
        ///     <term>Suffix</term>
        ///     <term>Example</term>
        /// </listheader>
        /// <item>
        ///     <term>Degrees</term>
        ///     <term>deg</term>
        ///     <term>90deg</term>
        /// </item>
        /// <item>
        ///     <term>Radians</term>
        ///     <term>rad</term>
        ///     <term>1.4rad</term>
        /// </item>
        /// <item>
        ///     <term>Gradians</term>
        ///     <term>grad</term>
        ///     <term>100grad</term>
        /// </item>
        /// <item>
        ///     <term>Turns</term>
        ///     <term>turn</term>
        ///     <term>0.5turn</term>
        /// </item>
        /// </list>
        /// <para>
        ///     The values is specified using a period as decimal separator. Use the <see cref="Parse(string, IFormatProvider)"/> overload
        ///     to use a localized decimal separator.
        /// </para>
        /// </remarks>
        public static HueString Parse(string? value)
        {
            return new HueString(value);
        }

        /// <summary>
        /// Parses a culture-specific list of hues specified as a string.
        /// </summary>
        /// <param name="value">Hue string to parse.</param>
        /// <param name="formatProvider">Format provider for parsing numbers.</param>
        /// <remarks>
        /// <para>
        ///     This method can parse string representations of a <see cref="HueString"/> created with the 
        ///     <see cref="ToString(IFormatProvider)"/> method.
        /// </para>
        /// <para>
        ///     Commas are used as value separators. If the specified culture uses comma as decimal separator, semicolons are used as value separators instead.
        ///     The default unit is turns in the range [0, 1), but another unit can be specified using the suffixes below:
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>Unit</term>
        ///     <term>Suffix</term>
        ///     <term>Example</term>
        /// </listheader>
        /// <item>
        ///     <term>Degrees</term>
        ///     <term>deg</term>
        ///     <term>90deg</term>
        /// </item>
        /// <item>
        ///     <term>Radians</term>
        ///     <term>rad</term>
        ///     <term>1.4rad</term>
        /// </item>
        /// <item>
        ///     <term>Gradians</term>
        ///     <term>grad</term>
        ///     <term>100grad</term>
        /// </item>
        /// <item>
        ///     <term>Turns</term>
        ///     <term>turn</term>
        ///     <term>0.5turn</term>
        /// </item>
        /// </list>
        /// </remarks>
        public static HueString Parse(string? value, IFormatProvider formatProvider)
        {
            return new HueString(value, formatProvider);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (hashCode == null)
            {
                var newCode = 0;
                var i = 0;

                foreach (var hue in hues)
                {
                    newCode = unchecked(newCode + (int)(1000 * hue.Turns) * i);
                    i = unchecked(i + i + 37);
                }

                hashCode = newCode;
            }

            return hashCode.Value;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => Equals(obj as HueString);

        /// <inheritdoc />
        public bool Equals(HueString? other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (!ReferenceEquals(this, other))
            {
                if (other.hues.Count != hues.Count)
                {
                    return false;
                }

                for (var i = 0; i < hues.Count; i++)
                {
                    if (hues[i].Turns != other.hues[i].Turns)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the two <see cref="HueString"/> have the same hue values.
        /// </summary>
        /// <param name="a">The first cohue stringlor to compare.</param>
        /// <param name="b">The second hue string to compare.</param>
        public static bool operator ==(HueString? a, HueString? b) => Equals(a, b);

        /// <summary>
        /// Checks if the two <see cref="HueString"/> have different hue values.
        /// </summary>
        /// <param name="a">The first hue string to compare.</param>
        /// <param name="b">The second hue string to compare.</param>
        public static bool operator !=(HueString? a, HueString? b) => !Equals(a, b);

        /// <summary>
        /// Gets a <see cref="HueCollection"/> containing the hues in this <see cref="HueString"/>.
        /// </summary>
        public HueCollection ToCollection() => new HueCollection(hues);

        /// <summary>
        /// Creates a string representation of these hues.
        /// The result can be parsed using <see cref="HueString(string)"/>.
        /// </summary>
        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a culture-specific string representation of these hues.
        /// The result can be parsed using <see cref="HueString(string, IFormatProvider)"/>.
        /// </summary>
        /// <param name="formatProvider">Format provider used for formatting numbers in the hue string.</param>
        public string ToString(IFormatProvider formatProvider)
        {
            // IFormattable should default to current culture when null is specified as formatProvider
            // https://docs.microsoft.com/en-us/dotnet/api/system.iformattable.tostring
            return NumericList.Join(hues, formatProvider ?? CultureInfo.CurrentCulture);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(formatProvider);
        }
    }
}
