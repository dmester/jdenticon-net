#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
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
using System.Text;

namespace Jdenticon.IO
{
    /// <summary>
    /// Computes Adler32 checksums. Implementation based on https://tools.ietf.org/html/rfc1950 section 2.2.
    /// </summary>
    internal class Adler32
    {
        private uint s1 = 1;
        private uint s2 = 0;

        public uint Value => (s2 << 16) + s1;

        public void Update(byte[] buffer, int offset, int count)
        {
            const uint Modulo = 65521;

            var s1 = this.s1;
            var s2 = this.s2;

            for (var i = 0; i < count; i++)
            {
                s1 += buffer[offset + i];

                if (s2 + s1 < s2)
                {
                    // Addition would cause overflow
                    s1 = s1 % Modulo;
                    s2 = s2 % Modulo;
                }

                s2 += s1;
            }
            
            this.s1 = s1 % Modulo;
            this.s2 = s2 % Modulo;
        }
    }
}
