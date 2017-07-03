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

namespace Jdenticon.HashAlgorithms
{
    /// <summary>
    /// Computes MD5 hashes on platforms where the <see cref="System.Security.Cryptography"/> namespace
    /// is not supported.
    /// </summary>
    internal static class MD5
    {
        // This implementation is based on Wikipedia pseudo code:
        // https://en.wikipedia.org/wiki/MD5#Pseudocode

        private static readonly uint[] K = new uint[]
        {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        private static readonly int[] Shift = new int[]
        {
            7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,  7, 12, 17, 22,
            5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,  5,  9, 14, 20,
            4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,  4, 11, 16, 23,
            6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,  6, 10, 15, 21,
        };
        
        private static IEnumerable<uint[]> EnumerateBlocks(byte[] message)
        {
            const int BlockSizeBytes = 64;
            const int BlockSizeWords = BlockSizeBytes / sizeof(uint);
            const int MessageLengthSizeBytes = 8;
            
            var block = new uint[16];

            // Blocks entirely contained by message data
            var fullMessageCount = message.Length / BlockSizeBytes;
            for (var m = 0; m < fullMessageCount; m++)
            {
                ByteToWord(block, message, m * BlockSizeBytes, BlockSizeWords);
                yield return block;
            }
            
            // Final block(s)
            var byteBlock = new byte[BlockSizeBytes];

            // Rest of message
            var dataRestBytes = message.Length - fullMessageCount * BlockSizeBytes;
            if (dataRestBytes > 0)
            {
                Buffer.BlockCopy(message, fullMessageCount * BlockSizeBytes, byteBlock, 0, dataRestBytes);
            }

            // Trailing '1' bit
            byteBlock[dataRestBytes] = 0x80;
            
            ByteToWord(block, byteBlock, 0, BlockSizeWords);

            // If there is no room for the message size in this block, 
            // return the block and put the size in the following block.
            if (dataRestBytes + 1 + MessageLengthSizeBytes > BlockSizeBytes)
            {
                // Message size goes in next block
                yield return block;
                Array.Clear(block, 0, BlockSizeWords);
            }
            
            // Append message size to the last block
            unchecked
            {
                // MD5 uses little-endian
                var messageSize = (ulong)(message.Length * 8);
                block[BlockSizeWords - 2] = (uint)(messageSize);
                block[BlockSizeWords - 1] = (uint)(messageSize >> 32);
            }
            
            yield return block;
        }

        private static void ByteToWord(uint[] destination, byte[] source, int sourceOffset, int wordCount)
        {
            // MD5 uses little-endian
            for (var i = 0; i < wordCount; i++)
            {
                destination[i] =
                    (uint)source[sourceOffset + i * sizeof(uint) + 3] << (3 * 8) |
                    (uint)source[sourceOffset + i * sizeof(uint) + 2] << (2 * 8) |
                    (uint)source[sourceOffset + i * sizeof(uint) + 1] << (1 * 8) |
                    source[sourceOffset + i * sizeof(uint) + 0];
            }
        }

        private static void WordToByte(byte[] destination, uint[] source)
        {
            // MD5 uses little-endian
            unchecked
            {
                for (var i = 0; i < source.Length; i++)
                {
                    var word = source[i];
                    destination[i * 4 + 0] = (byte)(word);
                    destination[i * 4 + 1] = (byte)(word >> (1 * 8));
                    destination[i * 4 + 2] = (byte)(word >> (2 * 8));
                    destination[i * 4 + 3] = (byte)(word >> (3 * 8));
                }
            }
        }

        private static uint ROTL(uint value, int bits)
        {
            return unchecked((value << bits) | (value >> (32 - bits)));
        }
        
        /// <summary>
        /// Computes the SHA1 hash of the specified message.
        /// </summary>
        /// <param name="message">The data to be hashed.</param>
        public static byte[] ComputeHash(byte[] message)
        {
            var a0 = 0x67452301u;
            var b0 = 0xefcdab89u;
            var c0 = 0x98badcfeu;
            var d0 = 0x10325476u;
            
            foreach (var M in EnumerateBlocks(message))
            {
                var a = a0;
                var b = b0;
                var c = c0;
                var d = d0;

                unchecked
                {
                    for (var i = 0; i < 16; i++)
                    {
                        var f = (b & c) | (~b & d);
                        var g = i;

                        var dTemp = d;
                        d = c;
                        c = b;
                        b = b + ROTL((a + f + K[i] + M[g]), Shift[i]);
                        a = dTemp;
                    }

                    for (var i = 16; i < 32; i++)
                    {
                        var f = (d & b) | (~d & c);
                        var g = (5 * i + 1) & 0xf;

                        var dTemp = d;
                        d = c;
                        c = b;
                        b = b + ROTL((a + f + K[i] + M[g]), Shift[i]);
                        a = dTemp;
                    }

                    for (var i = 32; i < 48; i++)
                    {
                        var f = b ^ c ^ d;
                        var g = (3 * i + 5) & 0xf;

                        var dTemp = d;
                        d = c;
                        c = b;
                        b = b + ROTL((a + f + K[i] + M[g]), Shift[i]);
                        a = dTemp;
                    }

                    for (var i = 48; i < 64; i++)
                    {
                        var f = c ^ (b | ~d);
                        var g = (7 * i) & 0xf;

                        var dTemp = d;
                        d = c;
                        c = b;
                        b = b + ROTL((a + f + K[i] + M[g]), Shift[i]);
                        a = dTemp;
                    }

                    a0 += a;
                    b0 += b;
                    c0 += c;
                    d0 += d;
                }
            }

            var byteHash = new byte[4 * 4];
            WordToByte(byteHash, new uint[] { a0, b0, c0, d0 });
            return byteHash;
        }
    }
}
