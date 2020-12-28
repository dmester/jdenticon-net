using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.Wpf.Converters
{
    public class FloatRangeConverter : RangeConverter
    {
        public FloatRangeConverter() : base(typeof(Range<float>))
        {
        }
    }
}
