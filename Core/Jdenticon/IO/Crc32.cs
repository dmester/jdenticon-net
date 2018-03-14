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
