using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jdenticon.Tests
{
    internal class CustomCulture : IDisposable
    {
        CultureInfo previous;

        public CustomCulture()
        {
            previous = Thread.CurrentThread.CurrentCulture;

            // "fa" uses / as decimal separator
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fa");
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = previous;
        }
    }
}
