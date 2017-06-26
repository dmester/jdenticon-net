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

namespace Jdenticon.Cryptography
{
    /// <summary>
    /// Computes SHA1 hashes on platforms where the <see cref="System.Security.Cryptography"/> namespace
    /// is not supported.
    /// </summary>
    internal static class SHA1
    {
        // Implementation based on FIPS 180-4.

        private static IEnumerable<uint[]> EnumerateBlocks(byte[] message)
        {
            const int BlockSizeBytes = 64;
            const int BlockSizeWords = BlockSizeBytes / sizeof(uint);
            const int MessageLengthSizeBytes = 8;
            
            // The block is extended to 80 words to allow the algorithm to reuse the same buffer 
            // for the message schedule.
            var extendedBlock = new uint[80];

            // Blocks entirely contained by message data
            var fullMessageCount = message.Length / BlockSizeBytes;
            for (var m = 0; m < fullMessageCount; m++)
            {
                ByteToWord(extendedBlock, message, m * BlockSizeBytes, BlockSizeWords);
                yield return extendedBlock;
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
            
            ByteToWord(extendedBlock, byteBlock, 0, BlockSizeWords);

            // If there is no room for the message size in this block, 
            // return the block and put the size in the following block.
            if (dataRestBytes + 1 + MessageLengthSizeBytes > BlockSizeBytes)
            {
                // Message size goes in next block
                yield return extendedBlock;
                Array.Clear(extendedBlock, 0, BlockSizeWords);
            }
            
            // Append message size to the last block
            unchecked
            {
                // SHA1 uses big-endian
                var messageSize = (ulong)(message.Length * 8);
                extendedBlock[BlockSizeWords - 2] = (uint)(messageSize >> 32);
                extendedBlock[BlockSizeWords - 1] = (uint)(messageSize);
            }
            
            yield return extendedBlock;
        }

        private static void ByteToWord(uint[] destination, byte[] source, int sourceOffset, int wordCount)
        {
            // SHA1 uses big-endian
            for (var i = 0; i < wordCount; i++)
            {
                destination[i] =
                    (uint)source[sourceOffset + i * sizeof(uint) + 0] << (3 * 8) |
                    (uint)source[sourceOffset + i * sizeof(uint) + 1] << (2 * 8) |
                    (uint)source[sourceOffset + i * sizeof(uint) + 2] << (1 * 8) |
                    source[sourceOffset + i * sizeof(uint) + 3];
            }
        }

        private static void WordToByte(byte[] destination, uint[] source)
        {
            // SHA1 uses big-endian
            unchecked
            {
                for (var i = 0; i < source.Length; i++)
                {
                    var word = source[i];
                    destination[i * 4 + 0] = (byte)(word >> (3 * 8));
                    destination[i * 4 + 1] = (byte)(word >> (2 * 8));
                    destination[i * 4 + 2] = (byte)(word >> (1 * 8));
                    destination[i * 4 + 3] = (byte)(word);
                }
            }
        }

        private static uint ROTL1(uint value)
        {
            const int Shift = 1;
            return unchecked((value << Shift) | (value >> (32 - Shift)));
        }

        private static uint ROTL5(uint value)
        {
            const int Shift = 5;
            return unchecked((value << Shift) | (value >> (32 - Shift)));
        }

        private static uint ROTL30(uint value)
        {
            const int Shift = 30;
            return unchecked((value << Shift) | (value >> (32 - Shift)));
        }

        private static uint Ch(uint x, uint y, uint z)
        {
            return (x & y) ^ ((~x) & z);
        }

        private static uint Parity(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        private static uint Maj(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }
        
        /// <summary>
        /// Computes the SHA1 hash of the specified message.
        /// </summary>
        /// <param name="message">The data to be hashed.</param>
        public static byte[] ComputeHash(byte[] message)
        {
            var hash = new uint[]
            {
                0x67452301u,
                0xefcdab89u,
                0x98badcfeu,
                0x10325476u,
                0xc3d2e1f0u
            };

            const uint K0 = 0x5a827999u;
            const uint K20 = 0x6ed9eba1u;
            const uint K40 = 0x8f1bbcdcu;
            const uint K60 = 0xca62c1d6u;

            var a = hash[0];
            var b = hash[1];
            var c = hash[2];
            var d = hash[3];
            var e = hash[4];

            foreach (var w in EnumerateBlocks(message))
            {
                unchecked
                {
                    for (var t = 16; t < 80; t++)
                    {
                        w[t] = ROTL1(w[t - 3] ^ w[t - 8] ^ w[t - 14] ^ w[t - 16]);
                    }
                    
                    for (var t = 0; t < 20; t++)
                    {
                        var T = ROTL5(a) + Ch(b,c,d) + e + K0 + w[t];
                        e = d;
                        d = c;
                        c = ROTL30(b);
                        b = a;
                        a = T;
                    }

                    for (var t = 20; t < 40; t++)
                    {
                        var T = ROTL5(a) + Parity(b, c, d) + e + K20 + w[t];
                        e = d;
                        d = c;
                        c = ROTL30(b);
                        b = a;
                        a = T;
                    }

                    for (var t = 40; t < 60; t++)
                    {
                        var T = ROTL5(a) + Maj(b, c, d) + e + K40 + w[t];
                        e = d;
                        d = c;
                        c = ROTL30(b);
                        b = a;
                        a = T;
                    }

                    for (var t = 60; t < 80; t++)
                    {
                        var T = ROTL5(a) + Parity(b, c, d) + e + K60 + w[t];
                        e = d;
                        d = c;
                        c = ROTL30(b);
                        b = a;
                        a = T;
                    }

                    hash[0] = a += hash[0];
                    hash[1] = b += hash[1];
                    hash[2] = c += hash[2];
                    hash[3] = d += hash[3];
                    hash[4] = e += hash[4];
                }
            }

            var byteHash = new byte[hash.Length * 4];
            WordToByte(byteHash, hash);
            return byteHash;
        }
    }
}
