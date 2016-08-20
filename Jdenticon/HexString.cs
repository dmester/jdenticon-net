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
using System.Linq;
using System.Text;

namespace Jdenticon
{
    internal static class HexString
    {
        private const string HexCharacters = "0123456789abcdef";

        /// <summary>
        /// Creates a hexadecimal string representation of a byte array. Alphabetical characters will be lowercase.
        /// </summary>
        public static string ToString(byte[] array)
        {
            if (array == null) throw new ArgumentNullException("array");

            var result = new char[array.Length * 2];

            for (var i = 0; i < array.Length; i++)
            {
                result[i * 2 + 0] = HexCharacters[array[i] >> 4];
                result[i * 2 + 1] = HexCharacters[array[i] & 0xf];
            }

            return new string(result);
        }
        
        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hexString">Hexadecimal string to convert.</param>
        public static byte[] ToArray(string hexString)
        {
            if (hexString == null) throw new ArgumentNullException("hexString");
            if (hexString.Length % 2 == 1) throw new FormatException("The hexadecimal string had an unexpected length.");

            var bytes = new byte[hexString.Length / 2];

            for (var i = 0; i < hexString.Length; i += 2)
            {
                var upper = char.ToLowerInvariant(hexString[i]);
                var lower = char.ToLowerInvariant(hexString[i + 1]);

                var upperIndex = HexCharacters.IndexOf(upper);
                var lowerIndex = HexCharacters.IndexOf(lower);

                if (upperIndex < 0 || lowerIndex < 0)
                {
                    throw new FormatException("Invalid characters were found in the hexadecimal string.");
                }

                bytes[i / 2] = (byte)((upperIndex << 4) | lowerIndex);
            }

            return bytes;
        }
    }
}
