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
