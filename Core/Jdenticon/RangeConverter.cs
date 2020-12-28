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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace Jdenticon
{
    public class RangeConverter : TypeConverter
    {
        private readonly Type rangeType;

        public RangeConverter()
        {
        }

        protected RangeConverter(Type type)
        {
            rangeType = type;
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
                var propertyType = rangeType ?? context.PropertyDescriptor?.PropertyType;
                if (propertyType != null && propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Range<>))
                {
                    var rangeValueType = propertyType.GetGenericArguments()[0];

                    var parts = NumericList.Split(str, culture);

                    if (parts.Length > 0 && parts.Length < 3)
                    {
                        object from;
                        object to;

                        if (parts.Length == 1)
                        {
                            from = to = Convert.ChangeType(parts[0], rangeValueType, culture);
                        }
                        else
                        {
                            from = Convert.ChangeType(parts[0], rangeValueType, culture);
                            to = Convert.ChangeType(parts[1], rangeValueType, culture);
                        }

                        return Activator.CreateInstance(propertyType, from, to);
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                var rangeType = value.GetType();
                if (rangeType.IsGenericType && rangeType.GetGenericTypeDefinition() == typeof(Range<>))
                {
                    var from = rangeType.GetProperty(nameof(Range<int>.From)).GetValue(value, null);
                    var to = rangeType.GetProperty(nameof(Range<int>.To)).GetValue(value, null);

                    if (destinationType == typeof(InstanceDescriptor))
                    {
                        var rangeValueType = rangeType.GetGenericArguments()[0];
                        var constructor = rangeType.GetConstructor(new[] { rangeValueType, rangeValueType });

                        if (constructor != null)
                        {
                            return new InstanceDescriptor(constructor, new object[] { from, to }, true);
                        }
                    }
                    else if (destinationType == typeof(string))
                    {
                        var result = Convert.ToString(from, culture);

                        if (!Equals(from, to))
                        {
                            result += NumericList.GetSeparator(culture) + " " + Convert.ToString(to, culture);
                        }

                        return result;
                    }
                }
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
#endif
