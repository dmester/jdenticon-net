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
