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

#if HAVE_EXTENDED_COMPONENTMODEL

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Converts <see cref="Color"/> to <see cref="string"/>, and vice versa.
    /// </summary>
    public class ColorConverter : TypeConverter
    {
        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Color color)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    var constructor = typeof(Color).GetMethod(
                        nameof(Color.FromArgb), new[] { typeof(int), typeof(int), typeof(int), typeof(int) });

                    return new InstanceDescriptor(constructor, new object[] { color.A, color.R, color.G, color.B });
                }

                if (destinationType == typeof(string))
                {
                    if (color.A < 255)
                    {
                        return color.ToString("#RRGGBBAA");
                    }
                    else
                    {
                        return color.ToString("#RRGGBB");
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <inheritdoc />
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <inheritdoc />
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                return Color.Parse(str);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
#endif
