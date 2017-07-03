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
    /// Computes CRC32 checksums. Implementation based on https://www.w3.org/TR/PNG/#D-CRCAppendix.
    /// </summary>
    internal class Crc32
    {
        private static readonly uint[] crcTable = MakeCrcTable();
        private uint crc;

        /// <summary>
        /// Creates an instance of <see cref="Crc32"/>.
        /// </summary>
        public Crc32(uint seed = 0xffffffffu)
        {
            crc = seed;
        }

        /// <summary>
        /// Gets the checksum of the processed data so far.
        /// </summary>
        public uint Value => crc ^ 0xffffffff;
        
        private static uint[] MakeCrcTable()
        {
            uint c;
            int n, k;

            var crcTable = new uint[256];

            for (n = 0; n < 256; n++)
            {
                c = (uint)n;
                for (k = 0; k < 8; k++)
                {
                    if ((c & 1) == 1)
                        c = 0xedb88320u ^ (c >> 1);
                    else
                        c = c >> 1;
                }
                crcTable[n] = c;
            }

            return crcTable;
        }

        /// <summary>
        /// Adds the specified data to the checksum.
        /// </summary>
        public void Update(byte[] data, int offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                crc = crcTable[(crc ^ data[offset + i]) & 0xff] ^ (crc >> 8);
            }
        }
    }
}
