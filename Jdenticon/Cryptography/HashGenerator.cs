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
#if HAVE_HASH_ALGORITHMS
using System.Security.Cryptography;
#endif
using System.Text;

namespace Jdenticon.Cryptography
{
    /// <summary>
    /// Helper class for generating hashes for any values.
    /// </summary>
    public static class HashGenerator
    {
        /// <summary>
        /// Compute a SHA1 hash for the string representation of the specified value.
        /// </summary>
        /// <param name="value">Value to be hashed.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to use for hashing.</param>
        /// <exception cref="System.ArgumentException">The specified <paramref name="hashAlgorithmName"/> is not supported.</exception>
        /// <remarks>
        /// <para>
        /// This method will use <see cref="Object.ToString"/> to generate a string representation of 
        /// <paramref name="value"/> and then hash the UTF8 representation of the string using
        /// the specified algorithm. If <paramref name="value"/> is <c>null</c> an empty byte array
        /// is hashed.
        /// </para>
        /// <para>
        /// The hash algorithms available to be used as <paramref name="hashAlgorithmName"/> depends
        /// on the platform. For .NET Framework, please see the documentation for 
        /// <see cref="System.Security.Cryptography.HashAlgorithm.Create"/> for a complete list
        /// of available hash algorithms. For .NET Standard 1.0 only SHA1 is supported. For .NET
        /// Standard 1.3 SHA1, MD5 and SHA256 can be used as <paramref name="hashAlgorithmName"/>.
        /// </para>
        /// </remarks>
        public static byte[] ComputeHash(object value, string hashAlgorithmName)
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
            return ComputeHash(dataToHash, hashAlgorithmName);
        }

        private static byte[] ComputeHash(byte[] value, string hashAlgorithmName)
        {
#if HAVE_HASH_ALGORITHMS
            using (var hashAlgorithm = CreateHashAlgorithm(hashAlgorithmName))
            {
                return hashAlgorithm.ComputeHash(value);
            }
#else
            if (hashAlgorithmName != "SHA1")
            {
                throw new ArgumentException($"Unknown hash algorithm '{hashAlgorithmName}'.", nameof(hashAlgorithmName));
            }
            return SHA1.ComputeHash(value);
#endif
        }

#if HAVE_HASH_ALGORITHMS
        private static HashAlgorithm CreateHashAlgorithm(string name)
        {
#if HAVE_CRYPTO_CONFIG
            var obj = CryptoConfig.CreateFromName(name);
            
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
#else
            switch (name)
            {
                case "SHA1": return SHA1.Create();
                case "MD5": return MD5.Create();
                case "SHA256": return SHA256.Create();
                case "SHA384": return SHA384.Create();
                case "SHA512": return SHA512.Create();
            }
#endif

            throw new ArgumentException($"Unknown hash algorithm '{name}'.", nameof(name));
        }
#endif
    }
}
