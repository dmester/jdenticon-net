using System;
using System.Collections.Generic;
using System.Text;

#if !SUPPORT_NULLABLE_REFERENCE_TYPES

namespace System.Diagnostics.CodeAnalysis
{
    /// <exclude/>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class NotNullWhenAttribute : Attribute
    {
        public NotNullWhenAttribute(bool returnValue) { }
    }
}

#endif
