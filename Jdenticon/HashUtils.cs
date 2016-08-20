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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon
{
    /// <summary>
    /// Helper class for generating hashes for any values.
    /// </summary>
    internal static class HashUtils
    {
        /// <summary>
        /// Compute a SHA1 hash for the string representation of the specified value.
        /// </summary>
        /// <param name="value">Value to be hashed.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to use for hashing.</param>
        public static byte[] ComputeHash(object value, string hashAlgorithmName)
        {
            using (var hashAlgorithm = CreateHashAlgorithm(hashAlgorithmName))
            {
                string stringData;

                var formattable = value as IFormattable;
                if (formattable != null)
                {
                    stringData = formattable.ToString(null, CultureInfo.InvariantCulture);
                }
                else if (value != null)
                {
                    stringData = value.ToString();
                }
                else
                {
                    stringData = "";
                }

                var dataToHash = Encoding.UTF8.GetBytes(stringData);
                return hashAlgorithm.ComputeHash(dataToHash);
            }
        }

        private static HashAlgorithm CreateHashAlgorithm(string name)
        {
            var obj = CryptoConfig.CreateFromName(name);
            if (obj == null)
            {
                return null;
            }

            var hashAlgorithm = obj as HashAlgorithm;
            if (hashAlgorithm != null)
            {
                return hashAlgorithm;
            }

            var disposable = obj as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            throw new ArgumentException(
                "The specified name '" + name + "' created an object of type " + obj.GetType().FullName +
                " which is not a hash algorithm.", "name");
        }
    }
}
