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
using System.Globalization;
using System.IO;
using System.Text;

#if HAVE_HASH_ALGORITHMS
using System.Security.Cryptography;
#else
using Jdenticon.HashAlgorithms;
#endif

#nullable enable

namespace Jdenticon
{
    /// <summary>
    /// Helper class for generating hashes for any values.
    /// </summary>
    public static class HashGenerator
    {
        /// <summary>
        /// Compute a hash for the string representation of a specified value.
        /// </summary>
        /// <param name="value">Value that will be converted to a string and then hashed. Null 
        /// values are supported and handled as empty strings.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to be used for hashing.</param>
        /// <exception cref="System.ArgumentException">The specified <paramref name="hashAlgorithmName"/> is not supported.</exception>
        /// <remarks>
        /// <para>
        /// This method will use <see cref="Object.ToString"/> to generate a string representation of 
        /// <paramref name="value"/> and then hash the UTF8 representation of the string using
        /// the specified algorithm. If <paramref name="value"/> is <c>null</c> an empty byte array
        /// is hashed.
        /// </para>
        /// <note type="security">
        ///     <para>
        ///     Avoid using sensitive information as base for an icon, especially in combination with 
        ///     a weak hash algorithm like MD5 and SHA1. Consider using public information instead, 
        ///     like an id or a user name. 
        ///     </para>
        /// </note>
        /// <para>
        /// Hash algorithms available as <paramref name="hashAlgorithmName"/> depends on the platform. 
        /// The table below lists the possible values and on what platforms they are supported.
        /// </para>
        /// <list type="table">
        ///     <title>Supported hash algorithms per platform.</title>
        ///     <listheader>
        ///         <term>Hash algorithm</term>
        ///         <term>.NET Standard 1.0</term>
        ///         <term>.NET Standard 1.3</term>
        ///         <term>.NET Framework</term>
        ///     </listheader>
        ///     <item>
        ///         <term>MD5</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///     </item>
        ///     <item>
        ///         <term>SHA1</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///     </item>
        ///     <item>
        ///         <term>SHA256</term>
        ///         <term>-</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///     </item>
        ///     <item>
        ///         <term>SHA384</term>
        ///         <term>-</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///     </item>
        ///     <item>
        ///         <term>SHA512</term>
        ///         <term>-</term>
        ///         <term>Yes</term>
        ///         <term>Yes</term>
        ///     </item>
        /// </list>
        /// </remarks>
        public static byte[] ComputeHash(object? value, string hashAlgorithmName)
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
            switch (hashAlgorithmName)
            {
                case "SHA1": return SHA1.ComputeHash(value);
                case "MD5": return MD5.ComputeHash(value);
                default: throw new ArgumentException($"Unknown hash algorithm '{hashAlgorithmName}'.", nameof(hashAlgorithmName));
            }
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
